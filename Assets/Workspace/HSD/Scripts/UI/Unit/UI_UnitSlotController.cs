using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UI_UnitSlotController : MonoBehaviour
{
    [SerializeField] UnitDragDropSystem _dragDropSystem;
    [SerializeField] UnitController _unitController;
    [SerializeField] Transform _content;
    [SerializeField] GameObject _unitSlotPrefab;
    [SerializeField] int _slotCount;    

    private UI_UnitSlot[] _unitSlots;    
    private Dictionary<string, List<int>> _unitSlotDic = new Dictionary<string, List<int>>(256);

    private void Awake()
    {
        CreateSlots();
        UI_UnitSlot.OnUnitChanged += SetSlot;
    }

    private void CreateSlots()
    {
        _unitSlots = new UI_UnitSlot[_slotCount];

        for (int i = 0; i < _slotCount; i++)
        {
            UI_UnitSlot slot = Instantiate(_unitSlotPrefab, _content).GetComponent<UI_UnitSlot>();
            slot.ClearSlot();
            slot.Init(_dragDropSystem, i, this);
            _unitSlots[i] = slot;
        }
    }

    public void SetSlot(UnitData unit, int idx)
    {
        _unitSlots[idx].SetSlot(unit);

        if (!_unitSlotDic.ContainsKey(unit.Address))
        {
            _unitSlotDic.Add(unit.Address, new List<int>(5));
        }
        var slotList = _unitSlotDic[unit.Address];
        if (!slotList.Contains(idx))
        {
            slotList.Add(idx);
        }
    }

    public void ClearSlot(int idx)
    {
        UnitData unit = _unitSlots[idx].GetUnit();
        if (unit == null) return;

        _unitSlotDic[unit.Address].Remove(idx);

        _unitSlots[idx].ClearSlot();
    }

    public int GetEmptySlot()
    {        
        for (int i = 0; i < _unitSlots.Length; i++)
        {
            if (_unitSlots[i].IsEmpty())
            {
                return i;
            }
        }
        return -1;
    }

    public void RemoveUnit(UnitData unit, int idx)
    {
        if (_unitSlotDic.TryGetValue(unit.Address, out var slotList))
        {
            slotList.Remove(idx);
        }
    }

    public void RemoveLastUnit(UnitData unit)
    {
        var slotList = _unitSlotDic[unit.Address];
        int lastIdx = slotList[slotList.Count - 1];
        Debug.Log(lastIdx);
        ClearSlot(lastIdx);
    }

    public void RemoveInGameSlot(UnitBase unit, int slotIdx)
    {
        if(unit.CurrentSlot != Vector2Int.zero)
        {
            UnitSlot slot = _unitController.GetUnitSlot(unit);
            _unitController.RemoveUnit(slot);
        }        

        RemoveUnit(unit.StatusController.Data, slotIdx);
    }

    public int GetUnitCount(string address)
    {
        if (_unitSlotDic.TryGetValue(address, out List<int> slotIdxs))
        {
            return slotIdxs.Count;
        }
        return 0;
    }

    public int GetUnitCount(UnitData unit)
    {
        Debug.Log(unit.Address);
        return GetUnitCount(unit.Address);
    }
}
