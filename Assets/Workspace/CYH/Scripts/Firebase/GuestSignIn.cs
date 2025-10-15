using Cysharp.Threading.Tasks;
using Firebase.Auth;
using Firebase.Extensions;
using UnityEngine;
using UnityEngine.UI;

public class GuestSignIn : MonoBehaviour
{
    [SerializeField] private Button _guestLoginButton;

    private bool _isClicked;

    private void Start()
    {
        _guestLoginButton.onClick.AddListener(() =>
        {
            if (!AddressablesDownloader.IsDownloaded)
                return;

            if (!_isClicked)
            {
                if (FirebaseManager.Auth.CurrentUser != null)
                {
                    _isClicked = false;
                    CheckTutorialCompletedAsync();
                }
                else
                {
                    OnClick_GuestLogin();
                }
            }
        });
    }

    private void OnClick_GuestLogin()
    {
        _isClicked = true;

        FirebaseManager.Auth.SignInAnonymouslyAsync().ContinueWithOnMainThread(async task =>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                Debug.LogError($"게스트 로그인 실패 / 원인: {task.Exception}");
                _isClicked = false;
                return;
            }

            Firebase.Auth.AuthResult result = task.Result;
            FirebaseUser currentUser = FirebaseManager.Auth.CurrentUser;

            await currentUser.ReloadAsync();

            // 게스트 닉네임 변경 
            await Manager.Auth.SetGuestNicknameAsync(currentUser);
            await currentUser.ReloadAsync();

            // 튜토리얼 isTutorialComplete = false Data 생성
            SetTutorialInCompleteAsync();
            // 유저 재화 생성
            await Manager.DB.SaveCurrencyAsync(30, 50);
            // 유저 레벨, 경험치 생성
            await Manager.DB.SaveUserLevelAsync(1);
            await Manager.DB.SaveUserExpAsync(0);

            if (currentUser != null)
            {
                // 튜토리얼 isTutorialComplete = true Data 변경
                SetTutorialCompleteAsync();
                _isClicked = false;
                await SceneChangeManager.Instance.LoadSceneAsync("LobbyScene", InitAndLoad);
            }
        });
    }

    private async void CheckTutorialCompletedAsync()
    {
        bool isTutorialCompleted = await Manager.DB.CheckTutorialCompletedAsync();
        if (isTutorialCompleted)
        {
            await SceneChangeManager.Instance.LoadSceneAsync("LobbyScene", InitAndLoad);
        }
        else
        {
            await SceneChangeManager.Instance.LoadSceneAsync("LobbyScene", InitAndLoad);
        }
    }

    private async void SetTutorialInCompleteAsync()
    {
        await Manager.DB.SetTutorialInCompleteAsync();
    }

    private async void SetTutorialCompleteAsync()
    {
        await Manager.DB.SetTutorialCompleteAsync();
    }

    private async UniTask InitAndLoad()
    {
        Manager.Resources.LoadLabel("UnitPrefab").Forget();
        await Manager.Data.InitAsync();
        await Manager.Resources.LoadLabel("Stage");
        await Manager.DB.stageDB.LoadAllStageClearDatas();

        Manager.DB.EventHandler();
    }
}