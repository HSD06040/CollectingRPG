using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PricePanel : MonoBehaviour
{
    [SerializeField] Button[] _buttons;
    private TMP_Text[] _priceAmount;

    private void Awake()
    {
        for (int i = 0; i < _buttons.Length; i++)
        {
            _priceAmount[i] = _buttons[i].GetComponentInChildren<TMP_Text>();
        }
    }

    public void AddListenerButton(int index, UnityAction buyAction, int price)
    {
        _buttons[index].onClick.RemoveAllListeners();
        _buttons[index].onClick.AddListener(buyAction);
        _priceAmount[index].text = Utils.ToAbbreviation(price);
        Show(index);
    }

    public void Close(int index)
    {
        _buttons[index].gameObject.SetActive(false);
    }

    public void Show(int index)
    {
        _buttons[index].gameObject.SetActive(true);
    }
}
