using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;

public class DBManager : Singleton<DBManager>
{
    /// <summary>
    /// 유저 닉네임(displayname)을 DB에 저장
    /// </summary>
    /// <returns></returns>
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
