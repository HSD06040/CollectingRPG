using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class ItemSlot : MonoBehaviour
{
    [SerializeField] TMP_Text _itemNameText;
    [SerializeField] Image _itemIcon;

    public virtual void SetItem(string itemName, Sprite itemIcon)
    {
        _itemNameText.text = itemName;
        _itemIcon.sprite = itemIcon;
    }
}
