using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StageData", menuName = "Data/StageData")]
public class StageData : ScriptableObject
{
    public StageRewardData[] StageRewardDatas;
    [Range(1, 7)] public int RegionNumber;
    public int MaxStageNumber;
    public Sprite RegionImage;
    public GameObject Map;

    private static readonly Dictionary<int, RewardData[]> _rewardDic = new Dictionary<int, RewardData[]>();

    public RewardData[] GetStageReward(int stageNumber)
    {
        if (!_rewardDic.ContainsKey(stageNumber))
        {
            Init();
        }
        return _rewardDic[stageNumber];
    }

    private void Init()
    {
        foreach (var reward in StageRewardDatas)
        {
            if(!_rewardDic.ContainsKey(reward.StageNumber))
            {
                _rewardDic.Add(reward.StageNumber, reward.RewardDatas);
            }
        }
    }
}
