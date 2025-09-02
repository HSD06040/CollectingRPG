using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitToolTip : MonoBehaviour
{
    [SerializeField] UnitInfoUI _unitInfoUI;
    private bool _isSell;

    public void Show(UnitStatus status, bool isUI, bool isSell)
    {
        _unitInfoUI.gameObject.SetActive(true);
        _unitInfoUI.Setup(status, isUI, isSell);
        _isSell = isSell;
    }

    public void Close()
    {
        if (_isSell) return;

        _unitInfoUI.gameObject.SetActive(false);
    }
}
