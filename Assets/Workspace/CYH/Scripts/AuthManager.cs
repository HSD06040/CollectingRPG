using System.Threading.Tasks;
using UnityEngine;
using Firebase.Auth;

public class AuthManager : Singleton<AuthManager>
{
    /// <summary>
    /// 익명계정의 DisplayName을 "게스트 + 랜덤숫자"로 변경하는 메서드 
    /// </summary>
    /// <param name="currentUser">닉네임을 변경할 유저</param>
    public async Task SetGuestNicknameAsync(FirebaseUser currentUser)
    {
        UserProfile profile = new UserProfile();
        profile.DisplayName = $"게스트{Random.Range(1000, 10000)}";

        await currentUser.UpdateUserProfileAsync(profile);
        
        // 초기화
        await currentUser.ReloadAsync();

        // Firebase DB에 닉네임 저장
        await DBManager.Instance.SaveNicknameAsync();
        await currentUser.ReloadAsync();

        Debug.Log("닉네임 설정 성공");
        Debug.Log($"변경된 유저 닉네임 : {currentUser.DisplayName}");
    }

    /// <summary>
    /// 게스트에서 구글 계정으로 전환한 유저의 닉네임(displayname)을 재설정하고 Firebase DB에 저장하는 메서드
    /// </summary>
    /// <param name="currentUser">현재 로그인된 Firebase 유저</param>
    /// <param name="googleDisplayName">구글 계정 닉네임</param>
    /// <returns>비동기 작업 Task</returns>
    public async Task SetGoogleNicknameAsunc(FirebaseUser currentUser, string googleDisplayName)
    {
        UserProfile profile = new UserProfile();
        profile.DisplayName = googleDisplayName;
        Debug.Log($"SetGoogleNickname : googleDisplayName = {googleDisplayName}");

        await currentUser.UpdateUserProfileAsync(profile);

        // 초기화
        await currentUser.ReloadAsync();

        // Firebase DB에 닉네임 저장
        await DBManager.Instance.SaveNicknameAsync();
        await currentUser.ReloadAsync();

        Debug.Log("닉네임 설정 성공");
        Debug.Log($"변경된 유저 닉네임 : {currentUser.DisplayName}");
    }
}
