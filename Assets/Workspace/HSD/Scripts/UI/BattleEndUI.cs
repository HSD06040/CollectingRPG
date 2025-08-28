using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BattleEndUI : MonoBehaviour
{
    private void OnEnable()
    {
        BattleManager.OnBattleEnded += ShowPopup;
    }

    private void OnDisable()
    {
        BattleManager.OnBattleEnded -= ShowPopup;
    }

    private void ShowPopup()
    {
        PopupManager.Instance.ShowConfirmationPopup("전투가 종료되었습니다.\n로비로 돌아가시겠습니까?", async () =>
        {
            await WaitForClose();
        });
    }

    private async UniTask WaitForClose()
    {
        await Manager.DB.SetTutorialCompleteAsync();
        
        var tcs = new UniTaskCompletionSource();
        
        await SceneManager.LoadSceneAsync("USW_LobbyScene", LoadSceneMode.Single);        
    }
}
