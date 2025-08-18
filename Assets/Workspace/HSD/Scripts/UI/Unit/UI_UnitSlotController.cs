using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UI_UnitSlotController : MonoBehaviour
{
    [SerializeField] UnitDragDropSystem _dragDropSystem;
    [SerializeField] Transform _content;
    [SerializeField] GameObject _unitSlotPrefab;
    [SerializeField] int _slotCount;
    [SerializeField] UnitData[] _testDatas;

    private UI_UnitSlot[] _unitSlots;
    private Dictionary<string, int> _unitCountDic = new Dictionary<string, int>(256);
    private Dictionary<string, List<int>> _unitSlotDic = new Dictionary<string, List<int>>(256);

    private void Awake()
    {
        CreateSlots();
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

    public void RandomSpawn()
    {
        UnitData unit = _testDatas[Random.Range(0, _testDatas.Length)];

        AddUnit(unit);
    }

    public void AddUnit(UnitData unit)
    {
        int slotIdx = GetEmptySlot();

        if (slotIdx == -1)
        {
            Debug.Log("슬롯이 부족합니다.");
            return;
        }

        SetSlot(unit, slotIdx);
    }

    private void SetSlot(UnitData unit, int idx)
    {
        _unitSlots[idx].SetSlot(unit);

        if(!_unitSlotDic.ContainsKey(unit.Address))
        {
            _unitSlotDic.Add(unit.Address, new List<int>(5));
        }
        _unitSlotDic[unit.Address].Add(idx);

        if (!_unitCountDic.TryGetValue(unit.Address, out int value))
        {
            _unitCountDic.Add(unit.Address, 0);
        }        
        _unitCountDic[unit.Address]++;
        
        CheckUpgrade(unit);
    }

    public void ClearSlot(int idx)
    {
        UnitData unit = _unitSlots[idx].GetUnit();

        _unitCountDic[unit.Address]--;
        _unitSlotDic[unit.Address].Remove(idx);

        _unitSlots[idx].ClearSlot();       
    }

    private void CheckUpgrade(UnitData unit)
    {
        if(unit.Level == 3)
        {
            Debug.Log($"최종 유닛 {unit.Name} 업그레이드 불가");
            return;
        }

        if(_unitCountDic[unit.Address] >= 3)
        {
            Upgrade(unit);
        }
    }

    private void Upgrade(UnitData unit)
    {
        UnitData newUnit = Resources.Load<UnitData>($"UnitDatas/{unit.Name}_{unit.Level + 1}");

        _unitSlotDic.TryGetValue(unit.Address, out List<int> slotIdxs);

        foreach (var idx in slotIdxs)
        {
            ClearSlot(idx);
        }

        AddUnit(newUnit);
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
        _unitCountDic[unit.Address]--;
        _unitSlotDic[unit.Address].Remove(idx);
    }
}
