using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitToolTip : MonoBehaviour
{
    [SerializeField] UnitInfoUI _unitInfoUI;
    private bool _isSell;

    public void Show(UnitStatus status, bool isUI, bool isSell, bool isEnemy = false)
    {
        _unitInfoUI.Setup(status, isUI, isSell, isEnemy);
        _isSell = isSell;
    }

    public void Close()
    {
        _unitInfoUI.Close();
    }

    public void ForceClose()
    {
        _unitInfoUI.Close();
    }
}
