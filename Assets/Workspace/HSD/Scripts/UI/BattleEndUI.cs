using Cysharp.Threading.Tasks;
using Map;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BattleEndUI : MonoBehaviour
{
    [SerializeField] TMP_Text _stateText;
    [SerializeField] Button _applyButton;
    [SerializeField] TMP_Text _applyButtonText;
    [SerializeField] CanvasGroup _canvasGroup;

    private void Awake()
    {
        _canvasGroup.alpha = 0f;
        _canvasGroup.blocksRaycasts = false;
        _canvasGroup.interactable = false;
    }

    private void OnEnable()
    {
        BattleManager.OnPlayerDefeat += ShowDefeatPopup;
        BattleManager.OnBattleEnded += ShowWinPopup;
        BattleManager.OnGameEnded += ShowGameEndPopup;
    }

    private void OnDisable()
    {
        BattleManager.OnPlayerDefeat -= ShowDefeatPopup;
        BattleManager.OnBattleEnded -= ShowWinPopup;
        BattleManager.OnGameEnded -= ShowGameEndPopup;
    }

    private void ShowWinPopup()
    {
        _stateText.text = "전투 승리!";
        _applyButton.onClick.RemoveAllListeners();
        ShowAnimation().Forget();

        _applyButtonText.text = "맵으로";
        _applyButton.onClick.AddListener(MapPlayerTracker.Instance.unitManager.MapUIController.MapEnter);
        _applyButton.onClick.AddListener(ClosePopup);
        
        Manager.Game.Reward_Controller.GetCurrentFloorReward();
    }

    private void ShowGameEndPopup()
    {
        _stateText.text = "스테이지 클리어!";
        _applyButton.onClick.RemoveAllListeners();
        ShowAnimation().Forget();

        _applyButtonText.text = "로비로 돌아가기";
        _applyButton.onClick.AddListener(LoadLobby);
    }

    private void ShowDefeatPopup()
    {
        _stateText.text = "전투 패배...";

        _applyButton.onClick.RemoveAllListeners();
        ShowAnimation().Forget();

        _applyButtonText.text = "로비로 돌아가기";
        _applyButton.onClick.AddListener(LoadLobby);
    }

    private async UniTask ShowAnimation()
    {
        await _canvasGroup.FadeIn(1);
    }

    private void ClosePopup()
    {
        HideAnimation().Forget();
    }

    private async UniTask HideAnimation()
    {
        await _canvasGroup.FadeOut(1);
    }

    private void LoadLobby()
    {
        WaitForClose().Forget();
    }

    private async UniTask WaitForClose()
    {
        await SceneChangeManager.Instance.LoadSceneAsync("LobbyScene", TimeScaleClear);
        await Manager.Game.StageClearAsync(Manager.Data.StageGameData.GetCurrentStage());
        await Manager.DB.SetTutorialCompleteAsync();
    }

    private async UniTask TimeScaleClear()
    {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;

        await UniTask.Delay(100);
    }
}
