using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopUIController : MonoBehaviour
{
    // 상점 UI 로 추가되어야 할 것으로 예상되는 내용

    // 1. 상점 아이템 리스트를 받아 아이템 목록 및 수량을 변경
    // 2. 상점에서 아이템을 선택했을 때 해당 아이템을 살 것인지 말 것인지에 대한 팝업 설정 - PopUpManager로 설정 가능
    // 3. 일일 상점 초기화, 광고 보고 상점 아이템 구매 요소 등

    Coroutine _dailyCooltimeTimer;

    [SerializeField] TMP_Text _dailyResetText;
    [SerializeField] private Button _testButton;

    [Header("SlotData")]
    [SerializeField] private ShopItemSO _itemDB;
    [SerializeField] private ShopSlot _slotPrefab;
    [SerializeField] private Transform _dailyList;
    [SerializeField] private Transform _goldList;
    [SerializeField] private Transform _diamondList;

    [Header("Reroll")]
    [SerializeField] private Button _rerollButton;
    [SerializeField] private TMP_Text _rerollCountText;
    [SerializeField] private TMP_Text _rerollPriceText;
    [SerializeField] private Image _rerollPriceImage;
    [SerializeField] private Sprite _goldSprite;
    [SerializeField] private Sprite _diamondSprite;

    [Header("Ad Reroll")]
    [SerializeField] GoogleAdMob _googleAdMob;
    [SerializeField] private Button _adRerollButton;
    [SerializeField] private TMP_Text _adRerollCountText;

    private ShopSlotFactory _shopSlotFactory = new ShopSlotFactory();
    private DailyShopManager _dailyManager;


    private async void Start()
    {
        await InitShopUI();
        
        _dailyManager = GetComponent<DailyShopManager>();

        _rerollButton.onClick.AddListener(OnClickRefresh);
        _testButton.onClick.AddListener(RerollShopData);
        _adRerollButton.onClick.AddListener(OnClickAdReroll);
  
        // 새로고침 횟수 UI 초기화
        UpdateRerollButtonUI();
        UpdateAdRerollButtonUI();
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
                    RerollShopData();
                }

                DateTime now = DateTime.Now;
                TimeSpan cooltime = nextDate - now;
                _dailyResetText.text = $"다음 초기화 : {cooltime.Hours}시간 {cooltime.Minutes}분";
            }

            yield return new WaitForSeconds(1);
        }
    }

    #region Init Shop

    /// <summary>
    /// 상점 UI 초기화 (Daily / Gold / Diamond 슬롯 세팅)
    /// </summary>
    private async Task InitShopUI()
    {
        // Daily
        await InitDailyShop();

        // Gold
        InitGoldShop();

        // Diamond
        InitDiamondShop();
    }

    private async Task InitDailyShop()
    {
        foreach (Transform child in _dailyList)
        {
            Destroy(child.gameObject);
        }

        DailyShopManager dailyShopManager = new DailyShopManager();
        List<ShopSlotData> dailySlots = await dailyShopManager.GetDailySlotsAsync();

        foreach (var slot in dailySlots)
        {
            Instantiate(_slotPrefab, _dailyList).SetSlot(slot);
        }
    }

    private void InitGoldShop()
    {
        foreach (Transform child in _goldList)
        {
            Destroy(child.gameObject);
        }

        foreach (var meta in _itemDB.Items)
        {
            if (meta.Type == ShopType.Gold)
            {
                ShopSlotData slot = _shopSlotFactory.FromGold(meta.ItemId, _itemDB);
                Instantiate(_slotPrefab, _goldList).SetSlot(slot);
            }
        }
    }

    private void InitDiamondShop()
    {
        foreach (Transform child in _diamondList)
        {
            Destroy(child.gameObject);
        }

        foreach (var meta in _itemDB.Items)
        {
            if (meta.Type == ShopType.Diamond)
            {
                ShopSlotData slot = _shopSlotFactory.FromDiamond(meta.ItemId, _itemDB);
                Instantiate(_slotPrefab, _diamondList).SetSlot(slot);
            }
        }
    }

    #endregion


    #region currency

    /// <summary>
    /// 새로고침 버튼 클릭 시 실행되는 메서드
    /// </summary>
    private async void OnClickRefresh()
    {
        if (!_dailyManager.CanReroll())
        {
            _rerollButton.interactable = false;
            return;
        }

        List<ShopSlotData> newSlots = await _dailyManager.RerollDailySlotAsync();

        if (newSlots != null)
        {
            foreach (Transform child in _dailyList)
                Destroy(child.gameObject);

            foreach (var slot in newSlots)
                Instantiate(_slotPrefab, _dailyList).SetSlot(slot);
        }

        UpdateRerollButtonUI();

        if (!_dailyManager.CanReroll())
        {
            _rerollButton.interactable = false;
        }
    }

    /// <summary>
    /// 새로고침 버튼 UI 갱신하는 메서드
    /// </summary>
    private void UpdateRerollButtonUI()
    {
        int stageCount = _dailyManager.GetStageCount();
        int stageMax = _dailyManager.GetStageMax();
        (int cost, string currency) = _dailyManager.GetRefreshCost();

        _rerollCountText.text = $"{stageCount}/{stageMax}";
        _rerollButton.interactable = _dailyManager.CanReroll();

        // 가격 텍스트/아이콘 갱신
        if (currency == "Free")
        {
            _rerollPriceText.text = "무료";
            _rerollPriceImage.enabled = false;

            // 텍스트 위치 조정
            RectTransform rect = _rerollPriceText.GetComponent<RectTransform>();
            Vector2 pos = rect.anchoredPosition;
            pos.x = 16f;
            rect.anchoredPosition = pos;
        }
        else
        {
            _rerollPriceText.text = cost.ToString();
            _rerollPriceImage.sprite = (currency == "Gold") ? _goldSprite : _diamondSprite;
            _rerollPriceImage.enabled = true;

            // 텍스트 위치 조정
            RectTransform rect = _rerollPriceText.GetComponent<RectTransform>();
            Vector2 pos = rect.anchoredPosition;
            pos.x = 41f;
            rect.anchoredPosition = pos;
        }
    }

    #endregion


    #region AD

    /// <summary>
    /// 광고시청 새로고침 버튼 클릭 시 실행되는 메서드
    /// </summary>
    private void OnClickAdReroll()
    {
        if (!_dailyManager.CanAdReroll())
        {
            _adRerollButton.interactable = false;
            return;
        }

        // 광고 실행
        _googleAdMob.ShowAd(OnAdWatched);
    }

    private async void OnAdWatched()
    {
        // 광고 끝난 후 count 증가 / 새로고침
        List<ShopSlotData> newSlots = await _dailyManager.RerollDailySlotByAdAsync();

        if (newSlots != null)
        {
            foreach (Transform child in _dailyList)
                Destroy(child.gameObject);

            foreach (var slot in newSlots)
                Instantiate(_slotPrefab, _dailyList).SetSlot(slot);
        }

        UpdateAdRerollButtonUI();
    }

    /// <summary>
    /// 광고시청 새로고침 버튼 UI 갱신하는 메서드
    /// </summary>
    private void UpdateAdRerollButtonUI()
    {
        int current = _dailyManager.GetAdRefreshCount();
        int max = 2;

        _adRerollCountText.text = $"{current}/{max}";
        _adRerollButton.interactable = _dailyManager.CanAdReroll();
    }

    #endregion

    /// <summary>
    /// 일일상점을 초기화하는 메서드
    /// </summary>
    private async void RerollDailyShop()
    {
        await InitDailyShop();
        _dailyManager.RerollCount();
        _dailyManager.RerollAdCount();

        UpdateRerollButtonUI();
        UpdateAdRerollButtonUI();
    }

    /// <summary>
    /// 상점 전체를 초기화하는 메서드
    /// 타이머 초기화 시 호출
    /// </summary>
    private void RerollShopData()
    {
        // Dia, Gold 무료 획득 초기화
        InitGoldShop();
        InitDiamondShop();

        //새로고침 버튼 횟수 초기화, 일일상점 상품 리스트 초기화
        RerollDailyShop();
    }

}