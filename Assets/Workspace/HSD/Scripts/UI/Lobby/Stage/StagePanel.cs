using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class StagePanel : MonoBehaviour
{
    [SerializeField] StageSelectButton[] _stageButtons;
    [SerializeField] Image _stageImage;
    [SerializeField] TMP_Text _stageName;
    [SerializeField] TMP_Text _stageDescription;
    private StageData _stageData;

    private void OnEnable()
    {
        StageOpenCheck();
        StageClearCheck();
    }

    public void Init(StageData stageData)
    {
        _stageData = stageData;

        //_applyButton.onClick.AddListener(); !확인 버튼 클릭 시 추가필요!

        for (int i = 0; i < _stageButtons.Length; i++)
        {
            int region = stageData.RegionNumber;
            int stage = i + 1;

            _stageButtons[i].Init(() => SetStage(stageData.RegionNumber, i + 1), stageData.RegionNumber, i + 1);            
        }

        StageClearCheck();
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

    private void StageClearCheck()
    {
        if (_stageData == null)
            return;

        for (int i = 0; i < _stageButtons.Length; i++)
        {
            _stageButtons[i].CheckClear(_stageData);
        }
    }

    private void SetStage(int region, int stage)
    {
        Manager.Data.StageGameData.SetStage(region, stage);
    }
}
