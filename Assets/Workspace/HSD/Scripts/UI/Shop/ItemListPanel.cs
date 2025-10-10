using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class ItemListPanel : MonoBehaviour
{
    [SerializeField] protected Transform _itemContent;
    [SerializeField] protected PricePanel _pricePanel;    

    public abstract void SettingItmes();
}
