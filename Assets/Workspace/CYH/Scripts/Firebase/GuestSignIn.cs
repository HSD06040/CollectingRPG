using Firebase.Auth;
using Firebase.Extensions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GuestSignIn : MonoBehaviour
{
    [SerializeField] private Button _guestLoginButton;

    // TODO: [CYH] 패널 전환 테스트_1 (삭제 예정)
    //[SerializeField] private GameObject tutorialPanel;
    //[SerializeField] private GameObject SigninPanel;

    private bool _isClicked;


    private void Start()
    {
        _guestLoginButton.onClick.AddListener(() =>
        {
            if (!_isClicked)
            {
                if (FirebaseManager.Auth.CurrentUser != null)
                {
                    _isClicked = false;

                    // 튜토리얼 진행 여부 체크
                    CheckTutorialCompletedAsync();
                }
                else
                {
                    OnClick_GuestLogin();
                }
            }
        });
    }

    /// <summary>
    /// Firebase 익명 로그인 후 닉네임 설정
    /// </summary>
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

            // SignInPanel -> Tutorial패널 로 변경
            //if (currentUser != null)
            //{
            //    // TODO: [CYH] 패널 전환 테스트_2 (삭제 예정)
            //    //tutorialPanel.SetActive(true);
            //    //SigninPanel.SetActive(false);
            //
            //    // 튜토리얼 isTutorialComplete = true Data 변경
            //    SetTutorialCompleteAsync();
            //    _isClicked = false;
            //    SceneManager.LoadScene("USW_LoadingScene");
            //}
        });
    }

    private async void CheckTutorialCompletedAsync()
    {
        bool isTutorialCompleted = await Manager.DB.CheckTutorialCompletedAsync();
        if (isTutorialCompleted)
        {
            //SceneManager.LoadScene("USW_LobbyScene");
            SceneManager.LoadScene("USW_LobbyScene_Copy2");
        }
        else
        {
            //SceneManager.LoadScene("USW_LoadingScene");
            SceneManager.LoadScene("USW_LoadingScene_Copy2");
            // TODO: [CYH] 패널 전환 테스트_2 (삭제 예정)
            //tutorialPanel.SetActive(true);
            //SigninPanel.SetActive(false);
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
}
