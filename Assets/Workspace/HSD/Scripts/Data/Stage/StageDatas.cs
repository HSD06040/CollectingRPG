using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StageDatas
{
    private static readonly Dictionary<int, StageData> _stageDic = new Dictionary<int, StageData>();    
    public int MaxStageCount => _stageDic.Count;

    public async UniTask Init()
    {
        foreach (var stage in await Manager.Resources.LoadAll<StageData>("StageData"))
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
        return _stageDic[region];
    }

    public bool CheckOpened(int region, int stage)
    {
        if (region == 1 && stage == 1)
            return true;

        StageData currentRegion = GetStage(region);

        if (stage > 1)
        {
            return currentRegion.HasClearProof(stage - 1);
        }
        else // stage == 1 이면 이전 지역 마지막 스테이지 확인
        {
            int prevRegion = region - 1;
            if (!_stageDic.ContainsKey(prevRegion))
                return false;

            StageData prevRegionData = GetStage(prevRegion);
            return prevRegionData.HasClearProof(prevRegionData.MaxStageNumber);
        }
    }
}
