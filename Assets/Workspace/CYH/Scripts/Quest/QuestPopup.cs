using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Threading.Tasks;

public class QuestPopup : MonoBehaviour
{
    [SerializeField] private QuestManager _questManager;

    [Header("List")]
    [SerializeField] private RectTransform _content;
    [SerializeField] private GameObject _QuestPrefab;

    [Header("UI")]
    [SerializeField] private TMP_Text _remainTime;
    [SerializeField] private Image _progressBar;

    private DateTime _resetTime;
    private Coroutine _initRoutine;
    private Coroutine _countdownRoutine;

    private void Start()
    {
        // 현재 날짜/서버 저장 날짜 비교 후 
        // 현재 시간 > 서버 저장 날짜 -> 퀘스트 리셋

        Init();
    }

    private void OnEnable()
    {
        // 타이머 시작
        _initRoutine = StartCoroutine(InitAndStartRoutine());
    }

    private void OnDisable()
    {
        // 타이머 정지
        if (_initRoutine != null)
        {
            StopCoroutine(_initRoutine);
            _initRoutine = null;
        }

        if (_countdownRoutine != null)
        {
            StopCoroutine(_countdownRoutine);
            _countdownRoutine = null;
        }
    }

    private void Init()
    {
        foreach (var quest in _questManager._quests)
        {
            if (quest is IQuestView view)
            {
                GameObject questItem = Instantiate(_QuestPrefab, _content);
                questItem.GetComponent<QuestItem>().Init(view);
            }
        }
    }

    private IEnumerator InitAndStartRoutine()
    {
        Task<DateTime> task = GetKstNowAsync();
        yield return new WaitUntil(() => task.IsCompleted);

        DateTime kstNow = task.Result;
        DateTime todayReset = new DateTime(kstNow.Year, kstNow.Month, kstNow.Day, 6, 0, 0);
        
        if (kstNow >= todayReset)
        {
            todayReset = todayReset.AddDays(1);
        }

        _resetTime = todayReset;
        _countdownRoutine = StartCoroutine(UpdateRemainRoutine());
    }

    private IEnumerator UpdateRemainRoutine()
    {
        while (true)
        {
            TimeSpan remain = _resetTime - DateTime.Now;

            if (remain <= TimeSpan.Zero)
            {
                Debug.Log("퀘스트 초기화 시간 / 서버 시간 재동기화");
                _initRoutine = StartCoroutine(InitAndStartRoutine());
                yield break;
            }

            _remainTime.text = string.Format("{0:D2}:{1:D2}:{2:D2}", remain.Hours, remain.Minutes, remain.Seconds);
            
            yield return new WaitForSeconds(1f);
        }
    }

    private async Task<DateTime> GetKstNowAsync()
    {
        long serverTime = await Manager.DB.LoadSeverTimeAsync();

        DateTime utcNow = DateTimeOffset.FromUnixTimeMilliseconds(serverTime).UtcDateTime;
        DateTime kstNow = utcNow.AddHours(9);

        return kstNow;
    }
}
