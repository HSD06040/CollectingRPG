
public interface IQuestView 
{
    bool IsComplete { get; }
    bool IsReceived { get; }
    int QuestID { get; }
    QuestType QuestType { get; }
    string QuestDesc { get; }
    int RewardPoint { get; }
    int MaxProgress { get; }
    int CurProgress { get; }
}
