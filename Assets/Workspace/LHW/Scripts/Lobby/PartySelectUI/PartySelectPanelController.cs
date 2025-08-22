using System;
using UnityEngine;
using UnityEngine.UI;

public class PartySelectPanelController : MonoBehaviour
{
    [SerializeField] private Button _gameStartButton;

    private int _currentSelectedPresetIndex = -1;
    public int CurrentSelectedPresetIndex => _currentSelectedPresetIndex;

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
        _currentSelectedPresetIndex = presetIndex;
        OnSelectedIndexChanged?.Invoke();
    }

    private void ActivateGameStartButton()
    { 
        if(_currentSelectedPresetIndex != -1)
        {
            _gameStartButton.interactable = true;
        }
        else
        {
            _gameStartButton.interactable = false;
        }
    }

    private void GameStart()
    {
        // 씬 전환
        Debug.Log("게임 시작");
    }
}
