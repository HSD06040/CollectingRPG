using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MagicStone", menuName = "MagicStone")]
public class MagicSton : ScriptableObject
{
    public MagicStoneData[] MagicStoneDatas;
    public int Level = 1;

    public MagicStoneData GetMagicStone()
    {
        return GetMagicStone(Manager.Data.MagicStonLevelChanceData.GetGrade(Level));
    }

    private MagicStoneData GetMagicStone(SubGrade subGrade)
    {
        return Array.Find(MagicStoneDatas, m => m.Grade == subGrade);
    }
}
