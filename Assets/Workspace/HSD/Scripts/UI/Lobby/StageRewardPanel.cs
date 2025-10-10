using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class StageRewardPanel : MonoBehaviour
{
    [SerializeField] Image _stageIcon;
    [SerializeField] TMP_Text _stageName;
    [SerializeField] StageRewardSlotController[] _stageRewardSlotControllers;

    public void Setup(StageData stageData)
    {
        gameObject.SetActive(true);

        _stageIcon.sprite = stageData.RewardImage;
        _stageName.text = stageData.StageName;

        for (int i = 0; i < stageData.MaxStageNumber; i++)
        {
            int region = stageData.RegionNumber;
            int stage = i + 1;

            OutGameRewardData[] rewardDatas = stageData.GetStageFirstReward(stage);
            _stageRewardSlotControllers[i].SetStageReward(region, stage, rewardDatas);
        }
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
}
