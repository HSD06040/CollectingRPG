using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StageRewardData
{
    public int StageNumber;
    public OutGameRewardData[] RewardDatas;
}

[Serializable]
public struct OutGameRewardData
{
    public OutGameRewardType RewardType;
    public int Amount;
}
