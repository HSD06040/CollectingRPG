using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StageDatas
{
    private static readonly Dictionary<int, StageData> _stageDic = new Dictionary<int, StageData>();    

    public void Init()
    {
        foreach (var stage in Resources.LoadAll<StageData>("Data/Stage"))
        {
            if (!_stageDic.ContainsKey(stage.RegionNumber))
            {
                _stageDic.Add(stage.RegionNumber, stage);
            }
        }
    }

    public RewardData[] GetStageReward(int region, int stage)
    {
        return GetStage(region).GetStageReward(stage);
    }

    public StageData GetStage(int region)
    {
        if(!_stageDic.ContainsKey(region))
        {
            Init();
        }

        return _stageDic[region];
    }
}
