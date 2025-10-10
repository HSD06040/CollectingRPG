using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StageRewardSlotController : MonoBehaviour
{
    [SerializeField] Reward_Slot_UI[] _reward_Slot_UIs;
    [SerializeField] TMP_Text _stageNumber;

    public void SetStageReward(int region, int stage, OutGameRewardData[] rewardDatas)
    {
        _stageNumber.text = $"{region}-{stage}";

        int count = 0;

        for (int i = 0; i < rewardDatas.Length; i++)
        {
            count++;
            _reward_Slot_UIs[i].Setup(rewardDatas[i].GetRewardSprite(), rewardDatas[i].Amount);
        }

        for (int i = count; i < _reward_Slot_UIs.Length; i++)
        {
            _reward_Slot_UIs[i].Close();
        }
    }
}
