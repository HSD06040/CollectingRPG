using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class ShopUIController : MonoBehaviour
{
    // 상점 UI 로 추가되어야 할 것으로 예상되는 내용

    // 1. 상점 아이템 리스트를 받아 아이템 목록 및 수량을 변경
    // 2. 상점에서 아이템을 선택했을 때 해당 아이템을 살 것인지 말 것인지에 대한 팝업 설정 - PopUpManager로 설정 가능
    // 3. 일일 상점 초기화, 광고 보고 상점 아이템 구매 요소 등

    [SerializeField] TMP_Text _dailyResetText;

    Coroutine _dailyCooltimeTimer;

    private void OnEnable()
    {
        StartDailyTimer();
    }

    private void OnDisable()
    {
        StopDailyTimer();
    }

    private void StartDailyTimer()
    {
        if (_dailyCooltimeTimer != null)
        {
            StopCoroutine(_dailyCooltimeTimer);
        }
        _dailyCooltimeTimer = StartCoroutine(DailyCooltimeCoroutine());
    }

    private void StopDailyTimer()
    {
        if (_dailyCooltimeTimer != null)
        {
            StopCoroutine(_dailyCooltimeTimer);
            _dailyCooltimeTimer = null;
        }
    }

    private IEnumerator DailyCooltimeCoroutine()
    {
        while (true)
        {
            if (TimeManager.Instance != null)
            {
                bool isResetTime = TimeManager.Instance.LoadDailyShopResetTime(out DateTime nextDate);

                if (isResetTime)
                {
                    // 초기화 과정 넣기
                }

                DateTime now = DateTime.Now;
                TimeSpan cooltime = nextDate - now;
                _dailyResetText.text = $"다음 초기화 : {cooltime.Hours}시간 {cooltime.Minutes}분";
            }

            yield return new WaitForSeconds(1);
        }
    }
}