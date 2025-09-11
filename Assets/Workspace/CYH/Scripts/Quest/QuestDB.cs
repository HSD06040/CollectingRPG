using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase.Database;
using UnityEngine;

public class QuestDB
{
    /// <summary>
    /// 유저의 일일퀘스트 데이터를 로드하는 메서드
    /// 데이터가 없으면 defaultData 생성 후 반환
    /// </summary>
    /// <returns>로드된/새로 생성된 유저 퀘스트 데이터<</returns>
    public async Task<UserQuestData> LoadUserQuestDataAsync()
    {
        string uid = FirebaseManager.Auth.CurrentUser.UserId;

        DatabaseReference userQuestRef = FirebaseManager.DataReference
            .Child("UserData").Child(uid).Child("QuestData").Child("DailyQuest");

        DataSnapshot snapShot = await userQuestRef.GetValueAsync();
        
        // 기존 데이터 x
        if (!snapShot.Exists)
        {
            Debug.LogWarning("UserQuestData 없음 -> defaultData 생성 후 저장");

            // defaultData 생성
            var defaultData = new UserQuestData(DateTime.Now.ToString("yyyyMMdd"), 0);
            string json = JsonUtility.ToJson(defaultData);
  
            await userQuestRef.SetRawJsonValueAsync(json);

            QuestManager.Instance.ApplyUserQuestData(defaultData);

            return defaultData;
        }

        // 기존 데이터 o
        var jsonData = snapShot.GetRawJsonValue();
        var data = JsonUtility.FromJson<UserQuestData>(jsonData);

        QuestManager.Instance.ApplyUserQuestData(data);
        
        return data;
    }

    /// <summary>
    /// 유저의 마일스톤 데이터를 로드하는 메서드
    /// 데이터가 없으면 생성 후 반환
    /// </summary>
    /// <returns>로드된 마일스톤 데이터</returns>
    public async Task<MilestoneData> LoadUserMilestoneAsync()
    {
        string uid = FirebaseManager.Auth.CurrentUser.UserId;

        DatabaseReference milestoneRef = FirebaseManager.DataReference
            .Child("UserData").Child(uid).Child("QuestData").Child("DailyQuest").Child("Milestone");

        DataSnapshot snapshot = await milestoneRef.GetValueAsync();

        // 기존 데이터 x
        if (!snapshot.Exists)
        {
            Debug.LogWarning("Milestone 데이터 x");
            SaveUserMilestoneAsync();
            return new MilestoneData(new bool[5]);
        }

        // 기존 데이터 o
        string json = snapshot.GetRawJsonValue();
        var data = JsonUtility.FromJson<MilestoneData>(json);

        Debug.Log("[LoadUserMilestoneAsync] MilestoneStates:");
        for (int i = 0; i < data.MilestoneStates.Length; i++)
        {
            Debug.Log($" Index {i}: {data.MilestoneStates[i]}");
        }

        // 인게임 연결
        QuestManager.Instance.ApplyMilestoneData(data);
        
        Debug.Log("LoadUserMilestoneAsync");
        return data;
    }

    /// <summary>
    /// Daykey, TotalPoint를 유저DB에 저장하는 메서드
    /// </summary>
    public async void SaveUserQuestDataAsync()
    {
        string uid = FirebaseManager.Auth.CurrentUser.UserId;

        DatabaseReference userQuestRef = FirebaseManager.DataReference
            .Child("UserData").Child(uid).Child("QuestData").Child("DailyQuest");

        await userQuestRef.Child("DayKey").SetValueAsync(QuestManager.Instance.UserQuestData.DayKey);
        await userQuestRef.Child("TotalPoint").SetValueAsync(QuestManager.Instance.UserQuestData.TotalPoint);
    }

    /// <summary>
    /// Milestone 전체 데이터를 유저DB에 저장하는 메서드
    /// </summary>
    public async void SaveUserMilestoneAsync()
    {
        string uid = FirebaseManager.Auth.CurrentUser.UserId;

        DatabaseReference userMilestoneRef = FirebaseManager.DataReference
            .Child("UserData").Child(uid).Child("QuestData").Child("DailyQuest").Child("Milestone");

        string json = JsonUtility.ToJson(QuestManager.Instance.MilestoneData);
        await userMilestoneRef.SetRawJsonValueAsync(json);

        Debug.Log("SaveUserMilestoneAsync");
    }

    /// <summary>
    /// 지정한 Milestone 보상 수령 여부를 유저DB에 저장하는 메서드
    /// </summary>
    /// <param name="index">마일스톤 인덱스</param>
    /// <param name="isReceived">보상 수령 여부</param>
    public async void SaveDailyRewardAsync(int index, bool isReceived)
    {
        string uid = FirebaseManager.Auth.CurrentUser.UserId;

        DatabaseReference userQuestRef = FirebaseDatabase.DefaultInstance
            .GetReference("UserData").Child(uid).Child("QuestData").Child("DailyQuest");

        await userQuestRef.Child("Milestone").Child("MilestoneStates").Child($"{index}").SetValueAsync(isReceived);
    }
}
