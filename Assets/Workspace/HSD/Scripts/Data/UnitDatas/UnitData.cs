using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Unit_Data", menuName = "Data/Unit/Unit_Data")]
public class UnitData : ScriptableObject
{
    [Header("MetaData")]    
    public Grade Grade;
    public GameObject UnitPrefab; // Test
    public Sprite Icon;
    public int ID;
    public string Name;

    [TextArea]
    public string Description;

    public int Cost;
    public int UpgradeCount;

    [Header("Attack_Data")]
    public UnitSkill Skill;
    public UnitAttackData AttackData; // Melee, Ranged 등 공격 타입에 따라 다름

    [Header("Player_Enhancement")]
    public UnitEnhancementData EnhancementData; // 적일 경우 더미 데이터로 존재 (추후 기획에 따라 달라질 수 있음)
    public UnitStats[] UnitStats;

    public UnitStats GetUnitStat(int level)
    {
        return UnitStats[level];
    }
}
