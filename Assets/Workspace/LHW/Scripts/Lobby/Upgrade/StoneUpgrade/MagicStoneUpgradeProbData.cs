using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MagicStoneUpgradeProbData", menuName = "Data/Upgrade/MagicStoneUpgradeProbData")]
public class MagicStoneUpgradeProbData : ScriptableObject
{
    public List<MagicStoneLevelGrowth> StoneUpgradeProbs;

    public List<SubGradeProb> GetCurrentLevelProbData(int level)
    {
        MagicStoneLevelGrowth levelGrowth = StoneUpgradeProbs.Find(l => l.Level == level);
        if (levelGrowth.Equals(default(MagicStoneLevelGrowth)))
        {
            Debug.LogWarning($"[MagicStoneUpgradeProbData] 해당 Level({level}) 데이터가 없음");
            return null;
        }

        return levelGrowth.GradeProbs;
    }
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