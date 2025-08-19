using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Firebase.Auth;
using Firebase.Database;

public class DBManager : Singleton<DBManager>
{
    /// <summary>
    /// 유저 닉네임(displayname)을 DB에 저장하는 메서드
    /// </summary>
    /// <returns>
    /// true: 저장 성공
    /// false: 저장 실패
    /// </returns>
    public async Task<bool> SaveNicknameAsync()
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
            return true;
        }
        else
        {
            Debug.LogError("닉네임 저장 실패");
            return false;
        }
    }

    /// <summary>
    /// 유저 닉네임(string)을 DB에 저장하는 메서드
    /// </summary>
    /// <param name="nickname">저장할 닉네임</param>
    /// <returns>
    /// true: 저장 성공
    /// false: 저장 실패
    /// </returns>
    public async Task<bool> SaveNicknameAsync(string nickname)
    {
        FirebaseUser currentUser = FirebaseManager.Auth.CurrentUser;

        string uid = currentUser.UserId;
        string userNickname = nickname;

        Dictionary<string, object> dictionary = new Dictionary<string, object>();

        dictionary[$"UserData/{uid}/Nickname"] = userNickname;

        var task = FirebaseManager.DataReference.UpdateChildrenAsync(dictionary);
        await task;

        if (task.IsCompletedSuccessfully)
        {
            return true;
        }
        else
        {
            Debug.LogError("닉네임 저장 실패");
            return false;
        }
    }

    /// <summary>
    /// Firebase DB UserData에서 현재 유저 닉네임을 불러오는 메서드
    /// </summary>
    /// <param name="callback">로드된 닉네임을 반환하는 콜백 (없을 경우 null)</param>
    public async Task LoadNicknameAsync(Action<string> callback)
    {
        string uid = FirebaseManager.Auth.CurrentUser.UserId;
        DatabaseReference nicknameRef = FirebaseManager.DataReference.Child("UserData").Child(uid).Child("Nickname");

        DataSnapshot snapshot = await nicknameRef.GetValueAsync();
        string nickname = snapshot.Value.ToString();

        if (snapshot.Exists)
        {
            Debug.Log($"닉네임 로드 성공 : {nickname}");
            callback(nickname);
        }
        else
        {
            Debug.LogWarning("닉네임 데이터 없음");
            callback(null);
        }
    }

    /// <summary>
    /// 계정 로그인 시 튜토리얼 진행 여부를 체크하는 메서드
    /// </summary>
    /// <returns>
    /// true: 튜토리얼 진행
    /// false: 튜토리얼 미진행
    /// </returns>
    public async Task<bool> CheckTutorialCompletedAsync()
    {
        FirebaseAuth auth = FirebaseManager.Auth;
        string uid = FirebaseManager.Auth.CurrentUser.UserId;
        DatabaseReference userRef = FirebaseManager.DataReference.Child("UserData").Child(uid).Child("isTutorialComplete");

        // 튜토리얼 진행 여부 확인
        DataSnapshot snapshot = await userRef.GetValueAsync();

        if (!snapshot.Exists)
        {
            Debug.LogWarning($"튜토리얼 상태 null / 유저 UID : {uid}");
            return false;
        }

        // 튜토리얼 진행
        if (snapshot.Value is bool isTutorialComplete)
        {
            Debug.Log("isTutorialComplete: true / 튜토리얼 진행 계정");
            // 로비 씬으로 전환
            return true;
        }
        else
        {
            Debug.Log("isTutorialComplete: false / 튜토리얼 미진행 계정");
            // 튜토리얼 패널 활성화
            return false;
        }
    }

    /// <summary>
    /// 튜토리얼 진행 후 튜토리얼 진행 여부 변수를 true 로 설정하는 메서드
    /// isTutorialComplete = false
    /// </summary>
    public async Task SetTutorialInCompleteAsync()
    {
        string userUid = FirebaseManager.Auth.CurrentUser.UserId;
        DatabaseReference userRef = FirebaseManager.DataReference.Child("UserData").Child(userUid).Child("isTutorialComplete");

        await userRef.SetValueAsync(false);
    }

    /// <summary>
    /// 계정의 생성 후 튜토리얼 진행 여부 변수를 false 로 설정하는 메서드
    /// isTutorialComplete = false
    /// </summary>
    public async Task SetTutorialCompleteAsync()
    {
        string userUid = FirebaseManager.Auth.CurrentUser.UserId;
        DatabaseReference userRef = FirebaseManager.DataReference.Child("UserData").Child(userUid).Child("isTutorialComplete");

        await userRef.SetValueAsync(true);
    }
}
