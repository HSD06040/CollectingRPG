using TMPro;
using UnityEditor.U2D.Aseprite;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class StagePanel : MonoBehaviour
{
    [Header("Instantiate")]
    [SerializeField] GameObject _stageButtonPrefab;
    [SerializeField] RectTransform _content;    
    [Space]

    [SerializeField] StageSelectButton[] _stageButtons;
    [SerializeField] Image _stageImage;
    [SerializeField] TMP_Text _stageName;
    [SerializeField] TMP_Text _stageDescription;
    [SerializeField] Button _rewardCheckButton;
    private StageData _stageData;
    private StageRewardPanel _stageRewardPanel;

    private void OnEnable()
    {
        StageOpenCheck();
        StageClearCheck();
    }

    public void Init(StageData stageData, StageRewardPanel stageRewardPanel)
    {
        _stageData = stageData;
        _stageRewardPanel = stageRewardPanel;

        //_applyButton.onClick.AddListener(); !확인 버튼 클릭 시 추가필요!
        _stageButtons = new StageSelectButton[4];

        for (int i = 0; i < 4; i++)
        {
            int index = i + 1;
            int region = stageData.RegionNumber;
            int stage = i + 1;

            StageSelectButton button = Instantiate(_stageButtonPrefab, _content).GetComponent<StageSelectButton>();
            _stageButtons[i] = button;

            button.Init(() => SetStage(region, index, button), region, index);
        }

        StageOpenCheck();
        StageClearCheck();

        _stageImage.sprite = stageData.RegionImage;
        _stageName.text = $"{stageData.RegionNumber}. {stageData.StageName}";
        _stageDescription.text = stageData.StageDescription;
    }

    public void StageRewardPanelOpen()
    {
        _stageRewardPanel.Setup(_stageData);
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
            _stageButtons[i].CheckFirstRewardGain(_stageData);
        }
    }

    private void SetStage(int region, int stage, StageSelectButton stageSelectButton)
    {
        if(StagePanelController.StageSelectButton != null)
            StagePanelController.StageSelectButton.DeSelect();

        Manager.Data.StageGameData.SetStage(region, stage);
        
        StageSelectionEvents.SelectStage(region, stage);

        StagePanelController.StageSelectButton = stageSelectButton;
    }
    
}
