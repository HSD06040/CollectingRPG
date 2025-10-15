using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PartySelectPanelController : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private GameObject _stageSelectPanel;
    [SerializeField] private GameObject _partySelectPanel;
    [SerializeField] private BottomPanelController _bottomPanelCtrl;

    [Header("Button")]
    [SerializeField] private Button _gameStartButton;
    [SerializeField] private Button _backButton;

    public Action OnSelectedIndexChanged;

    private void OnEnable()
    {
        _gameStartButton.onClick.AddListener(GameStart);
        _backButton.onClick.AddListener(CloseUI);
        OnSelectedIndexChanged += ActivateGameStartButton;
        ActivateGameStartButton();
    }

    private void OnDisable()
    {
        OnSelectedIndexChanged -= ActivateGameStartButton;
    }

    public void SetSelectedPresetIndex(int presetIndex)
    {
        if (Manager.Data != null)
        {
            Manager.Data.PresetDB.SelectPresetIndex(presetIndex);
            OnSelectedIndexChanged?.Invoke();
        }
    }

    private void ActivateGameStartButton()
    { 
        if(Manager.Data != null && Manager.Data.PresetDB.SelectedPresetIndex != -1)
        {
            _gameStartButton.interactable = true;
        }
        else
        {
            _gameStartButton.interactable = false;
        }
    }

    private async void GameStart()
    {
        Manager.Data.StageGameData.GetStage(out int region, out int stage);

        if (region == 0 || stage == 0)
        {
            UIManager.Instance.MessagePopup.Show("스테이지를 선택해주세요.");
            return;
        }

        // 씬 전환
        await SceneChangeManager.Instance.LoadSceneAsync("GameScene", GameSceneInit);

        Debug.Log("게임 시작");
    }

    private async UniTask GameSceneInit()
    {
        await Manager.Resources.LoadLabel("Stage");

        await Manager.Data.StageGridData.SetGridData();
    }

    public void ArrangePreset()
    {
        _bottomPanelCtrl.SelectButton(3);
        _stageSelectPanel.SetActive(false);
        _partySelectPanel.SetActive(false);
    }

    private void CloseUI()
    {
        gameObject.SetActive(false);
    }
}
