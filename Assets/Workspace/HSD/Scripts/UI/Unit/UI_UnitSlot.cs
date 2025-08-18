using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_UnitSlot : MonoBehaviour
{
    [SerializeField] private UnitData _unit;
    [SerializeField] private Image _unitIcon;

    public void SetUnit(UnitData unit)
    {
        _unit = unit;

        UpdateUnitSlot();
    }

    public void UpdateUnitSlot()
    {
        if (_unit != null)
        {
            _unitIcon.sprite = _unit.Icon;
        }
        else
        {
            _unitIcon.sprite = null;
        }
    }

    public void ClearSlot()
    {
        SetUnit(null);
    }

    public bool IsEmpty()
    {
        return _unit == null;
    }

    public UnitData GetUnit()
    {
        return _unit;
    }
}