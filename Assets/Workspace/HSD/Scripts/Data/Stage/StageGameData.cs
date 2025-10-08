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
    public StageFloorRewardData[] FloorRewardDatas = new StageFloorRewardData[9];
    private static readonly Dictionary<int, StageFloorRewardType[]> _floorRewardDic = new Dictionary<int, StageFloorRewardType[]>();

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

    public StageFloorRewardType[] GetFloorReward(int floor)
    {
        if (!_floorRewardDic.ContainsKey(floor))
            Init();

        return _floorRewardDic.ContainsKey(floor) ? _floorRewardDic[floor] : null;
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
public class StageFloorRewardData
{
    public int Floor;
    public StageFloorRewardType[] StageFloorRewardTypes;

    public StageFloorRewardData()
    {
        StageFloorRewardTypes = new StageFloorRewardType[2]
        {
            new StageFloorRewardType(),
            new StageFloorRewardType()
        };
    }
}

[Serializable]
public class StageFloorRewardType
{
    public InGameRewardType RewardType;
    public int Amount;
}
