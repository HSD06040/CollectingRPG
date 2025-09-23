using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopSlot : MonoBehaviour
{
    [SerializeField] private ShopType _type;

    [Header("UI Components")]
    [SerializeField] private TMP_Text _itemNameText;
    [SerializeField] private TMP_Text _countText;
    [SerializeField] private TMP_Text _priceText;
    [SerializeField] private Image _itemImage;
    [SerializeField] private Image _priceImage;  

    private ShopSlotData _slotData; 
    
    public void SetSlot(ShopSlotData slot)
    {
        _slotData = slot;
        _type = slot.Type;

        _itemNameText.text = slot.ItemName;
        _itemImage.sprite = slot.ItemSprite;
        _priceText.text = slot.ItemPrice;
        _priceImage.sprite = slot.PriceSprite;
     

        if (slot.Type == ShopType.Diamond || slot.IsFree)
        {
            _priceImage.gameObject.SetActive(false);
            RectTransform rectTransform = _priceText.rectTransform;
            Vector2 offset = rectTransform.offsetMin;
            offset.x = 0f;
            rectTransform.offsetMin = offset;
        }

        if (slot.Type == ShopType.Daily && slot.Count > 0)
        {
            _countText.gameObject.SetActive(true);
            _countText.text = $"{slot.Count}";
        }
        else
        {
            _countText.gameObject.SetActive(false);
        }
    }


    public void OnClickBuy()
    {
        switch (_type)
        {
            case ShopType.Diamond:
                IAPManager.Instance.BuyProduct(_slotData.ItemId);
                break;
            case ShopType.Gold:
                Debug.Log($"Gold 구매 : {_slotData.ItemName}");
                break;
            case ShopType.Daily:
                Debug.Log($"Daily 구매 : {_slotData.ItemName} / {_slotData.Count}개");
                break;
        }
    }
}