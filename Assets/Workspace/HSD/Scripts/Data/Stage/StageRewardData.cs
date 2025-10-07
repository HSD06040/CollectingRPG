using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StageRewardData
{
    public int StageNumber;
    public RewardData[] RewardDatas;
}

[Serializable]
public struct RewardData
{
    public RewardType RewardType;
    public int Amount;
}
