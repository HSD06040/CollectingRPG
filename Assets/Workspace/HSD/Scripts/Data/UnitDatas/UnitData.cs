using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Unit_Data", menuName = "Data/Unit/Unit_Data")]
public class UnitData : ScriptableObject
{
    [Header("MetaData")]  
    public Grade Grade;
    public string AddressableAddress;
    public GameObject UnitPrefab => Manager.Resources.Get<GameObject>(AddressableAddress);
    public Sprite Icon;
    public int ID;
    public int PerferredLine;
    public string Name;

    [TextArea]
    public string Description;
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
    public UnitUpgradeData UpgradeData;

    public UnitStats GetUnitStat(int level)
    {
        return UnitStats[level];
    }
}
