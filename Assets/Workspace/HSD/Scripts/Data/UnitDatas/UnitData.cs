using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Unit_Data", menuName = "Data/Unit/Unit_Data")]
public class UnitData : MetaData
{
    [Header("MetaData")]    
    public string AddressableAddress;
    public GameObject UnitPrefab => Manager.Resources.Get<GameObject>(AddressableAddress);
    public int ID;
    public int PerferredLine;
    public int Cost;

    [Header("Attack_Data")]
    public UnitSkill Skill;
    public UnitAttackData AttackData; // Melee, Ranged 등 공격 타입에 따라 다름

    [Header("Unit_Stat")]    
    public UnitStats[] UnitStats;

    [Header("Synergy")]
    public ClassType ClassSynergy;
    public Synergy Synergy;

    [Header("Upgrade")]
    public int UpgradeCount;
    public UpgradeUnitData UpgradeData;
    public LevelUpData LevelUpData;

    public UnitStats GetUnitStat(int level)
    {
        return UnitStats[level];
    }

    public void Init()
    {
        UpgradeData?.Init(Grade, LevelUpData);
    }
}
