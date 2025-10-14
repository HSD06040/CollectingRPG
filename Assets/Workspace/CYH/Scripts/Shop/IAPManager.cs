using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.Purchasing;

public class IAPManager : Singleton<IAPManager>
{
    // 가격 로드하는 메서드
    public string GetLocalizedPrice(string productId)
    {
        var listener = CodelessIAPStoreListener.Instance;
        if (listener == null)
        {
            Debug.Log($"{productId} : listener == null");
            return "error";
        }

        Product product = listener.GetProduct(productId);
        if (product != null && product.availableToPurchase)
        {
            Debug.Log($"{productId} : localizedPriceString == {product.metadata.localizedPriceString}");
            return product.metadata.localizedPriceString;
        }

        return "error";
    }

    // 상품명 로드하는 메서드
    public string GetLocalizedName(string productId)
    {
        Product product = CodelessIAPStoreListener.Instance.GetProduct(productId);
        return (product != null && product.availableToPurchase) ? product.metadata.localizedTitle : "";
    }

    // 구매 시 호출되는 메서드
    public async void BuyProduct(string productId)
    {
        Debug.Log($"[BuyProduct] 실행");
        CodelessIAPStoreListener listener = CodelessIAPStoreListener.Instance;
        if (listener == null) return;

        Product product = listener.GetProduct(productId);
        if (product != null && product.availableToPurchase)
        {
            listener.InitiatePurchase(productId);

            string numberOnly = new string(GetLocalizedName(productId).Where(char.IsDigit).ToArray());
            int diaAmount = int.Parse(numberOnly);
            Debug.Log($"구매한 상품: {productId} / 지급 다이아: {diaAmount}개");
            await Manager.DB.AddDiamondAsync(diaAmount);
            Debug.Log($"다이아 {diaAmount}개 지급 완료");
        }
        else
        {
            Debug.LogWarning($"IAP 상품 {productId} 구매 x");
        }
    }
}