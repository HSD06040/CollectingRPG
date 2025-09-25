using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

        // 상점 첫번째 슬롯 가격 텍스트 위치
        if (slot.IsFree)
        {
            _priceImage.gameObject.SetActive(false);
            RectTransform rectTransform = _priceText.rectTransform;
            Vector2 offset = rectTransform.offsetMin;
            offset.x = 11f;
            rectTransform.offsetMin = offset;
        }

        // 구매횟수
        if (slot.Type == ShopType.Daily)
        {
            // soldout 이미지 setactive true
            _countText.gameObject.SetActive(true);
        }
        else
        {
            _countText.gameObject.SetActive(false);
        }
    }

    public void OnClickBuy()
    {
        if (_slotData.IsFree && _slotData.IsPurchased) return;

        switch (_type)
        {
            case ShopType.Diamond:
                if (_slotData.IsFree)
                {
                    //TODO: [CYH] Firebase 재화 업데이트
                    _slotData.IsPurchased = true;
                    _disablePanel.SetActive(true);
                    Debug.Log($"무료 다이아몬드 {_slotData.Count}개 획득");
                }
                else
                {
                    IAPManager.Instance.BuyProduct(_slotData.ItemId);
                    Debug.Log($"다이아몬드 {_slotData.Count}개 구매");
                }
                break;
            case ShopType.Gold:
                if (_slotData.IsFree)
                {
                    _slotData.IsPurchased = true;
                    _disablePanel.SetActive(true);
                    Debug.Log($"무료 골드 {_slotData.Count}개 획득");
                }
                else
                {
                    //TODO: [CYH] Firebase 재화 업데이트
                    Debug.Log($"골드 {_slotData.Count}개 구매");
                }
                break;
            case ShopType.Daily:
                if (_slotData.IsPurchased) return;
                _slotData.IsPurchased = true;
                _disablePanel.SetActive(true);
                Debug.Log($"Daily 구매 : {_slotData.ItemName} / {_slotData.Count}개");
                break;
        }
    }
}