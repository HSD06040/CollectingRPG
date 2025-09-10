using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance { get; private set; }

    //  모든 퀘스트 데이터
    [SerializeField] internal List<ScriptableObject> _quests;

    private void Awake()
    {
        SetSingleton();

        Subscribe();
    }

    private void OnDestroy()
    {
        UnSubscribe();
    }

    #region Init
   
    //  (이벤트 연결 및 해제 / 싱글톤 세팅)
    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Subscribe()
    {
        foreach (var questObj in _quests)
        {
            if (questObj is IQuest condition)
            {
                condition.Subscribe(() => ClearHandler(condition));
            }
        }
    }

    private void UnSubscribe()
    {
        foreach (var questObj in _quests)
        {
            if (questObj is IQuest condition)
            {
                condition.Unsubscribe(() => ClearHandler(condition));
            }
        }
    }

    #endregion

    private void ClearHandler(IQuest condition)
    {
        if (condition is QuestDataSO<object> quest)
        {
            if (!quest.IsComplete)
            {
                quest.ClearQuest();
            }
        }
    }

    public void CallHandler<T>(QuestDataSO<T> condition, T value)
    {
        condition.ClearCondition(value);
    }

    #region 각 조건 클리어 시 호출할 함수

    public void OnMonsterKilled(BattleQuest condition, int monsterID)
    {
        CallHandler<int>(condition, monsterID);
    }

    public void OnBossKilled(BattleQuest condition, int bossID)
    {
        CallHandler<int>(condition, bossID);
    }

    public void OnItemCollected(ItemQuest condition, bool useItem)
    {
        CallHandler<bool>(condition, useItem);
    }

    public void OnItemUsed(ItemQuest condition, bool useItem)
    {
        CallHandler<bool>(condition, useItem);
    }

    public void OnAdWatched(ItemQuest condition, bool adWatched)
    {
        CallHandler<bool>(condition, adWatched);
    }

    public void OnStateChanged(StateQuest condition, bool state)
    {
        CallHandler<bool>(condition, state);
    }
    #endregion

}
