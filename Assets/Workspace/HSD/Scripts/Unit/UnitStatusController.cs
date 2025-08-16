using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitStatusController : MonoBehaviour, IDamageable
{
    public UnitData Data { get; private set; }

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
    public Stat<int> AttackRange;
    public Stat<int> AttackCount;
    #endregion

    public Property<int> CurHp = new Property<int>();
    public Property<int> CurMana = new Property<int>();

    public void Init(UnitData data)
    {
        Data = data;



        CurHp.Value = MaxHealth.Value;
        CurMana.Value = MaxMana.Value;
    }

    public void TakeDamage(int amount, DamageType damageType)
    {
        int defense = damageType == DamageType.Magic ? MagicDefense.Value : PhysicalDefense.Value;
        int totalDamage = Utils.CalculateFinalDamage(amount, defense, damageType);
        CurHp.Value -= totalDamage;
        Debug.Log($"현재 체력 :{CurHp.Value}, 받은 데미지 : {totalDamage}");
    }

    public void GetMana()
    {
        CurMana.Value += ManaGain.Value;
    }
}
