using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_UnitSlot : MonoBehaviour, IDragHandler, IBeginDragHandler
{
    [SerializeField] private UnitStatus _unit;
    [SerializeField] private Image _unitIcon;
    private UI_UnitSlotController _unitSlotController;
    private UnitDragDropSystem _dragDropSystem;
    private UnitStatus _chachedUnit;
    private int _slotIdx;

    public static Action<UnitStatus, int> OnUnitChanged;

    public void Init(UnitDragDropSystem dragDropSystem, int slotIdx, UI_UnitSlotController unitSlotController)
    {
        _dragDropSystem = dragDropSystem;
        _slotIdx = slotIdx;
        _unitSlotController = unitSlotController;
    }

    public void SetSlot(UnitStatus unit)
    {
        _unit = unit;

        UpdateUnitSlot();
    }

    public void UpdateUnitSlot()
    {
        if (_unit != null)
        {
            _unitIcon.sprite = _unit.Data.Icon;
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

    public UnitStatus GetUnit()
    {
        return _unit;
    }

    private void UnitSetting(Collider2D collider, UnitBase unit)
    {        
        if (collider == null)
        {
            SetSlot(_chachedUnit);
            _unitSlotController.AddUnit(unit.Status, _slotIdx);
            _chachedUnit = null;            
            return;
        }
        
        _unitSlotController.RemoveUnit(unit.Status, _slotIdx);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (_unit == null) return;

        _unitSlotController.RemoveUnit(_unit, _slotIdx);
        GameObject unit = Instantiate(_unit.Data.UnitPrefab);

        UnitBase unitBase = unit.GetComponent<UnitBase>();
        unitBase.Status = _unit;
        unitBase.Init();

        _dragDropSystem.SetUnit(unit, UnitSetting, _slotIdx);
        _chachedUnit = _unit;
        ClearSlot();
    }

    public void OnDrag(PointerEventData eventData)
    {
        
    }

    public void OnDrop(PointerEventData eventData)
    {
        UnitBase unitBase = _dragDropSystem.GetCurrentUnitBase();

        if (unitBase == null)
        {
            Debug.Log("No unit to drop");
            return;
        }

        UnitStatus temp = _unit;
        UnitStatus unit = unitBase.Status;
        Vector2Int pos = unitBase.CurrentSlot;

        // ui 에서 생성한 거라면
        if (pos == Vector2Int.zero)
        {
            if(IsEmpty())
            {
                OnUnitChanged?.Invoke(unit, _slotIdx);
            }
            else
            {
                
                OnUnitChanged?.Invoke(unit, _slotIdx);
                _unitSlotController.SetSlot(temp, _dragDropSystem.GetCurrentSlotIdx());
            }
        }
        else
        {
            // 인게임에서 생성한 거라면 (인 게임 Slot -> UI)
           
            _unitSlotController.RemoveInGameSlot(unitBase, _slotIdx, true); // 인게임 슬롯에서 제거

            if (temp != null) // 만약 UI 슬롯이 비어있지 않다면
            {
                Debug.Log("Temp != null");
                // 인게임 해당 슬롯에 추가
                _unitSlotController.AddInGameSlot(temp, _slotIdx, unitBase.CurrentSlot);
                _unitSlotController.RemoveUnit(temp, _slotIdx);
                OnUnitChanged?.Invoke(unit, _slotIdx);
                return;
            }
            
            _unitSlotController.RemoveUnit(unit, _slotIdx);
            OnUnitChanged?.Invoke(unit, _slotIdx);
        }
        
        Destroy(unitBase.gameObject);
    }
}