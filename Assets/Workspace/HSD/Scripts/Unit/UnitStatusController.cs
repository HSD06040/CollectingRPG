using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitStatusController : MonoBehaviour, IDamageable
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
    public Stat<int> CritDamage;

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
    public Property<int> TotalDamage = new Property<int>();

    public event Action OnPlayerDied;
    public bool IsDead => CurHp.Value <= 0;

    public void Init(UnitStatus status)
    {
        Status = status;
        SetBaseStat(status.GetCurrentStat());
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
        CritDamage.SetBaseStat(stat.CritDamage);

        PhysicalDefense.SetBaseStat(stat.PhysicalDefense);
        MagicDefense.SetBaseStat(stat.MagicDefense);

        AttackRange.SetBaseStat(stat.AttackRange);
        AttackCount.SetBaseStat(stat.AttackCount);

        CurHp.Value = MaxHealth.Value;
        CurMana.Value = MaxMana.Value;
        TotalDamage.Value = 0;
    }

    public void TakeDamage(int amount)
    {
        CurHp.Value -= amount;

        if(CurHp.Value <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        OnPlayerDied?.Invoke();
    }

    public void GetMana()
    {
        CurMana.Value += ManaGain.Value;
    }
}
