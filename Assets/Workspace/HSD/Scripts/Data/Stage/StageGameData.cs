using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StageGameData
{
    private int _currentRegion;
    private int _currentStage;
    public Property<int> CurrentFloor = new();
    public StageInGameRewardData[] FloorRewardDatas = new StageInGameRewardData[9];
    private static readonly Dictionary<int, StageInGameRewardType[]> _floorRewardDic = new Dictionary<int, StageInGameRewardType[]>();

    #region Stage
    public void SetStage(int region, int stage)
    {
        _currentRegion = region;
        _currentStage = stage;
    }
    public void GetStage(out int region, out int stage)
    {
        region = _currentRegion;
        stage = _currentStage;
    }
    #endregion

    public StageData GetCurrentStage()
    {
        return Manager.Data.StageDatas.GetStage(_currentStage);
    }

    public StageInGameRewardType[] GetFloorReward(int floor)
    {
        if (!_floorRewardDic.ContainsKey(floor))
            Init();

        return _floorRewardDic.ContainsKey(floor) ? _floorRewardDic[floor] : null;
    }

    public StageInGameRewardType[] GetCurrentFloorReward()
    {
        int idx = CurrentFloor.Value + 1;
        if (!_floorRewardDic.ContainsKey(idx))
            Init();

        return GetFloorReward(idx);
    }

    private void Init()
    {
        foreach (var reward in FloorRewardDatas)
        {
            if (!_floorRewardDic.ContainsKey(reward.Floor))
            {
                _floorRewardDic.Add(reward.Floor, reward.StageFloorRewardTypes);
            }
        }
    }
}

[Serializable]
public class StageInGameRewardData
{
    public int Floor;
    public StageInGameRewardType[] StageFloorRewardTypes;

    public StageInGameRewardData()
    {
        StageFloorRewardTypes = new StageInGameRewardType[2]
        {
            new StageInGameRewardType(),
            new StageInGameRewardType()
        };
    }
}

[Serializable]
public class StageInGameRewardType
{
    public InGameRewardType RewardType;
    public int Amount;
    public MagicStoneData MagicStoneData;
}
