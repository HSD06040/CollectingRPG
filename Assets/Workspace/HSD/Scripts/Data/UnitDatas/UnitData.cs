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

    [Header("AnimationData")]
    public bool isNotChange;
    public AnimatorData AnimatiorData;

    [Header("Attack_Data")]
    public UnitSkill Skill;
    public UnitAttackData AttackData; // Melee, Ranged 등 공격 타입에 따라 다름

    [Header("Unit_Stat")]    
    public UnitStats[] UnitStats; // 3개 1,2,3 성
    public UnitStats AugmentStat = new();

    [Header("Synergy")]
    public ClassType ClassSynergy;
    public Synergy Synergy;

    [Header("Upgrade")]
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

    public UnitDataDTO ToDTO(UnitData data)
    {
        return new UnitDataDTO
        {
            ClassSynergy = (int)data.ClassSynergy,
            Cost = data.Cost,
            Description = data.Description,
            Grade = data.Grade,
            ID = data.ID,
            Name = data.name,
            PrefferedLine = data.PerferredLine,
            Synergy = (int)data.Synergy,
            UnitStats = data.UnitStats
        };
    }

    public void AddStat(StatEffectModifier modifier, bool persent = false)
    {
        AugmentStat.AddStat(UnitStats[0], modifier, persent);
    }
}

[System.Serializable]
public class UnitDataDTO
{
    public int ClassSynergy;
    public int Cost;
    public string Description;
    public Grade Grade;
    public int ID;
    public string Name;
    public int PrefferedLine;
    public int Synergy;
    public UnitStats[] UnitStats;
}