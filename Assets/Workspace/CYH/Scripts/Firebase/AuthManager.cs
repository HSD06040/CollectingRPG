using System.Threading.Tasks;
using System.Linq;
using UnityEngine;
using Firebase.Auth;
using Firebase.Extensions;
using Google;

public class AuthManager : Singleton<AuthManager>
{
    /// <summary>
    /// 현재 로그인된 계정을 로그아웃 처리하는 메서드
    /// </summary>
    public void UserSignOut()
    {
        FirebaseManager.Auth.SignOut();
        Debug.Log("로그아웃");

        //TODO: [CYH] 게스트/구글 계정 예외 처리
        GoogleSignIn.DefaultInstance.SignOut();
        GoogleSignIn.DefaultInstance.Disconnect();
    }

    /// <summary>
    /// 현재 로그인된 계정을 FirebaseAuth에서 삭제하는 메서드
    /// </summary>
    public void DeleteUser()
    {
        FirebaseUser currentUser = FirebaseManager.Auth.CurrentUser;

        currentUser.DeleteAsync()
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsCanceled)
                {
                    Debug.LogError("유저 삭제 취소");
                }
                if (task.IsFaulted)
                {
                    Debug.LogError("유저 삭제 실패");
                }
;
                Debug.Log("유저 삭제 성공");

                // 현재 로그인된 유저가 있으면 로그아웃
                if (currentUser != null)
                {
                    // 구글 계정 로그아웃 처리 및 계정과 앱 연결 해제
                    bool isGoogleUser = currentUser.ProviderData.Any(provider => provider.ProviderId == "google.com");
                    if (isGoogleUser)
                    {
                        GoogleSignIn.DefaultInstance.SignOut();
                        GoogleSignIn.DefaultInstance.Disconnect();
                    }

                    FirebaseManager.Auth.SignOut();
                }

                FirebaseManager.Auth.SignOut();
                GoogleSignIn.DefaultInstance.SignOut();
                GoogleSignIn.DefaultInstance.Disconnect();
            });
    }
    
    /// <summary>
    /// 익명계정의 DisplayName을 "게스트 + 랜덤숫자"로 변경하는 메서드 
    /// </summary>
    /// <param name="currentUser">닉네임을 변경할 유저</param>
    public async Task SetGuestNicknameAsync(FirebaseUser currentUser)
    {
        UserProfile profile = new UserProfile();
        profile.DisplayName = $"Guest{Random.Range(1000, 10000)}";

        await currentUser.UpdateUserProfileAsync(profile);
        await currentUser.ReloadAsync();

        // Firebase DB에 닉네임 저장
        await Manager.DB.SaveNicknameAsync();
        await currentUser.ReloadAsync();

        Debug.Log($"변경된 유저 닉네임 / SetGuestNicknameAsync : {currentUser.DisplayName}");
    }

    /// <summary>
    /// 게스트에서 구글 계정으로 전환한 유저의 닉네임(displayname)을 재설정하고 Firebase DB에 저장하는 메서드
    /// </summary>
    /// <param name="currentUser">현재 로그인된 Firebase 유저</param>
    /// <param name="googleDisplayName">구글 계정 닉네임</param>
    public async Task SetGoogleNicknameAsync(FirebaseUser currentUser, string googleDisplayName)
    {
        string googleName = googleDisplayName;
        UserProfile profile = new UserProfile();
        profile.DisplayName = googleName;

        await currentUser.UpdateUserProfileAsync(profile);
        await currentUser.ReloadAsync();

        // Firebase DB에 닉네임 저장
        await Manager.DB.SaveNicknameAsync();
        await currentUser.ReloadAsync();

        Debug.Log($"변경된 유저 닉네임 / SetGoogleNicknameAsync : {currentUser.DisplayName}");
    }
}
