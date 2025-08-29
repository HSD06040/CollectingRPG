using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class UnitStatusController : MonoBehaviour, IDamageable, IEffectable
{
    public UnitStatus Status { get; set; }

    #region Stat
    [Header("Status")]
    public Stat<int> MaxHealth;
    public Stat<int> MaxMana;
    public Stat<int> ManaGain;
    public Stat<float> AttackSpeed;
    public Stat<float> MoveSpeed;

    [Header("Damage")]
    public Stat<int> PhysicalDamage;
    public Stat<int> MagicDamage;

    [Header("CritRate")]
    public Stat<int> CritChance;

    [Header("Defense")]
    public Stat<int> PhysicalDefense;
    public Stat<int> MagicDefense;

    [Header("Range")]
    public Stat<float> AttackRange;
    public Stat<int> AttackCount;
    public float DetectionRange = 5f;
    #endregion

    public Property<int> CurHp = new Property<int>();
    public Property<int> CurMana = new Property<int>();
    public Property<int> Shield = new Property<int>();
    public Property<int> TotalDamage = new Property<int>();
    public UnitPassiveController PassiveController { get; set; }

    public event Action<UnitStatusController> OnUnitDied;
    public Action<UnitStatus> OnUseSkill;
    public Action OnAttack;

    private readonly Dictionary<SourceKey, CancellationTokenSource> _activeBuffs = new Dictionary<SourceKey, CancellationTokenSource>(10);


    public bool IsDead => CurHp.Value <= 0;

    #region Init&Clear
    public void Init(UnitStatus status)
    {
        PassiveController = new UnitPassiveController(this);
        Status = status;
        SetBaseStat(status.GetCurrentStat());
        ClearAllStat();
    }

    private void SetBaseStat(UnitStats stat)
    {
        MaxHealth.SetBaseStat(stat.MaxHealth);
        MaxMana.SetBaseStat(stat.MaxMana);
        ManaGain.SetBaseStat(stat.ManaGain);

        AttackSpeed.SetBaseStat(stat.AttackSpeed);
        MoveSpeed.SetBaseStat(stat.MoveSpeed);

        PhysicalDamage.SetBaseStat(stat.PhysicalDamage);
        MagicDamage.SetBaseStat(stat.MagicDamage);

        CritChance.SetBaseStat(stat.CritChance);

        PhysicalDefense.SetBaseStat(stat.PhysicalDefense);
        MagicDefense.SetBaseStat(stat.MagicDefense);

        AttackRange.SetBaseStat(stat.AttackRange);
        AttackCount.SetBaseStat(stat.AttackCount);

        CurHp.Value = MaxHealth.Value;
        CurMana.Value = MaxMana.Value;
        TotalDamage.Value = 0;
    }

    private void ClearAllStat()
    {
        // 모든 스탯의 모디파이어 제거
        MaxHealth.ClearModifiers();
        MaxMana.ClearModifiers();
        ManaGain.ClearModifiers();

        AttackSpeed.ClearModifiers();
        MoveSpeed.ClearModifiers();

        PhysicalDamage.ClearModifiers();
        MagicDamage.ClearModifiers();

        CritChance.ClearModifiers();

        PhysicalDefense.ClearModifiers();
        MagicDefense.ClearModifiers();

        AttackRange.ClearModifiers();
        AttackCount.ClearModifiers();

        foreach (var cts in _activeBuffs.Values)
        {
            cts.Cancel();
            cts.Dispose();
        }
        _activeBuffs.Clear();
    }
    #endregion

    public void TakeDamage(int amount)
    {
        if(IsDead)
            return;

        CurHp.Value = Mathf.Clamp(CurHp.Value - amount,0, int.MaxValue);

        if(CurHp.Value == 0)
        {
            Die();
        }
    }

    public void IncreaseHealth(int amount)
    {
        CurHp.Value = Mathf.Clamp(CurHp.Value + amount, 0, MaxHealth.Value);
    }

    public void IncreaseMana(int amount)
    {
        CurMana.Value = Mathf.Clamp(CurMana.Value + amount, 0, MaxMana.Value);
    }

    private void Die()
    {
        OnUnitDied?.Invoke(this);
    }

    public void GetMana()
    {
        IncreaseMana(ManaGain.Value);
    }

    public void ApplyEffect(BuffEffectData buffEffectData, int value, string source)
    {
        var key = new SourceKey(buffEffectData.StatType, source);

        if (_activeBuffs.TryGetValue(key, out var cts))
        {
            cts.Cancel();
            cts.Dispose();
            _activeBuffs.Remove(key);
        }
        else
        {
            AddStat(buffEffectData.StatType, value, source);
        }

        var newCts = new CancellationTokenSource();
        _activeBuffs[key] = newCts;

        ClearEffectAsync(buffEffectData, value, source, newCts.Token).Forget();
    }

    private async UniTaskVoid ClearEffectAsync(BuffEffectData buffEffectData, int value, string source, CancellationToken token)
    {
        try
        {
            await UniTask.Delay(TimeSpan.FromSeconds(buffEffectData.Duration), cancellationToken: token);

            RemoveStat(buffEffectData.StatType, source);
            _activeBuffs.Remove(new SourceKey(buffEffectData.StatType, source));
        }
        catch (OperationCanceledException)
        {
            // 갱신으로 취소된 경우 RemoveStat 안 함
        }
    }

    public void AddStat(StatType statType, int value, string source)
    {
        switch(statType)
        {
            case StatType.MaxHealth:
                MaxHealth.AddModifier(value, source);
                IncreaseHealth(value);
                break;
            case StatType.MaxMana:
                MaxMana.AddModifier(value, source);
                break;
            case StatType.ManaGain:
                ManaGain.AddModifier(value, source);
                break;
            case StatType.PhysicalDamage:
                PhysicalDamage.AddModifier(value, source);
                break;
            case StatType.MagicDamage:
                MagicDamage.AddModifier(value, source);
                break;
            case StatType.CritChance:
                CritChance.AddModifier(value, source);
                break;
            case StatType.PhysicalDefense:
                PhysicalDefense.AddModifier(value, source);
                break;
            case StatType.MagicDefense:
                MagicDefense.AddModifier(value, source);
                break;
            case StatType.CurHp:
                IncreaseHealth(value);
                break;
            case StatType.CurMana:
                IncreaseMana(value);
                break;
        }
    }

    public void RemoveStat(StatType statType, string source)
    {
        switch (statType)
        {
            case StatType.MaxHealth:
                MaxHealth.RemoveModifier(source);
                break;
            case StatType.MaxMana:
                MaxMana.RemoveModifier(source);
                break;
            case StatType.ManaGain:
                ManaGain.RemoveModifier(source);
                break;
            case StatType.PhysicalDamage:
                PhysicalDamage.RemoveModifier(source);
                break;
            case StatType.MagicDamage:
                MagicDamage.RemoveModifier(source);
                break;
            case StatType.CritChance:
                CritChance.RemoveModifier(source);
                break;
            case StatType.PhysicalDefense:
                PhysicalDefense.RemoveModifier(source);
                break;
            case StatType.MagicDefense:
                MagicDefense.RemoveModifier(source);
                break;
        }
    }
}
