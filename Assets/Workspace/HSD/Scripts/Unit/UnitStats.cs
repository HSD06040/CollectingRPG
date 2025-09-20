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

    public void AddStat(UnitStats stats, StatEffectModifier modifier, bool persent)
    {        
        switch (modifier.StatType)
        {
            case StatType.MaxHealth:
                break;
            case StatType.MaxMana:
                break;
            case StatType.ManaGain:
                break;
            case StatType.AttackSpeed:
                break;
            case StatType.MoveSpeed:
                break;
            case StatType.PhysicalDamage:
                break;
            case StatType.MagicDamage:
                break;
            case StatType.CritChance:
                break;
            case StatType.CritDamage:
                break;
            case StatType.PhysicalDefense:
                break;
            case StatType.MagicDefense:
                break;
            case StatType.AttackRange:
                break;
            case StatType.AttackCount:
                break;
            case StatType.CurHp:
                break;
            case StatType.CurMana:
                break;
            case StatType.GoldBonus:
                break;
        }
    }
}
