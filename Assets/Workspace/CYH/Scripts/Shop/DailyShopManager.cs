using UnityEngine;

public class DailyShopManager : MonoBehaviour
{
    private const int MAX_COUNT = 2;
    private int _dailyFreeCount; // 0 = 무료, 1~2 = 광고, 3이상 = 랜덤 아이템

    public ShopSlotData SetDailyFreeSlot()
    {
        if (_dailyFreeCount == 0)
        {
            return new ShopSlotData { Type = ShopType.Daily, ItemPrice = "무료 보상" };
        }
        else if (_dailyFreeCount <= MAX_COUNT)
        {
            return new ShopSlotData { Type = ShopType.Daily, ItemPrice = $"광고 {_dailyFreeCount}/2" };
        }
        else
        {
            // 일일상품 랜덤 로직
            return RandomSlot(); 
        }
    }

    public void FreeSlotCount()
    {
        _dailyFreeCount++;
    }

    private ShopSlotData RandomSlot()
    {
        return new ShopSlotData { Type = ShopType.Daily, ItemName = $"Test" }; 
    }
}
