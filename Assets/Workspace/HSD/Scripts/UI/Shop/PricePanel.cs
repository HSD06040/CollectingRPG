using DG.Tweening.Core.Easing;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Purchasing;
using UnityEngine.UI;

public class PricePanel : MonoBehaviour
{
    [SerializeField] BuyButton[] _buyButtons;

    public void AddListenerButton(int index, UnityAction buyAction, int price)
    {
        _buyButtons[index].Button.onClick.RemoveAllListeners();
        _buyButtons[index].Button.onClick.AddListener(buyAction);
        _buyButtons[index].PriceAmountText.text = Utils.ToAbbreviation(price);
        Show(index);
    }

    public void SetPosition(int index, RectTransform rectTransform)
    {
        RectTransform buttonRect = (RectTransform)_buyButtons[index].Button.transform;

        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(null, _buyButtons[index].transform.position);

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            buttonRect.parent as RectTransform,
            screenPoint,
            null,
            out Vector2 localPoint
        );


        Vector2 anchoredPos = buttonRect.anchoredPosition;
        anchoredPos.x = localPoint.x;
        buttonRect.anchoredPosition = anchoredPos;
    }

    public void Close(int index)
    {
        _buyButtons[index].Button.interactable = false;
        _buyButtons[index].SoldOutImage.gameObject.SetActive(true);
    }

    public void Show(int index)
    {
        _buyButtons[index].Button.interactable = true;
        _buyButtons[index].SoldOutImage.gameObject.SetActive(false);
    }
}
