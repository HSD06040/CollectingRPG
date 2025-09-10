using System;

//DB에 저장할 유저 퀘스트 데이터
[Serializable]
public class UserQuestData 
{
    // Firebase UserData-uid-QuestData
    public string DayKey;
    public int TotalPoint;
    public bool IsRewardReceived_1;
    public bool IsRewardReceived_2;
    public bool IsRewardReceived_3;
    public bool IsRewardReceived_4;
    public bool IsRewardReceived_5;
}

[Serializable]
public class DailyQuestFlags   
{
    // Firebase UserData-uid-QuestData-QuestList-QuestID
    public bool IsCompleted;    //퀘스트 완료 여부
    public bool IsReceived;     // 포인트 수령 여부
}
