using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeStatData", menuName = "Data/Upgrade/UpgradeStatData")]
public class UpgradStatData : ScriptableObject
{
    public List<GradeGrowth> StatData;
}

[Serializable]
public struct GradeGrowth
{
    public Grade CharGrade;
    public List<LevelGrowth> Stats;
}

[Serializable]
public struct LevelGrowth
{
    public int Level;
    public List<StatusGrowth> Stats;
}

[Serializable]
public struct StatusGrowth
{
    public StatType Type;
    public float Value;
}