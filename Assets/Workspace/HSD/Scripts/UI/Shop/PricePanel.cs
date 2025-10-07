using DG.Tweening.Core.Easing;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PricePanel : MonoBehaviour
{
    [SerializeField] BuyButton[] _buyButtons;

    public void AddListenerButton(int index, UnityAction buyAction, int price)
    {
        _buyButtons[index].Button.onClick.RemoveAllListeners();
        _buyButtons[index].Button.onClick.AddListener(buyAction);
        _buyButtons[index]._priceAmountText.text = Utils.ToAbbreviation(price);
        Show(index);
    }

    public void SetPosition(int index, RectTransform rectTransform)
    {
        Vector3 pos = _buyButtons[index].transform.position;
        pos.x = rectTransform.position.x;
        _buyButtons[index].transform.position = pos;
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
