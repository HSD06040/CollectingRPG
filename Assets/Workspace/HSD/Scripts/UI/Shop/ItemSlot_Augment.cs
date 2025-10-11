using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSlot_Augment : ItemSlot, IDragHandler, IEndDragHandler, IBeginDragHandler, IPointerDownHandler, IPointerUpHandler
{
    private AUGData _augmentData;

    public void SetAugmentItem(AUGData augmentData)
    {
        base.SetItem(augmentData.Name, augmentData.Icon);
        _augmentData = augmentData;

        SetGradeImage(augmentData.Grade);
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
        _shopToolTip.Show(_augmentData, pos);
    }

    private void CloseToolTip()
    {
        _shopToolTip.Close();
    }
}
