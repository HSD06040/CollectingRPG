using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopSlot : MonoBehaviour
{
    private ShopSlotData _slotData;
    [SerializeField] private ShopType _type;

    [Header("UI Components")]
    [SerializeField] private TMP_Text _itemNameText;
    [SerializeField] private TMP_Text _countText;
    [SerializeField] private TMP_Text _priceText;
    [SerializeField] private Image _itemImage;
    [SerializeField] private Image _priceImage;
    [SerializeField] private Button _itemObtainButton;
    [SerializeField] private GameObject _disablePanel;

    [Header("Currency Sprite")]
    [SerializeField] private Sprite _goldSprite;
    [SerializeField] private Sprite _diaSprite;

    [Header("Item Sprite")]
    [SerializeField] private Sprite _heroSprite;
    [SerializeField] private Sprite _magicSprite;


    private void Start()
    {
        _itemObtainButton.onClick.AddListener(() => OnClickBuy());
    }

    public void SetSlot(ShopSlotData slot)
    {
        _slotData = slot;
        _type = slot.Type;

        _itemNameText.text = slot.ItemName;
        _itemImage.sprite = slot.ItemSprite;
        _priceText.text = slot.ItemPrice;
        _priceImage.sprite = slot.PriceSprite;
        _countText.text = slot.Count;

        //Debug.Log($"[SetSlot] {slot.ItemName} / Purchased={slot.IsPurchased}");

        // 일일 상점 아이템 아이콘
        if (slot.Type == ShopType.Daily && int.Parse(slot.ItemId) < 2010001)
        {
            _itemImage.sprite = _heroSprite;
        }
        else if (slot.Type == ShopType.Daily && int.Parse(slot.ItemId) >= 2010001)
        {
            _itemImage.sprite = _magicSprite;
        }

        // 일일 상점 가격 아이콘
        if (slot.Type == ShopType.Daily && slot.IsGold)
        {
            _priceImage.sprite = _goldSprite;
        }
        else if (slot.Type == ShopType.Daily && slot.IsDiamond)
        {
            _priceImage.sprite = _diaSprite;
        }

        // 상점 첫번째 슬롯/인앱결제 다이아 슬롯 가격 텍스트 위치
        if (slot.IsFree|| (slot.Type == ShopType.Diamond && !slot.IsFree))
        {
            _priceImage.gameObject.SetActive(false);
            
            RectTransform rect = _priceText.GetComponent<RectTransform>();
            Vector2 pos = rect.anchoredPosition;
            pos.x = 11f;
            rect.anchoredPosition = pos;
        }

        // 구매 횟수
        if (slot.Type == ShopType.Daily)
        {
            // soldout 이미지 setactive true
            _countText.gameObject.SetActive(true);
        }
        else
        {
            _countText.gameObject.SetActive(false);
        }

        // 이미 구매한 경우(일일 상점/골드&다이아 무료) -> 버튼 비활성화 & disablePanel setactive true
        if (slot.IsPurchased)
        {
            _disablePanel.SetActive(true);
            _itemObtainButton.interactable = false;
        }
        else
        {
            _disablePanel.SetActive(false);
            _itemObtainButton.interactable = true;
        }
    }
    public async void OnClickBuy()
    {
        if (_slotData.IsFree && _slotData.IsPurchased) return;

        switch (_type)
        {
            case ShopType.Diamond:
                if (_slotData.IsFree)
                {
                    _slotData.IsPurchased = true;
                    _disablePanel.SetActive(true);
                    Debug.Log($"무료 다이아몬드 {_slotData.Count}개 획득");
                    await Manager.DB.AddDiamondAsync(int.Parse(_slotData.Count));
                }
                else
                {
                    IAPManager.Instance.BuyProduct(_slotData.ItemId);
                    Debug.Log($"다이아몬드 {_slotData.Count}개 구매");
                    //await Manager.DB.AddDiamondAsync(int.Parse(_slotData.Count));
                    //await Manager.DB.AddDiamondAsync(int.Parse(_slotData.Count));
                }
                break;
            case ShopType.Gold:
                if (_slotData.IsFree)
                {
                    _slotData.IsPurchased = true;
                    _disablePanel.SetActive(true);
                    Debug.Log($"무료 골드 {_slotData.Count}개 획득");
                    await Manager.DB.AddGoldAsync(int.Parse(_slotData.Count));
                }
                else
                {
                    Debug.Log($"골드 {_slotData.Count}개 구매");
                    Debug.Log($"다이아 {_slotData.ItemPrice}개 차감");
                    await Manager.DB.SubtractDiamondAsync(int.Parse(_slotData.ItemPrice));
                    await Manager.DB.AddGoldAsync(int.Parse(_slotData.Count));
                }
                break;
            case ShopType.Daily:
                if (_slotData.IsPurchased) return;
                _slotData.IsPurchased = true;
                _disablePanel.SetActive(true);
                Debug.Log($"Daily 구매 : {_slotData.ItemName} / {_slotData.Count}개");
                if (_slotData.IsGold)
                {
                    Debug.Log($"골드 {_slotData.ItemPrice}개 차감");
                    await Manager.DB.SubtractGoldAsync(int.Parse(_slotData.ItemPrice));
                }
                else
                {
                    Debug.Log($"다이아 {_slotData.ItemPrice}개 차감");
                    await Manager.DB.SubtractDiamondAsync(int.Parse(_slotData.ItemPrice));
                }
                break;
        }

        DailyShopManager dailyShopManager = FindObjectOfType<DailyShopManager>();
        if (dailyShopManager != null)
        {
            dailyShopManager.UpdateSlotData(_slotData);
            dailyShopManager.SaveShopData();
        }
    }
}