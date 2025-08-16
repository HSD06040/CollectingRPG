using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Unit_Data", menuName = "Data/Unit/Unit_Data")]
public class UnitData : ScriptableObject
{
    [Header("MetaData")]
    public int ID;
    public string Name;
    [TextArea]
    public string Description;
    public int Cost;
    public int CombatPower;

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
    public int CritDamage;

    [Header("Defense")]
    public int PhysicalDefense;
    public int MagicDefense;

    [Header("Range")]
    public int AttackRange;
    public int AttackCount;
    public AttackAreaType AttackAreaType;

    [Header("Attack_Data")]
    public UnitSkill Skill;
    public UnitAttackData AttackData; // Melee, Ranged 등 공격 타입에 따라 다름

    [Header("Player_Enhancement")]
    public UnitEnhancementData EnhancementData; // 적일 경우 더미 데이터로 존재 (추후 기획에 따라 달라질 수 있음)
}
