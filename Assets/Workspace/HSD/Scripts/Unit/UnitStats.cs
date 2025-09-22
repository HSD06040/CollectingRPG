using System;
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
    public void AddAugments(StatType status, float rate, string name)
    {
        StatEffectModifier modifier = CalculateAugment(status, rate);
        AugmentManager.Instance.AddAugment(modifier.StatType, modifier.Value, name);
    }

    public StatEffectModifier CalculateAugment(StatType status, float rate)
    {
        StatEffectModifier modifier = new StatEffectModifier();
        modifier.StatType = status;
        switch (modifier.StatType)
        {
            case StatType.MaxHealth: modifier.Value = Mathf.RoundToInt(MaxHealth * (rate / 100)); break;
            case StatType.MaxMana: modifier.Value = Mathf.RoundToInt(MaxMana * (rate / 100)); break;
            case StatType.ManaGain: modifier.Value = Mathf.RoundToInt(ManaGain * (rate / 100)); break;
            case StatType.AttackSpeed: modifier.Value = Mathf.RoundToInt(AttackSpeed * (rate / 100)); break;
            case StatType.MoveSpeed: modifier.Value = Mathf.RoundToInt(MoveSpeed * (rate / 100)); break;
            case StatType.PhysicalDamage: modifier.Value = Mathf.RoundToInt(PhysicalDamage * (rate / 100)); break;
            case StatType.MagicDamage: modifier.Value = Mathf.RoundToInt(MagicDamage * (rate / 100)); break;
            case StatType.CritChance: modifier.Value = Mathf.RoundToInt(CritChance * (rate / 100)); break;
            case StatType.PhysicalDefense: modifier.Value = Mathf.RoundToInt(PhysicalDefense * (rate / 100)); break;
            case StatType.MagicDefense: modifier.Value = Mathf.RoundToInt(MagicDefense * (rate / 100)); break;
            case StatType.AttackRange: modifier.Value = Mathf.RoundToInt(AttackRange * (rate / 100)); break;
            case StatType.AttackCount: modifier.Value = Mathf.RoundToInt(AttackCount * (rate / 100)); break;
            default: modifier.Value = 0; break;
        }

        return modifier;
    }

    public void RemoveAugments(StatType status, string name)
    {
        AugmentManager.Instance.RemoveAugment(status, name);
    }
}
