using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InGameStagePanel : MonoBehaviour
{
    [SerializeField] TMP_Text _stageNumberText;
    [SerializeField] TMP_Text _stageNameText;

    private void OnEnable()
    {
        Manager.Data.StageGameData.GetStage(out int region, out int stage);
        _stageNumberText.text = $"STAGE {region}-{stage}";
        _stageNameText.text = $"{Manager.Data.StageGameData.GetCurrentStage().StageName}";
    }
}
