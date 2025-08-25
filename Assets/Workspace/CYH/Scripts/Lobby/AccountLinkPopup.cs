using Firebase.Auth;
using Firebase.Extensions;
using Google;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AccountLinkPopup : MonoBehaviour
{
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
                    Debug.LogError("구글 계정 전환 실패");

                    GoogleSignIn.DefaultInstance.SignOut();
                    GoogleSignIn.DefaultInstance.Disconnect();
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
