using System;
using System.Collections;
using UnityEngine;
using TMPro;

public class ShopUIController : MonoBehaviour
{
    // 상점 UI 로 추가되어야 할 것으로 예상되는 내용

    // 1. 상점 아이템 리스트를 받아 아이템 목록 및 수량을 변경
    // 2. 상점에서 아이템을 선택했을 때 해당 아이템을 살 것인지 말 것인지에 대한 팝업 설정 - PopUpManager로 설정 가능
    // 3. 일일 상점 초기화, 광고 보고 상점 아이템 구매 요소 등

    [SerializeField] TMP_Text _dailyResetText;

    Coroutine _dailyCooltimeTimer;

    [Header("SlotData")]
    [SerializeField] private ShopItemSO _itemDB;
    [SerializeField] private ShopSlot _slotPrefab;
    [SerializeField] private Transform _dailyList;
    [SerializeField] private Transform _goldList;
    [SerializeField] private Transform _diamondList;

    private ShopSlotFactory _shopSlotFactory = new ShopSlotFactory();


    private void Start()
    {
        string uid = FirebaseManager.Auth.CurrentUser.UserId;

        // Gold 
        foreach (var meta in _itemDB.Items)
        {
            if (meta.Type == ShopType.Gold)
            {
                ShopSlotData slot = _shopSlotFactory.FromGold(meta.ItemId, _itemDB);
                Instantiate(_slotPrefab, _goldList).SetSlot(slot);
            }
        }

        // Diamond
        foreach (var meta in _itemDB.Items)
        {
            if (meta.Type == ShopType.Diamond)
            {
                var slot = _shopSlotFactory.FromDiamond(meta.ItemId, _itemDB);
                Instantiate(_slotPrefab, _diamondList).SetSlot(slot);
            }
        }
    }

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
                    
                }

                DateTime now = DateTime.Now;
                TimeSpan cooltime = nextDate - now;
                _dailyResetText.text = $"다음 초기화 : {cooltime.Hours}시간 {cooltime.Minutes}분";
            }

            yield return new WaitForSeconds(1);
        }
    }
}