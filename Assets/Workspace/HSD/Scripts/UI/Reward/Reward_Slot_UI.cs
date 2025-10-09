using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Reward_Slot_UI : MonoBehaviour
{
    [SerializeField] Image _icon;
    [SerializeField] TMP_Text _amountText;

    public void Setup(Sprite icon, int amount)
    {
        _icon.sprite = icon;
        _amountText.text = Utils.ToAbbreviation(amount);
        gameObject.SetActive(true);
    }
}
