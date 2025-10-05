using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StagePanel : MonoBehaviour
{
    [SerializeField] StageSelectButton[] _stageButtons;
    [SerializeField] Sprite _openChestImage;    

    public void Init(StageData stageData)
    {        
        for (int i = 0; i < _stageButtons.Length; i++)
        {
            _stageButtons[i].Init(() => SetStage(stageData.RegionNumber, i + 1), stageData.RegionNumber, i + 1);
        }
    }

    private void SetStage(int region, int stage)
    {
        Manager.Data.StageGameData.SetStage(region, stage);
    }
}
