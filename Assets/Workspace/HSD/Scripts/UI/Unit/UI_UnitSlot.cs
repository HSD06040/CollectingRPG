using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_UnitSlot : MonoBehaviour, IDragHandler, IBeginDragHandler
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
        if (_unit == null) return;

        _unitSlotController.RemoveUnit(_unit, _slotIdx);
        GameObject unit = Instantiate(_unit.UnitPrefab);

        UnitBase unitBase = unit.GetComponent<UnitBase>();
        unitBase.Data = _unit;
        unitBase.Init();

        _dragDropSystem.SetUnit(unit, UnitSetting, _slotIdx);
        _chachedUnit = _unit;
        ClearSlot();
    }

    public void OnDrag(PointerEventData eventData)
    {
        
    }

    // 인게임 -> UI에 드랍했을 때
    public void OnDrop(PointerEventData eventData)
    {
        UnitBase unitBase = _dragDropSystem.GetCurrentUnitBase();

        if (unitBase == null)
        {
            Debug.Log("No unit to drop");
            return;
        }

        UnitData unit = unitBase.Data;
        Vector2Int pos = unitBase.CurrentSlot;        

        // ui 에서 생성한 거라면
        if(pos == Vector2Int.zero)
        {
            if(IsEmpty())
            {
                _unitSlotController.ClearSlot(_slotIdx);
                OnUnitChanged?.Invoke(unit, _slotIdx);
            }
            else
            {
                UnitData temp = _unit;
                OnUnitChanged?.Invoke(unit, _slotIdx);
                _unitSlotController.SetSlot(temp, _dragDropSystem.GetCurrentSlotIdx());
            }
        }
        else
        {
            _unitSlotController.RemoveInGameSlot(unitBase, _slotIdx);

            // 현재 UI에 데이터가 있는 지 확인, unitBase가 UI에서 파생된 애가 아닌지 확인
            if (_unit != null && unitBase.CurrentSlot != Vector2Int.zero)
            {
                _unitSlotController.AddInGameSlot(_unit, _slotIdx, unitBase.CurrentSlot);
            }

            OnUnitChanged?.Invoke(unit, _slotIdx);
        }
        
        Destroy(unitBase.gameObject);
    }
}