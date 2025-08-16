using Firebase.Auth;
using Firebase.Extensions;
using Google;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using System.Collections.Generic;

public class GoogleLink : MonoBehaviour
{
    [SerializeField] private Button _googleButton;

    private void Start()
    {
        _googleButton.onClick.AddListener(OnClick_LinkWithGoogle);
    }

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
                Debug.Log($"구글 계정 닉네임 : {googleDisplayName}");

                // 구글 닉네임 변경 
                await SetGoogleNicknameAsunc(currentUser, googleDisplayName);
                await currentUser.ReloadAsync();

                //GameStartPanel 닉네임 text 변경 이벤트 호출
                //_gameStartPanel.OnSetNicknameField?.Invoke(user.DisplayName);

                Debug.Log("------유저 정보------");
                Debug.Log($"유저 이름 : {currentUser.DisplayName}");
                Debug.Log($"유저 ID: {currentUser.UserId}");
                Debug.Log($"이메일 : {currentUser.Email}");

                Debug.LogError("구글 계정 전환 성공 / 재로그인");

                // 강제 로그아웃
                FirebaseManager.Auth.SignOut();

                // 구글 계정 로그아웃 처리 및 계정과 앱 연결 해제
                GoogleSignIn.DefaultInstance.SignOut();
                GoogleSignIn.DefaultInstance.Disconnect();
            });
        });
    }

    private static async Task SetGoogleNicknameAsunc(FirebaseUser currentUser, string googleDisplayName)
    {
        UserProfile profile = new UserProfile();
        profile.DisplayName = googleDisplayName;
        Debug.Log($"SetGoogleNickname : googleDisplayName = {googleDisplayName}");

        await currentUser.UpdateUserProfileAsync(profile);

        // 초기화
        await currentUser.ReloadAsync();

        // Firebase DB에 닉네임 저장
        await SaveNicknameAsync();
        await currentUser.ReloadAsync();

        Debug.Log("닉네임 설정 성공");
        Debug.Log($"변경된 유저 닉네임 : {currentUser.DisplayName}");
    }

    private static async Task<bool> SaveNicknameAsync()
    {
        FirebaseUser currentUser = FirebaseManager.Auth.CurrentUser;

        string uid = currentUser.UserId;
        string userNickname = FirebaseManager.Auth.CurrentUser.DisplayName;

        Dictionary<string, object> dictionary = new Dictionary<string, object>();

        dictionary[$"UserData/{uid}/Nickname"] = userNickname;

        var task = FirebaseManager.DataReference.UpdateChildrenAsync(dictionary);
        await task;

        if (task.IsCompletedSuccessfully)
        {
            Debug.Log("UserData에 닉네임 저장 성공");
            return true;
        }
        else
        {
            Debug.LogError("닉네임 저장 실패");
            return false;
        }
    }
}

