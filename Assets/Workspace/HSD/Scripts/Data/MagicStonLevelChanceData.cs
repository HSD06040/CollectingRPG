using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MagicStonLevelChanceData", menuName = "Data/MagicStone/MagicStonLevelChanceData")]
public class MagicStonLevelChanceData : ScriptableObject
{
    public MagicStonLevelChances[] MagicStonLevelsChanceDatas;
    private static readonly Dictionary<int, MagicStonLevelChances> _magicStonLevelChanceDic = new Dictionary<int, MagicStonLevelChances>();
    public int[] NeedPirces;

    public void Init()
    {
        for (int i = 0; i < MagicStonLevelsChanceDatas.Length; i++)
        {
            int level = i + 1;
            if (!_magicStonLevelChanceDic.ContainsKey(level))
            {
                _magicStonLevelChanceDic.Add(level, MagicStonLevelsChanceDatas[i]);
            }
        }
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
            totalChance += chance.SuccessChance;
        }

        float randomValue = Random.Range(0f, totalChance);
        float cumulative = 0f;

        foreach (var chance in chances)
        {
            cumulative += chance.SuccessChance;
            if (randomValue <= cumulative)
            {
                return chance.Grade;
            }
        }

        return chances[chances.Length - 1].Grade;
    }
}

[System.Serializable]
public class MagicStonLevelChances
{
    public MagicStonLevelChance[] MagicStonLevelChanceDatas;
}

[System.Serializable]
public class MagicStonLevelChance
{
    public SubGrade Grade;
    public float SuccessChance;
}
