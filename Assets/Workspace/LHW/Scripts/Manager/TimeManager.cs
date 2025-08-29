using System;
using UnityEngine;

[Serializable]
public class RewardInfo
{
    public long dateTicks; // DateTime을 Ticks로 저장
    public int state;      // 획득 여부 또는 스택 수

    public RewardInfo(long dateTicks, int state)
    {
        this.dateTicks = dateTicks;
        this.state = state;
    }

    public DateTime GetDateTime()
    {
        return new DateTime(dateTicks);
    }

    public void SetDateTime(DateTime dateTime)
    {
        dateTicks = dateTime.Ticks;
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

    [Header("Reference")]
    [SerializeField] private GoogleAdMob _adMob;
    
    private RewardInfo _dailyFreeGachaRewardInfo;
    public RewardInfo DailyFreeGachaRewardInfo => _dailyFreeGachaRewardInfo;

    private RewardInfo _dailyAdGachaRewardInfo;
    public RewardInfo DailyAdGachaRewardInfo => _dailyAdGachaRewardInfo;

    public Action OnDailyGachaInfoChanged;

    private void Init()
    {
        LoadDailyFreeGachaResetTimeInfo();
        LoadAdGachaResetTimeInfo();
    }

    #region Data Load & Save

    private void LoadDailyFreeGachaResetTimeInfo()
    {
        // 테스트용: 오늘 아침 6시, 가챠횟수 1회
        DateTime todayReset = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 6, 0, 0);
        _dailyFreeGachaRewardInfo = new RewardInfo(todayReset.Ticks, 1);

        if (_dailyFreeGachaRewardInfo.state == 0 && IsDailyFreeGachaResetTime(_dailyFreeGachaRewardInfo.GetDateTime()))
        {
            _dailyFreeGachaRewardInfo.state = 1;
        }
        OnDailyGachaInfoChanged?.Invoke();
    }

    public void SaveDailyFreeGachaResetTimeInfo()
    {
        DateTime now = DateTime.Now;
        DateTime todayReset = new DateTime(now.Year, now.Month, now.Day, 6, 0, 0);
        DateTime nextResetDate = (now.Hour < 6) ? todayReset : todayReset.AddDays(1);

        _dailyFreeGachaRewardInfo.SetDateTime(nextResetDate);
        _dailyFreeGachaRewardInfo.state = 0;

        Debug.Log($"다음 무료 가챠 초기화 시간 : {nextResetDate}");
        OnDailyGachaInfoChanged?.Invoke();
    }

    private void LoadAdGachaResetTimeInfo()
    {
        // 테스트용 초기값: 13시간 전, 가챠 횟수 1회
        _dailyAdGachaRewardInfo = new RewardInfo(DateTime.Now.AddHours(-11).Ticks, 1);

        if (_dailyAdGachaRewardInfo.state < 2 && IsDailyAdGachaResetTime(out int stack))
        {
            _dailyAdGachaRewardInfo.state += stack;
            if (_dailyAdGachaRewardInfo.state > 2) _dailyAdGachaRewardInfo.state = 2;
        }
        OnDailyGachaInfoChanged?.Invoke();
    }

    public void SaveAdGachaResetTimeInfo()
    {
        if (_dailyAdGachaRewardInfo.state > 0)
        {
            if (_adMob.IsReady)
            {
                _adMob.LoadedAd.Show();
            }
            _dailyAdGachaRewardInfo.state -= 1;
            Debug.Log($"광고 가챠 스택 감소: {_dailyAdGachaRewardInfo.state}, 마지막 갱신: {_dailyAdGachaRewardInfo.GetDateTime()}");
        }
        OnDailyGachaInfoChanged?.Invoke();
    }

    #endregion

    public bool CanObtainedFreeGachaReward()
    {
        if (_dailyFreeGachaRewardInfo.state == 1) return true;
        if (IsDailyFreeGachaResetTime(_dailyFreeGachaRewardInfo.GetDateTime())) return true;
        return false;
    }

    public bool CanObtainAdGachaReward()
    {
        if (_dailyAdGachaRewardInfo.state <= 0) return false;

        if (IsDailyAdGachaResetTime(out int stack))
        {
            _dailyAdGachaRewardInfo.state += stack;
            if (_dailyAdGachaRewardInfo.state > 2) _dailyAdGachaRewardInfo.state = 2;
            return true;
        }

        return true;
    }

    private bool IsDailyFreeGachaResetTime(DateTime date)
    {
        DateTime now = DateTime.Now;

        if (now.Year > date.Year && now.Hour >= date.Hour) return true;
        if (now.Year == date.Year && now.Month > date.Month && now.Hour >= date.Hour) return true;
        if (now.Year == date.Year && now.Month == date.Month && now.Day > date.Day && now.Hour >= date.Hour) return true;

        return false;
    }

    /// <summary>
    /// 12시간 단위 누적 스택 계산
    /// </summary>
    private bool IsDailyAdGachaResetTime(out int stack)
    {
        DateTime now = DateTime.Now;
        DateTime lastTime = _dailyAdGachaRewardInfo.GetDateTime();
        TimeSpan difference = now - lastTime;

        stack = 0;

        if (difference.TotalHours >= 12)
        {
            int stackCount = (int)(difference.TotalHours / 12);
            stack = Mathf.Min(stackCount, 2); // 최대 2 스택

            // 마지막 갱신 시간 이동
            _dailyAdGachaRewardInfo.SetDateTime(lastTime.AddHours(12 * stack));

            Debug.Log($"스택 증가: {stack}, 새로운 마지막 갱신 시간: {_dailyAdGachaRewardInfo.GetDateTime()}");
            return true;
        }

        return false;
    }
}