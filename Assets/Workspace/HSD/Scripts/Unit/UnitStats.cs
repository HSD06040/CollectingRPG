using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UnitStats
{
    [Header("Status")]
    public int MaxHealth;
    public int MaxMana;
    public int ManaGain;
    public float AttackSpeed;
    public float MoveSpeed;

    [Header("Damage")]
    public int PhysicalDamage;
    public int MagicDamage;

    [Header("CritRate")]
    public int CritChance;

    [Header("Defense")]
    public int PhysicalDefense;
    public int MagicDefense;

    [Header("Range")]
    public float AttackRange;
    public int AttackCount;
}
