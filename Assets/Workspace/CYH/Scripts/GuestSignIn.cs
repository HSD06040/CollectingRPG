using Firebase.Auth;
using Firebase.Extensions;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Random = UnityEngine.Random;


public class GuestSignIn : MonoBehaviour
{
    [SerializeField] private Button _guestLoginButton;

    private bool _isClicked;
    public Action LoginCompleted { get; set; }

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

            FirebaseUser user = FirebaseManager.Auth.CurrentUser;

            Debug.Log("게스트 생성 완료");

            await user.ReloadAsync();

            // 게스트 닉네임 변경 
            await SetGuestNickname(user);
            await user.ReloadAsync();

            Debug.Log("------유저 정보(GuestLogin)------");
            Debug.Log($"유저 닉네임 : {user.DisplayName}");
            Debug.Log($"유저 ID : {user.UserId}");
            Debug.Log($"이메일 : {user.Email}");

            // LoginPanel -> GameStartPanel 로 변경
            if (user != null)
            {
                Debug.Log("게스트 정보 업데이트 완료. GameStart패널 활성화");
                LoginCompleted?.Invoke();
                _isClicked = false;
            }
        });
    }

    /// <summary>
    /// 익명계정의 DisplayName을 "게스트 + 랜덤숫자"로 변경하는 메서드 
    /// 연결: GuestLogin
    /// </summary>
    /// <param name="currentUser">닉네임을 변경할 유저</param>
    public static async Task SetGuestNickname(FirebaseUser currentUser)
    {
        UserProfile profile = new UserProfile();
        profile.DisplayName = $"게스트{Random.Range(1000, 10000)}";

        await currentUser.UpdateUserProfileAsync(profile);
        // 초기화
        await currentUser.ReloadAsync();
        
        // Firebase DB에 닉네임 저장
        await SaveNicknameAsync();
        await currentUser.ReloadAsync();

        Debug.Log("닉네임 설정 성공");
        Debug.Log($"변경된 유저 닉네임 : {currentUser.DisplayName}");
    }

    public static async Task<bool> SaveNicknameAsync()
    {
        FirebaseUser currentUser = FirebaseManager.Auth.CurrentUser;
        string uid = currentUser.UserId;
        string userNickname = FirebaseManager.Auth.CurrentUser.DisplayName;

        Dictionary<string, object> dictionary = new Dictionary<string, object>();

        // 익명계정 RankData 저장 x
        if (currentUser.IsAnonymous)
        {
            dictionary[$"UserData/{uid}/Nickname"] = userNickname;
        }

        else
        {
            dictionary[$"UserData/{uid}/Nickname"] = userNickname;
            dictionary[$"RankData/{uid}/Nickname"] = userNickname;
        }

        var task = FirebaseManager.DataReference.UpdateChildrenAsync(dictionary);
        await task;

        if (task.IsCompletedSuccessfully)
        {
            Debug.Log("UserData / RankData 에 닉네임 저장 성공");
            return true;
        }
        else
        {
            Debug.LogError("닉네임 저장 실패");
            return false;
        }
    }
}
