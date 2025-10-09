using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MagicStoneLevelUpData
{
    public List<PieceLevelRatio> LevelRatio = new List<PieceLevelRatio>();

    public int GetCumulativePiece(int level)
    {
        int cumulativePiece = 0;

        int index = level - 1;
        if (index < 0)
        {
            index = 0;
            cumulativePiece += 10;
        }

        for (int i = index; i < LevelRatio.Count; i++)
        {
            cumulativePiece += LevelRatio[i].RequirePiece;
        }
        Debug.Log(cumulativePiece);

        return cumulativePiece;
    }
}