using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

[Serializable]
public class RewardInfo
{
    public string date; // 초기화 시간
    public int state; // 획득 여부

    public RewardInfo(string date, int state)
    {
        this.date = date;
        this.state = state;
    }
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
        Init();
    }

    #endregion

    [SerializeField] private RewardInfo _dailyGachaRewardInfo;
    private DateTime _dailyResetTime;

    private void Init()
    {
        LoadDailyGachaResetTimeInfo();
    }

    #region Data Load&Save

    private void LoadDailyGachaResetTimeInfo()
    {
        // TODO : 데이터베이스에서 정보를 로드
        // 우선은 테스트용으로 로컬에서 수치를 직접 넣어 테스트를 진행
        _dailyGachaRewardInfo = new RewardInfo("2025/08/29 06:00", 0);

        _dailyResetTime = DateTime.ParseExact(_dailyGachaRewardInfo.date, "yyyy/MM/dd HH:mm", null);

        // 날짜에 따라 가챠 값을 초기화
        if (_dailyGachaRewardInfo.state == 1 && IsDailyResetTime(_dailyResetTime))
        {
            _dailyGachaRewardInfo.state = 0;
        }
    }
    
    public void SaveDailyGachaResetTimeInfo()
    {
        // TODO : 데이터베이스에 정보를 업로드
        // 우선은 테스트용으로 로컬에서 수치를 직접 넣어 테스트를 진행
        DateTime now = DateTime.Now;

        DateTime todayReset = new DateTime(now.Year, now.Month, now.Day, 6, 0, 0);

        DateTime nextResetDate;
        
        // now 기준으로 오전 6시 이전일 경우 당일 오전 6시로 nextResetDate를 설정
        if (now.Hour < 6)
        {
            nextResetDate = todayReset;
        }
        // now 기준으로 오전 6시 이후일 경우 다음 날 오전 6시로 nextResetDate를 설정
        else
        {
            nextResetDate = todayReset.AddDays(1);
        }

        _dailyGachaRewardInfo.date = nextResetDate.ToString();
        _dailyGachaRewardInfo.state = 1;
        Debug.Log($"다음 가챠 초기화 시간 : {_dailyGachaRewardInfo.date}");
    }

    #endregion
    public bool CanObtainedGachaReward(DateTime date)
    {
        // 정보를 바탕으로 보상 획득 가능 여부 체크

        if (_dailyGachaRewardInfo.state == 1) return false;

        if (IsDailyResetTime(date))
        {
            return true;
        }

        return false;
    }

    private bool IsDailyResetTime(DateTime date)
    {
        DateTime now = DateTime.Now;

        // 연도가 바뀌고 24시간을 지났으면 하루는 무조건 지났으므로 true
        if(now.Year > date.Year && now.Hour >= date.Hour) return true;

        // 연도가 같고 한달 말일에서 다음달 초일을 지나고 24시간이 지났으면 하루는 무조건 지났으므로 true
        if(now.Year == date.Year && now.Month > date.Month && now.Hour >= date.Hour) return true;

        // 연도가 같고 달이 같고 하루가 지난 상태에서 24시간이 지났으면 하루는 무조건 지났으므로 true
        if(now.Year == date.Year && now.Month == date.Month && now.Day >= date.Day && now.Hour >= date.Hour) return true;
        
        return false;
    }
}