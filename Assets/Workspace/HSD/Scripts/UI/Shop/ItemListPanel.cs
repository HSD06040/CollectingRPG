using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class ItemListPanel : MonoBehaviour
{
    [SerializeField] protected Transform _itemContent;
    [SerializeField] protected PricePanel _pricePanel;
    [SerializeField] protected ShopToolTip _shopToolTip;

    public abstract void SettingItmes();
}
