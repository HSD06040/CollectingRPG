using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitToolTip : MonoBehaviour
{
    [SerializeField] UnitInfoUI _unitInfoUI;

    public void Show(UnitStatus status)
    {
        _unitInfoUI.Setup(status);
        _unitInfoUI.gameObject.SetActive(true);
    }

    public void Close()
    {
        _unitInfoUI.gameObject.SetActive(false);
    }
}
