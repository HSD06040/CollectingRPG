using UnityEngine;
using UnityEngine.UI;
using Firebase.Auth;
using Firebase.Extensions;
using Random = UnityEngine.Random;

public class GuestSignIn : MonoBehaviour
{
    [SerializeField] private Button _guestLoginButton;

    // TODO: [CYH] 패널 전환 테스트_1 (삭제 예정)
    [SerializeField] private GameObject tutorialPanel;
    [SerializeField] private GameObject SigninPanel;

    private bool _isClicked;


    private void Start()
    {
        _guestLoginButton.onClick.AddListener(() =>
        {
            if (!_isClicked)
            {
                OnClick_GuestLogin();
            }
        });
    }

    /// <summary>
    /// Firebase 익명 로그인 후 닉네임 설정
    /// </summary>
    private void OnClick_GuestLogin()
    {
        _isClicked = true;

        // 게스트 로그인 가능 여부 체크
        if (FirebaseManager.Auth.CurrentUser != null)
        {
            Debug.LogError($"유저 UID : {FirebaseManager.Auth.CurrentUser.UserId}  " +
                $"/ 유저 닉네임 : {FirebaseManager.Auth.CurrentUser.DisplayName}");
            _isClicked = false;
            return;
        }

        FirebaseManager.Auth.SignInAnonymouslyAsync().ContinueWithOnMainThread(async task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("게스트 로그인 취소");
                _isClicked = false;
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError($"게스트 로그인 실패 / 원인: {task.Exception}");
                _isClicked = false;
                return;
            }

            Firebase.Auth.AuthResult result = task.Result;

            FirebaseUser currentUser = FirebaseManager.Auth.CurrentUser;

            Debug.Log("게스트 생성 완료");

            await currentUser.ReloadAsync();

            // 게스트 닉네임 변경 
            await AuthManager.Instance.SetGuestNicknameAsync(currentUser);
            await currentUser.ReloadAsync();

            Debug.Log("------유저 정보(GuestLogin)------");
            Debug.Log($"유저 닉네임 : {currentUser.DisplayName}");
            Debug.Log($"유저 ID : {currentUser.UserId}");
            Debug.Log($"이메일 : {currentUser.Email}");

            // SignInPanel -> tutorial패널 로 변경
            if (currentUser != null)
            {
                // TODO: [CYH] 패널 전환 테스트_2 (삭제 예정)
                Debug.Log("게스트 정보 업데이트 완료. tutorial패널 활성화");
                tutorialPanel.SetActive(true);
                SigninPanel.SetActive(false);
;
                _isClicked = false;
            }
        });
    }
}
