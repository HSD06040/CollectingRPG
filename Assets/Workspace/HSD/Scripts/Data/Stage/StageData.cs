using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StageData", menuName = "Data/StageData")]
public class StageData : ScriptableObject
{
    private readonly Dictionary<int, bool> _clearProofDic = new Dictionary<int, bool>();
    private readonly Dictionary<int, bool> _firstRewardGainDic = new Dictionary<int, bool>();

    public string StageName;
    [TextArea] public string StageDescription;
    public StageRewardData[] StageFirstRewardDatas = new StageRewardData[4];
    public StageRewardData[] StageRewardDatas = new StageRewardData[4];
    [Range(1, 7)] public int RegionNumber;
    public int MaxStageNumber;
    public Sprite RegionImage;
    public Sprite RegionPreviewSprite;
    public GameObject Map;

    private static readonly Dictionary<int, OutGameRewardData[]> _firstRewardDic = new Dictionary<int, OutGameRewardData[]>();
    private static readonly Dictionary<int, OutGameRewardData[]> _rewardDic = new Dictionary<int, OutGameRewardData[]>();

    public OutGameRewardData[] GetStageFirstReward(int stageNumber)
    {
        if (_firstRewardGainDic.TryGetValue(stageNumber, out bool stageRewardGain))
        {
            if (stageRewardGain)
            {
                UIManager.Instance.MessagePopup.Show($"이미 {RegionNumber}-{stageNumber}의 최초 클리어 보상을 획득했습니다.");                
                return null;
            }
        }        

        if (!_firstRewardDic.ContainsKey(stageNumber))
        {
            InitFistReward();
        }

        SetClearProof(stageNumber, true);
        return _firstRewardDic[stageNumber];
    }

    public OutGameRewardData[] GetStageReward(int stageNumber)
    {
        if (!_rewardDic.ContainsKey(stageNumber))
        {
            InitReward();
        }
        return _rewardDic[stageNumber];
    }

    private void InitFistReward()
    {
        foreach (var reward in StageFirstRewardDatas)
        {
            if (!_firstRewardDic.ContainsKey(reward.StageNumber))
            {
                _firstRewardDic.Add(reward.StageNumber, reward.RewardDatas);
            }
        }
    }

    private void InitReward()
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

    public void SetFirstRewardGainProof(int stageNumber, bool isGain)
    {
        if(_firstRewardDic.ContainsKey(stageNumber))
        {
            _firstRewardGainDic[stageNumber] = isGain;
        }
        else
        {
            _firstRewardGainDic.Add(stageNumber, isGain);
        }
        Debug.Log($"지역 {RegionNumber}, 스테이지 {stageNumber} 최초 클리어 보상 획득 로컬 세팅 완료: {isGain}");
    }

    public bool HasClearProof(int stageNumber)
    {
        return _clearProofDic.GetValueOrDefault(stageNumber, false);
    }
    public bool HasFirstRewardGainProof(int stageNumber)
    {
        return _firstRewardGainDic.GetValueOrDefault(stageNumber, false);
    }
    public Sprite GetStagePreviewSprite(int stageNumber)
    {
        return RegionPreviewSprite;
    }
}