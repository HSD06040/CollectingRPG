using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Reward_Controller
{
    public void GetReward(StageData stageData, int stage)
    {
        GetStageRewardAsync(stageData, stage).Forget();
    }

    public void GetFirstReward(StageData stageData, int stage)
    {
        GetStageFirstRewardAsync(stageData, stage).Forget();
    }

    public void GetCurrentFloorReward(UnityAction action = null)
    {
        StageInGameRewardType[] source = Manager.Data.StageGameData.GetCurrentFloorReward();
        StageInGameRewardType[] stageInGameRewardTypes = new StageInGameRewardType[source.Length];
        Array.Copy(source, stageInGameRewardTypes, source.Length);

        GetInGameReward(stageInGameRewardTypes);
    }

    public void GetInGameReward(StageInGameRewardType[] stageInGameRewardTypes, UnityAction action = null)
    {
        foreach (var reward in stageInGameRewardTypes)
        {
            switch (reward.RewardType)
            {
                case InGameRewardType.Silver:
                    InGameManager.Instance.AddSilver(reward.Amount);
                    break;
                case InGameRewardType.Energy:
                    InGameManager.Instance.AddEnergy(reward.Amount);
                    break;
                case InGameRewardType.MagicStone:
                    MagicStoneData magicStoneData = Manager.Data.GetRandomMagicStoneData();
                    MagicStoneController.Instance.TrySetMagicStone(magicStoneData);
                    reward.MagicStoneData = magicStoneData;
                    break;
            }
        }

        UIManager.Instance.Reward_UI.Show(stageInGameRewardTypes, action);
    }

    private async UniTask GetStageFirstRewardAsync(StageData stageData, int stage)
    {
        OutGameRewardData[] firstRewardData = stageData.GetStageFirstReward(stage);

        if (firstRewardData != null)
        {
            foreach (var reward in firstRewardData)
            {
                switch (reward.RewardType)
                {
                    case OutGameRewardType.Diamond:
                        await Manager.DB.AddDiamondAsync(reward.Amount);
                        break;
                    case OutGameRewardType.Gold:
                        await Manager.DB.AddGoldAsync(reward.Amount);
                        break;
                    case OutGameRewardType.Exp:
                        break;
                }
            }
        }

        UIManager.Instance.Reward_UI.Show(firstRewardData);
    }

    private async UniTask GetStageRewardAsync(StageData stageData, int stage)
    {        
        OutGameRewardData[] rewardData = stageData.GetStageReward(stage);

        if (rewardData != null)
        {
            foreach (var reward in rewardData)
            {
                switch (reward.RewardType)
                {
                    case OutGameRewardType.Diamond:
                        await Manager.DB.AddDiamondAsync(reward.Amount);
                        break;
                    case OutGameRewardType.Gold:
                        await Manager.DB.AddGoldAsync(reward.Amount);
                        break;
                    case OutGameRewardType.Exp:
                        break;
                }
            }
            Debug.Log("일반 보상 획득");
        }

        UIManager.Instance.Reward_UI.Show(rewardData);
    }
}
