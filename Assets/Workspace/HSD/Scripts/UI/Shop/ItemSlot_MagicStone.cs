using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSlot_MagicStone : ItemSlot, IDragHandler, IEndDragHandler, IBeginDragHandler, IPointerDownHandler, IPointerUpHandler
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
    public void OnBeginDrag(PointerEventData eventData)
    {
        OpenToolTip(eventData.position);
    }

    public void OnDrag(PointerEventData eventData)
    {
        OpenToolTip(eventData.position);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        CloseToolTip();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OpenToolTip(eventData.position);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        CloseToolTip();
    }

    private void OpenToolTip(Vector2 pos)
    {
        _shopToolTip.Show(_magicStoneData, pos);
    }

    private void CloseToolTip()
    {
        _shopToolTip.Close();
    }
}
