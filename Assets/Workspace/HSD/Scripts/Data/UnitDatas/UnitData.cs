using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Unit_Data", menuName = "Data/Unit/Unit_Data")]
public class UnitData : ScriptableObject
{
    public int ID;
    public string Name;
    [TextArea]
    public string Description;

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
    public AttackAreaType AttackAreaType;

    [Header("Attack_Data")]
    public UnitSkill Skill;
    public UnitAttackData AttackData; // Melee, Ranged 등 공격 타입에 따라 다름

    [Header("Player_Enhancement")]
    public UnitEnhancementData EnhancementData; // 적일 경우 더미 데이터로 존재 (추후 기획에 따라 달라질 수 있음)

    public float GetAttackTime()
    {
        return 1 / AttackSpeed.Value;
    }    
}
