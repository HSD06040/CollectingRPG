using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MagicStoneUpgradeProbData", menuName = "Data/Upgrade/MagicStoneUpgradeProbData")]
public class MagicStoneUpgradeProbData : ScriptableObject
{
    public List<MagicStoneLevelGrowth> StoneUpgradeProbs;
}

[Serializable]
public struct MagicStoneLevelGrowth
{
    public int Level;
    public List<SubGradeProb> GradeProbs;
}

[Serializable]
public struct SubGradeProb
{
    public SubGrade Grade;
    public float Probable;
}