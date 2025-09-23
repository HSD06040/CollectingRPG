using System;
using UnityEngine;

public enum ShopType
{
    Daily,
    Gold,
    Diamond
}

[Serializable]
public class ShopSlotData
{
    public ShopType Type;
    public string ItemId;
    public string ItemPrice;
    public string ItemName;
    public int Count;
    public Sprite ItemSprite;
    public Sprite PriceSprite;

    // 1번째 슬롯 예외
    public bool IsFree;
    public bool IsDailyFree;
}

[Serializable]
public class ShopSlotMeta
{
    public ShopType Type;       
    public string ItemId;        
    public string Name;          
    public int Price;            
    public Sprite ItemSprite;    
    public Sprite PriceSprite;

    // 1번째 슬롯 예외
    public bool IsFree;   
    public bool IsDailyFree;   
}

public class ShopSlotFactory 
{
    public ShopSlotData FromDaily(string itemId, int count, ShopItemSO db)
    {
        ShopSlotMeta meta = db.GetMeta(itemId);

        if(meta.IsDailyFree)
        {
            return new ShopSlotData
            {
                Type = ShopType.Daily,
                ItemId = itemId,
                ItemName = meta.Name,
                ItemSprite = meta.ItemSprite,
                Count = count,
                PriceSprite = meta.PriceSprite,
                ItemPrice = $"무료 획득",
                IsDailyFree = meta.IsDailyFree
            };
        }
        else
        {
            return new ShopSlotData
            {
                Type = ShopType.Daily,
                ItemId = itemId,
                Count = count,
                ItemName = meta.Name,
                ItemSprite = meta.ItemSprite,
                PriceSprite = meta.PriceSprite,
                ItemPrice = $"{meta.Price}",
                IsDailyFree = meta.IsDailyFree
            };
        }
       
    }

    public ShopSlotData FromGold(string itemId, ShopItemSO db)
    {
        ShopSlotMeta meta = db.GetMeta(itemId);

        if (meta.IsFree)
        {
            return new ShopSlotData
            {
                Type = ShopType.Gold,
                ItemId = itemId,
                ItemName = meta.Name,
                ItemSprite = meta.ItemSprite,
                PriceSprite = meta.PriceSprite,
                ItemPrice = $"무료 획득",
                IsFree = meta.IsFree
            };
        }
        else
        {
            return new ShopSlotData
            {
                Type = ShopType.Gold,
                ItemId = itemId,
                ItemName = meta.Name,
                ItemSprite = meta.ItemSprite,
                PriceSprite = meta.PriceSprite,
                ItemPrice = $"{meta.Price}",
                IsFree = meta.IsFree
            };
        }
    }

    public ShopSlotData FromDiamond(string productId, ShopItemSO db)
    {
        ShopSlotMeta meta = db.GetMeta(productId);

        string localizedPrice = IAPManager.Instance.GetLocalizedPrice(productId);
        string localizedName = IAPManager.Instance.GetLocalizedName(productId);

        if (meta.IsFree)
        {
            return new ShopSlotData
            {
                Type = ShopType.Diamond,
                ItemId = productId,
                ItemName = meta.Name,
                ItemSprite = meta.ItemSprite,
                PriceSprite = meta.PriceSprite,
                ItemPrice = $"무료 획득",
                IsFree = meta.IsFree
            };
        }
        return new ShopSlotData
        {
            Type = ShopType.Diamond,
            ItemId = productId,
            ItemName = string.IsNullOrEmpty(localizedName) ? meta.Name : localizedName,  
            ItemSprite = meta.ItemSprite,
            PriceSprite = meta.PriceSprite,
            ItemPrice = string.IsNullOrEmpty(localizedPrice) ? $"KRW {meta.Price}" : localizedPrice,
            IsFree = meta.IsFree
        };
    }
}
