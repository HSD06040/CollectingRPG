using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StageData", menuName = "Data/StageData")]
public class StageData : ScriptableObject
{
    private readonly Dictionary<int, bool> _clearProofDic = new Dictionary<int, bool>();

    public string StageName;
    [TextArea] public string StageDescription;
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
            if (!_rewardDic.ContainsKey(reward.StageNumber))
            {
                _rewardDic.Add(reward.StageNumber, reward.RewardDatas);
            }
        }
    }

    public void SetClearProof(int stageNumber, bool isCleared)
    {
        if (_clearProofDic.ContainsKey(stageNumber))
        {
            _clearProofDic[stageNumber] = isCleared;
        }
        else
        {
            _clearProofDic.Add(stageNumber, isCleared);
        }
        Debug.Log($"지역 {RegionNumber}, 스테이지 {stageNumber} 클리어 증명 로컬 세팅 완료: {isCleared}");
    }

    public bool HasClearProof(int stageNumber)
    {
        return _clearProofDic.GetValueOrDefault(stageNumber, false);
    }
}

[Serializable]
public class StageClearData
{
    public bool isCleared = false;
}
