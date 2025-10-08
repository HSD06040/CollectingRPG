using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
