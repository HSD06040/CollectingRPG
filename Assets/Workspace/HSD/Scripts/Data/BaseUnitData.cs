using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UnitData", menuName = "Data/Unit/UnitData")]
public class BaseUnitData : ScriptableObject
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

    [Header("Skill")]
    public UnitSkill UnitSkill;

    [Header("AdvancedUnitData")]
    public UnitData AdvancedData;

    public float GetAttackTime()
    {
        return AttackSpeed.Value / 1;
    }    
}
