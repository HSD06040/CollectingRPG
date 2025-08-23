using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.PlayerSettings;
using static UnityEngine.UI.CanvasScaler;

public class BattleManager : MonoBehaviour
{
    [SerializeField] UnitSlotManager _unitSlotManager;
    private UnitBase[,] _unitGrid;

    private void Awake()
    {
        int rows = _unitSlotManager.SlotCreater.Size.y;
        int cols = _unitSlotManager.SlotCreater.Size.x;
        _unitGrid = new UnitBase[rows, cols];

        _unitSlotManager.Init();
    }

    public void MoveUnit(UnitSlot oldSlot, UnitSlot slot, UnitBase unit)
    {
        UnitSlot newOldSlot = _unitSlotManager.GetUnitSlot(oldSlot.GetPos());
        UnitSlot newSlot = _unitSlotManager.GetUnitSlot(slot.GetPos());
        UnitBase unitBase = _unitGrid[unit.CurrentSlot.y - 1, unit.CurrentSlot.x - 1];

        ClearSlot(newOldSlot, unitBase, false);

        SetSlot(newSlot, unitBase);
    }

    public void AddUnit(UnitSlot slot, UnitBase unit)
    {
        UnitSlot newSlot = _unitSlotManager.GetUnitSlot(slot.GetPos());
        UnitBase newUnit = Instantiate(unit, slot.transform);

        SetSlot(newSlot, newUnit);
    }

    public void RemoveUnit(UnitSlot slot, UnitBase unit)
    {
        UnitSlot newSlot = _unitSlotManager.GetUnitSlot(slot.GetPos());
        UnitBase unitBase = _unitGrid[unit.CurrentSlot.y - 1, unit.CurrentSlot.x - 1];

        ClearSlot(newSlot, unitBase);
    }

    public void ClearSlot(UnitSlot slot, UnitBase unit, bool isDestroy = true)
    {
        slot.ClearSlot();

        _unitGrid[unit.CurrentSlot.y - 1, unit.CurrentSlot.x - 1] = null;

        if (isDestroy)
            Destroy(unit.gameObject);
    }

    public void SetSlot(UnitSlot slot, UnitBase unit)
    {
        slot.SetUnit(unit);

        _unitGrid[unit.CurrentSlot.y - 1, unit.CurrentSlot.x - 1] = unit;
    }

    public UnitBase[,] GetUnitGrid()
    {
        return _unitGrid;
    }
}