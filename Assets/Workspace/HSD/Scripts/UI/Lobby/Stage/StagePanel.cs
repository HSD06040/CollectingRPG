using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StagePanel : MonoBehaviour
{
    [SerializeField] StageSelectButton[] _stageButtons;
    [SerializeField] Sprite _openChestImage;
    [SerializeField] Image _stageImage;
    [SerializeField] TMP_Text _stageName;
    [SerializeField] TMP_Text _stageDescription;
    private StageData _stageData;

    private void OnEnable()
    {
        StageOpenCheck();
    }

    public void Init(StageData stageData)
    {
        _stageData = stageData;

        for (int i = 0; i < _stageButtons.Length; i++)
        {
            int region = stageData.RegionNumber;
            int stage = i + 1;

            _stageButtons[i].Init(() => SetStage(stageData.RegionNumber, i + 1), stageData.RegionNumber, i + 1);
        }

        StageOpenCheck();

        _stageImage.sprite = stageData.RegionImage;
        _stageName.text = $"{stageData.RegionNumber}. {stageData.StageName}";
        _stageDescription.text = stageData.StageDescription;
    }

    private void StageOpenCheck()
    {
        if (_stageData == null)
            return;

        for (int i = 0; i < _stageButtons.Length; i++)
        {
            int region = _stageData.RegionNumber;
            int stage = i + 1;

            _stageButtons[i].CheckStageOpen(Manager.Data.StageDatas.CheckOpened(region, stage));
        }
    }

    private void SetStage(int region, int stage)
    {
        Manager.Data.StageGameData.SetStage(region, stage);
    }
}
