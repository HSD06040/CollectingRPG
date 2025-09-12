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
    [SerializeField] private Button _arrangeButton;

    public Action OnSelectedIndexChanged;

    private void OnEnable()
    {
        _gameStartButton.onClick.AddListener(GameStart);
        _arrangeButton.onClick.AddListener(ArrangePreset);
        OnSelectedIndexChanged += ActivateGameStartButton;
        ActivateGameStartButton();
    }

    private void OnDisable()
    {
        OnSelectedIndexChanged -= ActivateGameStartButton;
    }

    public void SetSelectedPresetIndex(int presetIndex)
    {
        if (TempDataManager.Instance != null)
        {
            TempDataManager.Instance.SelectPresetIndex(presetIndex);
            OnSelectedIndexChanged?.Invoke();
        }
    }

    private void ActivateGameStartButton()
    { 
        if(TempDataManager.Instance != null && TempDataManager.Instance.SelectedPresetIndex != -1)
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
        // 씬 전환
        SceneManager.LoadScene("USW_GameScene");

        await Manager.Resources.LoadLabel("Stage");

        Debug.Log("게임 시작");
    }

    private void ArrangePreset()
    {
        _bottomPanelCtrl.SelectButton(3);
        _stageSelectPanel.SetActive(false);
        _partySelectPanel.SetActive(false);
    }
}
