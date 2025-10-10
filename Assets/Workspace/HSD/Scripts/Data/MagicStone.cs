using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MagicStone", menuName = "MagicStone")]
public class MagicStone : MetaData
{
    public MagicStoneData[] MagicStoneDatas;
    public MagicStoneUpgradeData UpgradeData;
    public MagicStoneType Type;

    public void Init()
    {
        foreach (var data in MagicStoneDatas)
        {
            data.Icon = Icon;
            data.Name = Name;
            data.Type = Type;
        }
    }

    public MagicStoneData GetMagicStone()
    {
        return GetMagicStone(Manager.Data.MagicStoneLevelChanceData.GetGrade(UpgradeData.CurrentUpgradeData.UpgradeLevel));
    }

    public MagicStoneData GetMagicStone(SubGrade subGrade)
    {
        return Array.Find(MagicStoneDatas, m => m.Grade == subGrade);
    }
}
