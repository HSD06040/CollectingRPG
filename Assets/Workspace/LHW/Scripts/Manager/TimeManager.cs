using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RewardInfo
{
    public string date; // 초기화 시간
    public int state; // 획득 여부
}

public class TimeManager : MonoBehaviour
{
    #region Singleton

    public static TimeManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); 
    }

    #endregion

    // 예시 : 일일퀘스트 초기화 및 획득 정보
    [SerializeField] private List<RewardInfo> _dailyRewardInfo;

    // 예시 : 주간퀘스트 초기화 및 획득 정보
    [SerializeField] private List<RewardInfo> _weeklyRewardInfo;

    [SerializeField] private RewardInfo _dailyGachaRewardInfo;

    public void LoadDailyGachaResetTimeInfo()
    {
        // 데이터베이스에서 정보를 로드
    }
    
    public void SaveDailyGachaResetTimeInfo()
    {
        // 데이터베이스에 정보를 업로드
    }

    public bool HasObtainedGachaReward()
    {
        // 정보를 바탕으로 보상 획득 가능 여부 체크
        return true;
    }
}