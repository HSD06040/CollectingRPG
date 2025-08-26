using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Firebase.Auth;
using Firebase.Extensions;
using Google;

public class AccountLinkPopup : MonoBehaviour
{
    [SerializeField] private PlayerDataController _playerDataController;
    
    [SerializeField] private Button _closeButton;
    [SerializeField] private Button _googleLinkButton;
    [SerializeField] private Button _deleteButton;
    [SerializeField] private Button _signOutButton;


    private void Start()
    {
        _closeButton.onClick.AddListener(() => gameObject.SetActive(false));
        _googleLinkButton.onClick.AddListener(() => OnClick_LinkWithGoogle());

        _signOutButton.onClick.AddListener(() =>
        {
            Manager.Auth.UserSignOut();
            GoogleSignIn.DefaultInstance.SignOut();
            GoogleSignIn.DefaultInstance.Disconnect();
            SceneManager.LoadScene("CYH_Sign-in");
        });

        _deleteButton.onClick.AddListener(() =>
        {
            Manager.DB.DeleteUserUid();
            Manager.Auth.DeleteUser();
            SceneManager.LoadScene("CYH_Sign-in");
        });
    }

    /// <summary>
    /// 게스트 계정 -> 구글 계정으로 전환하는 메서드
    /// </summary>
    public void OnClick_LinkWithGoogle()
    {
        // 계정 전환 가능 여부 체크
        FirebaseUser currentUser = FirebaseManager.Auth.CurrentUser;

        if (currentUser == null || !currentUser.IsAnonymous)
        {
            Debug.LogError("게스트 x / 계정 전환 불가");
            return;
        }

        GoogleSignIn.DefaultInstance.SignIn().ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                Debug.LogError($"구글 로그인 실패 / 원인: {task.Exception}");
                return;
            }

            GoogleSignInUser googleUser = task.Result;
            string idToken = googleUser.IdToken;

            Credential credential = GoogleAuthProvider.GetCredential(idToken, null);

            FirebaseManager.Auth.CurrentUser.LinkWithCredentialAsync(credential).ContinueWithOnMainThread(async linkTask =>
            {
                if (linkTask.IsCanceled)
                {
                    Debug.LogError("구글 계정 전환 취소");
                    return;
                }

                if (linkTask.IsFaulted)
                {
                    Debug.LogError($"구글 계정 전환 실패 / 원인: {linkTask.Exception}");
                    Debug.LogWarning("이미 생성된 구글 계정 / 해당 계정으로 로그인 시도");

                    // 1. 게스트 계정 삭제
                    await Manager.DB.DeleteUserUid();
                    await Manager.Auth.DeleteUser_sync();

                    // 2. 해당 credential로 로그인
                    Task signInTask = FirebaseManager.Auth.SignInWithCredentialAsync(credential);
                    await signInTask;

                    if (signInTask.IsCanceled || signInTask.IsFaulted)
                    {
                        Debug.LogError($"기존 구글 계정으로 로그인 실패: {signInTask.Exception}");

                        GoogleSignIn.DefaultInstance.SignOut();
                        GoogleSignIn.DefaultInstance.Disconnect();
                        return;
                    }

                    Debug.Log($"기존 구글 계정 로그인 성공 / currentUser UID: {currentUser.UserId}");

                    if (currentUser == null)
                    {
                        Debug.Log("currentUser == null");
                    }
                    else
                    {
                        Debug.Log("currentUser != null");
                    }

                    PlayerData data = await Manager.DB.LoadLobbyDataAsync();
                    _playerDataController.OnUpdateUI?.Invoke(data);
                    Debug.Log("UI 업데이트 이벤트 호출 : _playerDataController.UpdateUI(data)");
                    return;
                }

                Firebase.Auth.AuthResult linkedUser = linkTask.Result;
                string googleDisplayName = googleUser.DisplayName;

                // DB에 google 계정 닉네임 저장
                await Manager.DB.SaveNicknameAsync(googleDisplayName);
                await currentUser.ReloadAsync();

                Debug.Log("------유저 정보(GoogleLink)------");
                await Manager.DB.LoadNicknameAsync((nickname) =>
                {
                    Debug.Log($"유저 ID : {nickname}");
                });
                Debug.Log($"유저 ID: {currentUser.UserId}");
                Debug.Log($"이메일 : {currentUser.Email}");
            });
        });
    }
}
