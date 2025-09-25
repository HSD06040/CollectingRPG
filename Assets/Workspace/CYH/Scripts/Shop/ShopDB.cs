using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using UnityEngine;
using Firebase.Database;

public class ShopDB
{
    private DatabaseReference UserShopRef()
    {
        string uid = FirebaseManager.Auth.CurrentUser.UserId;
        return FirebaseManager.DataReference
            .Child("UserData").Child(uid).Child("ShopData");
    }

    // 변환 메서드
    #region Convert

    /// <summary>
    /// ShopSlotData 리스트 -> ShopSlotDTO 리스트로 변환
    /// (Firebase 업로드용)
    /// </summary>
    private List<ShopSlotDTO> ToDTOList(List<ShopSlotData> slots)
    {
        return slots.Select(slot => new ShopSlotDTO(slot)).ToList();
    }

    /// <summary>
    /// ShopSlotDTO 리스트 -> ShopSlotData 리스트로 변환
    /// (Firebase 다운로드 후 인게임 적용용)
    /// </summary>
    private List<ShopSlotData> ToDataList(List<ShopSlotDTO> dto)
    {
        return dto.Select(dto => new ShopSlotData
        {
            Type = dto.Type,
            ItemId = dto.ItemId,
            ItemPrice = dto.ItemPrice,
            ItemName = dto.ItemName,
            Count = dto.Count,
            UsageCount = dto.UsageCount,
            IsGold = dto.IsGold,
            IsDiamond = dto.IsDiamond,
            IsPurchased = dto.IsPurchased,
            IsFree = dto.IsFree,

            ItemSprite = LoadItemSprite(dto.ItemId),
            PriceSprite = LoadPriceSprite(dto.IsGold, dto.IsDiamond)
        }).ToList();
    }

    #endregion


    // Save
    #region Save

    public async void SaveUserShopDataAsync(
        int rerollCount, int adRerollCount,
        List<ShopSlotData> dailyList,
        List<ShopSlotData> goldList,
        List<ShopSlotData> diamondList)
    {
        UserShopData shopData = new UserShopData(
            rerollCount,
            adRerollCount,
            ToDTOList(dailyList),
            ToDTOList(goldList),
            ToDTOList(diamondList)
        );

        string json = JsonUtility.ToJson(shopData);
        await UserShopRef().SetRawJsonValueAsync(json);

        Debug.Log("SaveUserShopDataAsync 완료");
    }

    #endregion


    // Load
    #region Load

    public async Task<UserShopData> LoadUserShopDataAsync()
    {
        DataSnapshot snapshot = await UserShopRef().GetValueAsync();

        if (!snapshot.Exists)
        {
            Debug.LogWarning("ShopData x / default 생성");

            UserShopData defaultData = new UserShopData(0, 0,
                new List<ShopSlotDTO>(),
                new List<ShopSlotDTO>(),
                new List<ShopSlotDTO>());

            string json = JsonUtility.ToJson(defaultData);
            await UserShopRef().SetRawJsonValueAsync(json);

            return defaultData;
        }

        string jsonData = snapshot.GetRawJsonValue();
        UserShopData data = JsonUtility.FromJson<UserShopData>(jsonData);
        return data;
    }

    /// <summary>
    /// UserShopData -> 실제 게임 데이터 적용용 변환
    /// </summary>
    public void ApplyUserShopData(UserShopData userData,
        out List<ShopSlotData> dailyList,
        out List<ShopSlotData> goldList,
        out List<ShopSlotData> diamondList,
        out int rerollCount,
        out int adRerollCount)
    {
        dailyList = ToDataList(userData.DailyList);
        goldList = ToDataList(userData.GoldList);
        diamondList = ToDataList(userData.DiamondList);

        rerollCount = userData.RerollCount;
        adRerollCount = userData.AdRerollCount;
    }

    // Sprite 매핑
    private Sprite LoadItemSprite(string itemId)
    {
        // TODO: [CYH] CSV itemId 기준으로 sprite 로드
        return null;
    }

    private Sprite LoadPriceSprite(bool isGold, bool isDiamond)
    {
        // TODO: [CYH] currency 종류에 따라 맞는 sprite 반환
        return null;
    }

    #endregion
}