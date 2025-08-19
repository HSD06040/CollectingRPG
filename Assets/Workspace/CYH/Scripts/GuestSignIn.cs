using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase.Auth;
using Firebase.Extensions;
using UnityEngine;
using UnityEngine.UI;
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

        // 게스트 로그인 중복 시도 방지
        var current = FirebaseManager.Auth.CurrentUser;
        if (current != null)
        {
            Debug.LogError($"[Auth] 이미 로그인됨 - UID={current.UserId}, 닉네임={current.DisplayName}");
            _isClicked = false;
            return;
        }

        FirebaseManager.Auth
            .SignInAnonymouslyAsync()
            .ContinueWithOnMainThread(async task =>
            {
                if (task.IsCanceled)
                {
                    Debug.LogError("[Auth] 게스트 로그인 취소됨");
                    _isClicked = false;
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError($"[Auth] 게스트 로그인 실패 - 예외: {task.Exception}");
                    _isClicked = false;
                    return;
                }

                var result = task.Result;
                var user = FirebaseManager.Auth.CurrentUser;

                Debug.Log("[Auth] 게스트 로그인 완료");

                // 프로필(닉네임) 설정
                await SetGuestNickname(user);

                // 최종 상태 동기화 (1회)
                await user.ReloadAsync();

                // 요약 로그
                Debug.Log("------ 게스트 로그인(GuestLogin) ------");
                Debug.Log($"현재 닉네임: {user.DisplayName}");
                Debug.Log($"현재 ID: {user.UserId}");
                Debug.Log($"이메일: {user.Email}");

                // UI 전환
                if (user != null)
                {
                    Debug.Log("[Auth] 게스트 로그인 초기화 완료. GameStart 패널 활성화");
                    LoginCompleted?.Invoke();
                }

                _isClicked = false;
            });
    }

    /// <summary>
    /// 익명 사용자(DisplayName)를 "게스트 + 난수"로 설정하고 DB에 저장합니다.
    /// </summary>
    /// <param name="currentUser">현재 로그인된 사용자</param>
    public static async Task SetGuestNickname(FirebaseUser currentUser)
    {
        // 닉네임 생성
        string nickname = $"게스트{Random.Range(1000, 10000)}";
        var profile = new UserProfile { DisplayName = nickname };

        // 프로필 업데이트
        await currentUser.UpdateUserProfileAsync(profile);

        // 서버 상태 동기화 (업데이트 직후 1회)
        await currentUser.ReloadAsync();

        // DB 저장
        bool ok = await SaveNicknameAsync();
        if (ok)
        {
            Debug.Log("닉네임 저장 성공");
        }
        else
        {
            Debug.LogError("닉네임 저장 실패");
        }

        Debug.Log($"최종 설정 닉네임: {currentUser.DisplayName}");
    }

    /// <summary>
    /// UserData/RankData에 닉네임 저장.
    /// 익명 사용자는 UserData만, 비익명은 UserData+RankData.
    /// </summary>
    public static async Task<bool> SaveNicknameAsync()
    {
        var currentUser = FirebaseManager.Auth.CurrentUser;
        if (currentUser == null)
        {
            Debug.LogError("[DB] 저장 실패 - CurrentUser가 null");
            return false;
        }

        string uid = currentUser.UserId;
        string userNickname = currentUser.DisplayName;

        var dictionary = new Dictionary<string, object>();

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

        Debug.LogError("닉네임 저장 실패");
        return false;
    }
}
