using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemSlot_MagicStone : ItemSlot
{
    [SerializeField] TMP_Text _magicStoneTypeText;
    private MagicStoneData _magicStoneData;

    public void SetMagicStoneItem(string itemName, Sprite itemIcon, MagicStoneData magicStoneData)
    {
        base.SetItem(itemName, itemIcon);
        _magicStoneData = magicStoneData;
        _magicStoneTypeText.text = _magicStoneData.Type.GetMagicStonTypeKorean();
    }
}
