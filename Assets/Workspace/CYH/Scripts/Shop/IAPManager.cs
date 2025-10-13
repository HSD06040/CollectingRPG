using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.Purchasing;

public class IAPManager : Singleton<IAPManager>
{
    //// 가격 로드하는 메서드
    //public string GetLocalizedPrice(string productId)
    //{
    //    var listener = CodelessIAPStoreListener.Instance;
    //    if (listener == null)
    //    {
    //        Debug.Log($"{productId} : listener == null");
    //        return "error";
    //    }

    //    Product product = listener.GetProduct(productId);
    //    if (product != null && product.availableToPurchase)
    //    {
    //        Debug.Log($"{productId} : localizedPriceString == {product.metadata.localizedPriceString}");
    //        return product.metadata.localizedPriceString;
    //    }

    //    return "error";
    //}

    //// 상품명 로드하는 메서드
    //public string GetLocalizedName(string productId)
    //{
    //    Product product = CodelessIAPStoreListener.Instance.GetProduct(productId);
    //    return (product != null && product.availableToPurchase) ? product.metadata.localizedTitle : "";
    //}

    //// 구매 시 호출되는 메서드
    //public void BuyProduct(string productId)
    //{
    //    CodelessIAPStoreListener listener = CodelessIAPStoreListener.Instance;
    //    if (listener == null) return;

    //    Product product = listener.GetProduct(productId);
    //    if (product != null && product.availableToPurchase)
    //    {
    //        listener.InitiatePurchase(productId);

    //    }
    //    else
    //    {
    //        Debug.LogWarning($"IAP 상품 {productId} 구매 x");
    //    }
    //}



    private StoreController _storeController;
    private bool _isInitialized = false;
    private string chachedProductID;


    private async void Awake()
    {
        await InitializeIAPAsync();
    }

    private async Task InitializeIAPAsync()
    {
        await UnityServices.InitializeAsync();

        _storeController = UnityIAPServices.StoreController();

        _storeController.OnProductsFetched += HandleProductsFetched;
        _storeController.OnProductsFetchFailed += HandleProductsFetchFailed;

        _storeController.OnPurchaseConfirmed += HandlePurchaseConfirmed;
        _storeController.OnPurchaseFailed += HandlePurchaseFailed;




        await _storeController.Connect();
        Debug.Log("IAP Connect 성공");

        ProductCatalog catalog = ProductCatalog.LoadDefaultCatalog();

        List<ProductDefinition> defs = new List<ProductDefinition>();

        foreach (var item in catalog.allProducts)
        {
            defs.Add(new ProductDefinition(
                id: item.id,
                storeSpecificId: item.id,
                type: item.type,
                enabled: true
            ));
        }

        _storeController.FetchProducts(defs);
    }

   

    private void HandleProductsFetched(List<Product> products)
    {
        Debug.Log($"상품 로드 완료: {products.Count}개");
        _isInitialized = true;
    }

    private void HandleProductsFetchFailed(ProductFetchFailed failure)
    {
        Debug.LogError($"상품 로드 실패");
    }


    public string GetLocalizedPrice(string productId)
    {
        if (!_isInitialized)
        {
            Debug.LogWarning("초기화x / 가격 로드x");
            return "loading";
        }

        Product product = _storeController.GetProductById(productId);
        if (product != null)
        {
            Debug.Log($"[{productId}] localizedPriceString = {product.metadata.localizedPriceString}");
            return product.metadata.localizedPriceString;
        }

        return "0";
    }

    public string GetLocalizedName(string productId)
    {
        if (!_isInitialized)
        {
            Debug.LogWarning("초기화 x / 상품 정보 로드x");
        }

        Product product = _storeController.GetProductById(productId);
        if (product != null)
        {
            // return product.metadata.localizedTitle;
            string name = product.metadata.localizedTitle;

            int idx = name.IndexOf('(');
            if (idx > 0)
            {
                name = name.Substring(0, idx).Trim();
            }
            return name;
        }

        return "0";
    }


    public void BuyProduct(string productId)
    {
        chachedProductID = productId;
        if (!_isInitialized)
        {
            Debug.LogWarning("IAP 초기화x");
            return;
        }

        Product product = _storeController.GetProductById(productId);
        if (product != null)
        {
            Debug.Log($"구매 요청: {product.definition.id}");
            _storeController.PurchaseProduct(product);
        }
        else
        {
            Debug.LogError($"상품 ID x: {productId}");
        }
    }

    private async void HandlePurchaseConfirmed(Order order)
    {
        Debug.Log($"구매 완료: OrderInfo = {order.Info}");
       
        if (string.IsNullOrEmpty(chachedProductID))
        {
            Debug.LogWarning("캐시된 상품 ID 없음 / 지급 실패");
            return;
        }

        string numberOnly = new string(chachedProductID.Where(char.IsDigit).ToArray());
        int diaAmount = int.Parse(numberOnly);
        Debug.Log($"구매한 상품: {chachedProductID} / 지급 다이아: {diaAmount}개");
        await Manager.DB.AddDiamondAsync(diaAmount);
        Debug.Log($"다이아 {diaAmount}개 지급 완료");
        // await Manager.DB.AddDiamondAsync(100);
        chachedProductID = "";
    }

    private void HandlePurchaseFailed(FailedOrder order)
    {
        Debug.LogError($"구매한 상품: {chachedProductID} / 구매 실패: {order.FailureReason}");
    }
}