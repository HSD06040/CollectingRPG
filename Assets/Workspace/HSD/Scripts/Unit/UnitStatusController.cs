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
    public Property<bool> IsStund = new Property<bool>();
    public UnitPassiveController PassiveController { get; set; }

    public event Action<UnitStatusController> OnUnitDied;
    public Action<UnitStatus> OnUseSkill;
    
    public event Action OnDied;
    public Action OnSkill;
    public Action OnAttack;

    private readonly Dictionary<SourceKey, CancellationTokenSource> _activeBuffs = new Dictionary<SourceKey, CancellationTokenSource>(10);

    [HideInInspector] public float StatMultiplier = 0;

    public bool IsDead;

    private void OnDestroy()
    {
        PassiveController?.DeActiveAllPassive();
    }

    #region Init&Clear
    public void Init(UnitStatus status, UnitStats plusUnitStat = null)
    {
        PassiveController = new UnitPassiveController(this);
        Status = status;
        IsDead = false;
        IsStund.Value = false;

        if (plusUnitStat == null)
        {
            SetBaseStat(status.GetCurrentStat());
        }            
        else
        {
            SetBaseStat(status.GetCurrentStat(), plusUnitStat);
        }
    }

    #region SetStat
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

    private void SetBaseStat(UnitStats baseStat, UnitStats plusStat)
    {
        var stat = new UnitStats
        {
            MaxHealth = Mathf.RoundToInt(baseStat.MaxHealth + plusStat.MaxHealth * StatMultiplier),
            MaxMana = Mathf.RoundToInt(baseStat.MaxMana + plusStat.MaxMana * StatMultiplier),
            ManaGain = Mathf.RoundToInt(baseStat.ManaGain + plusStat.ManaGain * StatMultiplier),

            AttackSpeed = baseStat.AttackSpeed + plusStat.AttackSpeed * StatMultiplier,
            MoveSpeed = baseStat.MoveSpeed + plusStat.MoveSpeed * StatMultiplier,

            PhysicalDamage = Mathf.RoundToInt(baseStat.PhysicalDamage + plusStat.PhysicalDamage * StatMultiplier),
            MagicDamage = Mathf.RoundToInt(baseStat.MagicDamage + plusStat.MagicDamage * StatMultiplier),

            CritChance = Mathf.RoundToInt(baseStat.CritChance + plusStat.CritChance * StatMultiplier),

            PhysicalDefense = Mathf.RoundToInt(baseStat.PhysicalDefense + plusStat.PhysicalDefense * StatMultiplier),
            MagicDefense = Mathf.RoundToInt(baseStat.MagicDefense + plusStat.MagicDefense * StatMultiplier),

            AttackRange = Mathf.RoundToInt(baseStat.AttackRange + plusStat.AttackRange * StatMultiplier),
            AttackCount = Mathf.RoundToInt(baseStat.AttackCount + plusStat.AttackCount * StatMultiplier),
        };

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
    #endregion

    public void ClearAllStat()
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

    #region TakeDamage
    public void TakeDamage(int amount, bool isCrit = false)
    {
        if(IsDead)
            return;

        Manager.Pool.GetPopUp(transform.position).Init(amount, isCrit);

        CurHp.Value = Mathf.Clamp(CurHp.Value - amount, 0, int.MaxValue);


        if(CurHp.Value < 0)
        {
            Die();
        }
    }
    
    public void TakeTickDamage(int amount, float tickCount, float tickInterval)
    {
        TickDamage(amount, tickCount, tickInterval).Forget();
    }

    private async UniTask TickDamage(int amount, float tickCount, float tickInterval)
    {
        var destroyToken = this.GetCancellationTokenOnDestroy();
        int count = 0;

        while (tickCount > count)
        {
            count++;
            if (IsDead)
                return;

            TakeDamage(amount);

            try
            {
                await UniTask.Delay(TimeSpan.FromSeconds(tickInterval), cancellationToken: destroyToken);
            }
            catch (OperationCanceledException)
            {
                return;
            }            
        }
    }
    #endregion

    #region Increase
    public void IncreaseHealth(int amount)
    {
        CurHp.Value += amount;

        if(CurHp.Value > MaxHealth.Value)
        {
            CurHp.Value = MaxHealth.Value;
        }
    }

    public void IncreaseMana(int amount)
    {
        CurMana.Value += amount;

        if (CurMana.Value > MaxMana.Value)
        {
            CurMana.Value = MaxMana.Value;
        }
    }
    public void GetMana()
    {
        IncreaseMana(ManaGain.Value);
    }

    public void IncreaseShield(int amount)
    {        
        Shield.Value += amount;
    }
    #endregion

    public void Stun(float stunDuration)
    {
        StunDelay(stunDuration).Forget();
    }

    private async UniTask StunDelay(float stunTime)
    {
        IsStund.Value = true;

        await UniTask.Delay(TimeSpan.FromSeconds(stunTime));

        if (IsDead)
            return;

        IsStund.Value = false;
    }

    private void Die()
    {
        IsDead = true;
        OnDied?.Invoke();
        OnUnitDied?.Invoke(this);
    }

    #region Effect
    public void ApplyEffect(BuffEffectData buffEffectData, float value, string source)
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

        ClearEffectAsync(buffEffectData, source, newCts.Token).Forget();
    }

    private async UniTaskVoid ClearEffectAsync(BuffEffectData buffEffectData, string source, CancellationToken token)
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
    #endregion

    #region Stat Management
    public void AddStat(StatType statType, float value, string source)
    {
        //Debug.Log($"AddStat: {statType}, Value: {value}, Source: {source}");
        switch (statType)
        {
            case StatType.MaxHealth:
                MaxHealth.AddModifier((int)value, source);
                IncreaseHealth((int)value);
                break;
            case StatType.MaxMana:
                MaxMana.AddModifier((int)value, source);
                break;
            case StatType.ManaGain:
                ManaGain.AddModifier((int)value, source);
                break;
            case StatType.PhysicalDamage:
                PhysicalDamage.AddModifier((int)value, source);
                break;
            case StatType.MagicDamage:
                MagicDamage.AddModifier((int)value, source);
                break;
            case StatType.CritChance:
                CritChance.AddModifier((int)value, source);
                break;
            case StatType.PhysicalDefense:
                PhysicalDefense.AddModifier((int)value, source);
                break;
            case StatType.MagicDefense:
                MagicDefense.AddModifier((int)value, source);
                break;
            case StatType.AttackSpeed:
                AttackSpeed.AddModifier(value, source);
                break;
            case StatType.CurHp:
                IncreaseHealth((int)value);
                break;
            case StatType.CurMana:
                IncreaseMana((int)value);
                break;
            case StatType.Shield:
                IncreaseShield((int)value);
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
            case StatType.AttackSpeed:
                AttackSpeed.RemoveModifier(source);
                break;                
        }
    }
    #endregion
}
