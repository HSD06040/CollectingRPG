using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PartySelectPanelController : MonoBehaviour
{
    [SerializeField] private Button _gameStartButton;

    public Action OnSelectedIndexChanged;

    private void OnEnable()
    {
        _gameStartButton.onClick.AddListener(GameStart);
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
}
