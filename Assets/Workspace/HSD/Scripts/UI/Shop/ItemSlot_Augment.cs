using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSlot_Augment : ItemSlot
{
    private AUGData _augmentData;

    public void SetAugmentItem(AUGData augmentData)
    {
        base.SetItem(augmentData.Name, augmentData.Icon);
        _augmentData = augmentData;

        SetGradeImage(augmentData.Grade);
    }
}
