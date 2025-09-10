using Firebase.Database;

public class QuestDBManager 
{
    /// <summary>
    /// 퀘스트 진행 플래그
    /// </summary>
    /// <param name="questID">저장할 퀘스트ID</param>
    private async void SaveQuestCompleteAsync(int questID)
    {
        string uid = FirebaseManager.Auth.CurrentUser.UserId;

        var userQuestRef = FirebaseDatabase.DefaultInstance
            .GetReference("UserData").Child(uid).Child("QuestData").Child("DailyQuest");

        await userQuestRef.Child("DailyList").Child(questID.ToString()).Child("IsCompleted").SetValueAsync(true);

    }

    /// <summary>
    /// 퀘스트 포인트 획득
    /// </summary>
    /// <param name="questID">저장할 퀘스트ID</param>
    private async void SaveQuestPointAsync(int questID, int totalPoint)
    {
        string uid = FirebaseManager.Auth.CurrentUser.UserId;

        var userQuestRef = FirebaseDatabase.DefaultInstance
            .GetReference("UserData").Child(uid).Child("QuestData").Child("DailyQuest");

        await userQuestRef.Child("TotalPoint").SetValueAsync(totalPoint);
        await userQuestRef.Child("DailyList").Child(questID.ToString()).Child("IsReceived").SetValueAsync(true);
    }
}
