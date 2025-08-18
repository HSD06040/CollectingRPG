using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_UnitSlot : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    [SerializeField] private UnitData _unit;
    [SerializeField] private Image _unitIcon;
    private UI_UnitSlotController _unitSlotController;
    private UnitDragDropSystem _dragDropSystem;
    private UnitData _chachedUnit;
    private int _slotIdx;

    public void Init(UnitDragDropSystem dragDropSystem, int slotIdx, UI_UnitSlotController unitSlotController)
    {
        _dragDropSystem = dragDropSystem;
        _slotIdx = slotIdx;
        _unitSlotController = unitSlotController;
    }

    public void SetSlot(UnitData unit)
    {
        _unit = unit;

        UpdateUnitSlot();
    }

    public void UpdateUnitSlot()
    {
        if (_unit != null)
        {
            _unitIcon.sprite = _unit.Icon;
            _unitIcon.color = Color.white;
        }
        else
        {
            _unitIcon.sprite = null;
            _unitIcon.color = Color.clear;
        }
    }

    public void ClearSlot()
    {
        SetSlot(null);
    }

    public bool IsEmpty()
    {
        return _unit == null;
    }

    public UnitData GetUnit()
    {
        return _unit;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _dragDropSystem.SetUnit(Instantiate(_unit.UnitPrefab), UnitSetting, _slotIdx);
        _chachedUnit = _unit;
        ClearSlot();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        
    }

    public void OnDrag(PointerEventData eventData)
    {
        
    }

    private void UnitSetting(Collider2D collider, UnitBase unit)
    {
        if(collider == null)
        {
            SetSlot(_chachedUnit);
            _chachedUnit = null;
            Debug.Log("Collider is null");
            return;
        }

        _unitSlotController.RemoveUnit(unit.Data, _slotIdx);
    }    
}