using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSlot_Augment : ItemSlot
{
    private AugmentData _augmentData;

    public void SetAugmentItem(string itemName, Sprite itemIcon, AugmentData augmentData)
    {
        base.SetItem(itemName, itemIcon);
        _augmentData = augmentData;
    }    
}
