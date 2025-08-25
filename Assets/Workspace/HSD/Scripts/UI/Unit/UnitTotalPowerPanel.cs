using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UnitTotalPowerPanel : MonoBehaviour
{
    [SerializeField] TMP_Text _powerText;
    private int _totalPower = 0;

    private void Awake()
    {
        UpdateTotalPower(0);
    }

    public void UpdateTotalPower(int power)
    {
        _totalPower += power;

        _powerText.text = _totalPower.ToString("N0");
    }
}
