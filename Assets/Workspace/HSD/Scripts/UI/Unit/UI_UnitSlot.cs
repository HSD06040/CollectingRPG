using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_UnitSlot : MonoBehaviour, IDragHandler, IBeginDragHandler, IDropHandler
{
    [SerializeField] private UnitData _unit;
    [SerializeField] private Image _unitIcon;
    private UI_UnitSlotController _unitSlotController;
    private UnitDragDropSystem _dragDropSystem;
    private UnitData _chachedUnit;
    private int _slotIdx;

    public static Action<UnitData, int> OnUnitChanged;

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

    private void UnitSetting(Collider2D collider, UnitBase unit)
    {
        if (collider == null)
        {
            SetSlot(_chachedUnit);
            _chachedUnit = null;
            Debug.Log("Collider is null");
            return;
        }

        _unitSlotController.RemoveUnit(unit.Data, _slotIdx);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        GameObject unit = Instantiate(_unit.UnitPrefab);
        ComponentProvider.Get<UnitStatusController>(unit).Data = _unit;
        _dragDropSystem.SetUnit(unit, UnitSetting, _slotIdx);
        _chachedUnit = _unit;
        ClearSlot();
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("드래그 중");
    }

    public void OnDrop(PointerEventData eventData)
    {
        GameObject currentDragUnit = _dragDropSystem.GetCurrentUnit();
        if (currentDragUnit == null)
        {
            Debug.Log("No unit to drop");
            return;
        }

        UnitData unit = ComponentProvider.Get<UnitStatusController>(currentDragUnit).Data;

        if (_unit == null)
        {            
            _unitSlotController.RemoveInGameSlot(currentDragUnit.GetComponent<UnitBase>(), _slotIdx);
            OnUnitChanged?.Invoke(unit, _slotIdx);
            Destroy(currentDragUnit);
        }
    }
}