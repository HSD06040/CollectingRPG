using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitStatusController : MonoBehaviour, IDamageable
{
    public UnitData Data { get; set; }

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

    public void Init(UnitData data)
    {
        Data = data;
        SetBaseStat();
    }

    private void SetBaseStat()
    {
        MaxHealth.SetBaseStat(Data.MaxHealth);
        MaxMana.SetBaseStat(Data.MaxMana);
        ManaGain.SetBaseStat(Data.ManaGain);

        AttackSpeed.SetBaseStat(Data.AttackSpeed);
        MoveSpeed.SetBaseStat(Data.MoveSpeed);

        PhysicalDamage.SetBaseStat(Data.PhysicalDamage);
        MagicDamage.SetBaseStat(Data.MagicDamage);

        CritChance.SetBaseStat(Data.CritChance);
        CritDamage.SetBaseStat(Data.CritDamage);

        PhysicalDefense.SetBaseStat(Data.PhysicalDefense);
        MagicDefense.SetBaseStat(Data.MagicDefense);

        AttackRange.SetBaseStat(Data.AttackRange);
        AttackCount.SetBaseStat(Data.AttackCount);

        CurHp.Value = MaxHealth.Value;
        CurMana.Value = MaxMana.Value;
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
