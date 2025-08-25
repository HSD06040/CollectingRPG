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
            callback(nickname);
        }
        else
        {
            Debug.LogWarning("닉네임 데이터 없음");
            callback(null);
        }
    }

    /// <summary>
    /// Firebase DB UserData에서 LoabbyScene에 표시되어야 할 모든 데이터를 불러오는 메서드
    /// </summary>
    public async Task<PlayerData> LoadLobbyDataAsync()
    {
        string uid = FirebaseManager.Auth.CurrentUser.UserId;
        DatabaseReference userRef = FirebaseManager.DataReference.Child("UserData").Child(uid);

        DataSnapshot snapshot = await userRef.GetValueAsync();
        string nickname = snapshot.Value.ToString();

        if (!snapshot.Exists)
        {
            Debug.LogWarning("데이터 없음");
        }

        PlayerData data = new PlayerData
        {
            PlayerUid = uid,
            PlayerName = snapshot.Child("Nickname").Value?.ToString() ?? "LoadFailed",
            Gold = int.TryParse(snapshot.Child("Gold").Value?.ToString(), out int gold) ? gold : 0,
            Diamond = int.TryParse(snapshot.Child("Diamond").Value?.ToString(), out int diamond) ? diamond : 0
        };

        return data;
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
        DatabaseReference userRef = FirebaseManager.DataReference.Child("UserData").Child(uid).Child("IsTutorialComplete");

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
            Debug.Log("IsTutorialComplete: true / 튜토리얼 진행 계정");
            // 로비 씬으로 전환
            return true;
        }
        else
        {
            Debug.Log("IsTutorialComplete: false / 튜토리얼 미진행 계정");
            // 튜토리얼 패널 활성화
            return false;
        }
    }

    /// <summary>
    /// 튜토리얼 진행 후 튜토리얼 진행 여부 변수를 true 로 설정하는 메서드
    /// IsTutorialComplete = false
    /// </summary>
    public async Task SetTutorialInCompleteAsync()
    {
        string userUid = FirebaseManager.Auth.CurrentUser.UserId;
        DatabaseReference userRef = FirebaseManager.DataReference.Child("UserData").Child(userUid).Child("IsTutorialComplete");

        await userRef.SetValueAsync(false);
    }

    /// <summary>
    /// 계정의 생성 후 튜토리얼 진행 여부 변수를 false 로 설정하는 메서드
    /// IsTutorialComplete = false
    /// </summary>
    public async Task SetTutorialCompleteAsync()
    {
        string userUid = FirebaseManager.Auth.CurrentUser.UserId;
        DatabaseReference userRef = FirebaseManager.DataReference.Child("UserData").Child(userUid).Child("IsTutorialComplete");

        await userRef.SetValueAsync(true);
    }

    /// <summary>
    /// 유저의 Gold와 Diamond 값을 동시에 저장하는 메서드
    /// </summary>
    /// <param name="gold">저장할 골드 값</param>
    /// <param name="diamond">저장할 다이아 값</param>
    public async Task<bool> SaveCurrencyAsync(int gold, int diamond)
    {
        string uid = FirebaseManager.Auth.CurrentUser.UserId;

        Dictionary<string, object> dictionary = new Dictionary<string, object>();

        dictionary[$"UserData/{uid}/Gold"] = gold;
        dictionary[$"UserData/{uid}/Diamond"] = diamond;

        var task = FirebaseManager.DataReference.UpdateChildrenAsync(dictionary);
        await task;

        if (task.IsCompletedSuccessfully)
        {
            Debug.Log($"골드/다이아 저장 성공: Gold = {gold}, Diamond = {diamond}");
            return true;
        }
        else
        {
            Debug.LogError("골드/다이아 저장 실패");
            return false;
        }
    }

    /// <summary>
    /// 유저의 현재 Gold 값을 읽어와 지정한 값만큼 증가시킨 뒤 저장하는 메서드
    /// </summary>
    /// <param name="addAmount">증가시킬 Gold 양</param>
    /// <returns></returns>
    public async Task AddGoldAsync(int addAmount)
    {
        string uid = FirebaseManager.Auth.CurrentUser.UserId;
        var goldRef = FirebaseManager.DataReference.Child("UserData").Child(uid).Child("Gold");

        DataSnapshot snapshot = await goldRef.GetValueAsync();
        
        int current = 0;

        if (snapshot.Exists && snapshot.Value != null)
        {
            current = Convert.ToInt32(snapshot.Value);
        }
        
        int next = current + addAmount;

        await goldRef.SetValueAsync(next);
        Debug.Log($"증가한 Gold: {addAmount} -> {next}");
    }

    /// <summary>
    /// 유저의 현재 Diamond 값을 읽어와 지정한 값만큼 증가시킨 뒤 저장하는 메서드
    /// </summary>
    /// <param name="addAmount">증가시킬 Diamond 양</param>
    /// <returns></returns>
    public async Task AddDiamondAsync(int addAmount)
    {
        string uid = FirebaseManager.Auth.CurrentUser.UserId;
        var goldRef = FirebaseManager.DataReference.Child("UserData").Child(uid).Child("Diamond");

        DataSnapshot snapshot = await goldRef.GetValueAsync();

        int current = 0;

        if (snapshot.Exists && snapshot.Value != null)
        {
            current = Convert.ToInt32(snapshot.Value);
        }

        int next = current + addAmount;

        await goldRef.SetValueAsync(next);
        Debug.Log($"증가한 Diamond: {addAmount} -> {next}");
    }
}
