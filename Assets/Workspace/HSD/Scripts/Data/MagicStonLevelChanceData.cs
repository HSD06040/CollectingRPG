using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MagicStonLevelChanceData", menuName = "Data/MagicStone/MagicStonLevelChanceData")]
public class MagicStonLevelChanceData : ScriptableObject
{
    public MagicStoneLevelChances[] MagicStonLevelsChanceDatas;
    private static readonly Dictionary<int, MagicStoneLevelChances> _magicStonLevelChanceDic = new Dictionary<int, MagicStoneLevelChances>();
    public MagicStoneLevelUpData MagicStoneLevelUpData;

    public void Init()
    {
        for (int i = 0; i < MagicStonLevelsChanceDatas.Length; i++)
        {
            int level = i;
            if (!_magicStonLevelChanceDic.ContainsKey(level))
            {
                _magicStonLevelChanceDic.Add(level, MagicStonLevelsChanceDatas[i]);
            }
        }
    }

    public MagicStoneLevelChances GetMagicStoneLevelChances(int level)
    {
        if (!_magicStonLevelChanceDic.ContainsKey(level))
        {
            Init();
        }

        return _magicStonLevelChanceDic[level];
    }

    public SubGrade GetGrade(int level)
    {
        if (!_magicStonLevelChanceDic.ContainsKey(level))
        {
            Init();
        }

        if (!_magicStonLevelChanceDic.TryGetValue(level, out var levelData))
        {
            Debug.LogWarning($"[MagicStonLevelChanceData] Level {level} not found");
            return SubGrade.SILVER;
        }

        var chances = levelData.MagicStonLevelChanceDatas;
        if (chances == null || chances.Length == 0)
        {
            Debug.LogWarning($"[MagicStonLevelChanceData] Level {level} has no chance");
            return SubGrade.SILVER;
        }

        float totalChance = 0f;
        foreach (var chance in chances)
        {
            totalChance += chance.Chance;
        }

        float randomValue = Random.Range(0f, totalChance);
        float cumulative = 0f;

        foreach (var chance in chances)
        {
            cumulative += chance.Chance;
            if (randomValue <= cumulative)
            {
                return chance.Grade;
            }
        }

        return chances[chances.Length - 1].Grade;
    }
}

[System.Serializable]
public class MagicStoneLevelChances
{
    public SubGradeChanceData[] MagicStonLevelChanceDatas;
}

[System.Serializable]
public class SubGradeChanceData
{
    public SubGrade Grade;
    public float Chance;
}
