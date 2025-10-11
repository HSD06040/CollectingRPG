using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemSlot_MagicStone : ItemSlot
{
    [SerializeField] TMP_Text _magicStoneTypeText;
    private MagicStoneData _magicStoneData;

    public void SetMagicStoneItem(MagicStoneData magicStoneData)
    {
        base.SetItem(magicStoneData.Name, magicStoneData.Icon);
        _magicStoneData = magicStoneData;
        _magicStoneTypeText.text = _magicStoneData.Type.GetMagicStonTypeKorean();

        SetGradeImage(magicStoneData.Grade);
    }
}
