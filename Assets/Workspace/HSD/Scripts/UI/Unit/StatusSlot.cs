using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatusSlot : MonoBehaviour
{
    [SerializeField] TMP_Text _amountText;
    
    public void Setup(float amount)
    {
        _amountText.text = amount.ToString("F1");
    }
}
