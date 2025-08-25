using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class UI_UnitSlotController : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] UnitDragDropSystem _dragDropSystem;
    [SerializeField] UnitController _unitController;

    [Header("UI Elements")]
    [SerializeField] Transform _content;
    [SerializeField] GameObject _unitSlotPrefab;
    [SerializeField] GridLayoutGroup _gridLayoutGroup;
    [SerializeField] int _slotCount;    

    private UI_UnitSlot[] _unitSlots;
    private Dictionary<string, List<int>> _unitSlotDic = new Dictionary<string, List<int>>(128);

    private void Awake()
    {
        CreateSlots();
        UI_UnitSlot.OnUnitChanged += SetSlot;
    }

    private void CreateSlots()
    {
        _unitSlots = new UI_UnitSlot[_slotCount];

        Vector2 offset = _gridLayoutGroup.spacing;

        float width = (((RectTransform)_content.transform).rect.width - (offset.x * (_slotCount / 2) * 2)) / (_slotCount / 2);
        float height = (((RectTransform)_content.transform).rect.height - (offset.y * (_slotCount / 5) * 2)) / (_slotCount / 5);

        _gridLayoutGroup.cellSize = new Vector2(width, height);

        for (int i = 0; i < _slotCount; i++)
        {
            UI_UnitSlot slot = Instantiate(_unitSlotPrefab, _content).GetComponent<UI_UnitSlot>();
            slot.ClearSlot();
            slot.Init(_dragDropSystem, i, this);
            _unitSlots[i] = slot;
        }
    }

    public void SetSlot(UnitStatus unit, int idx)
    {
        _unitSlots[idx].SetSlot(unit);
        AddUnit(unit, idx);
    }

    public void ClearSlot(int idx)
    {
        UnitStatus unit = _unitSlots[idx].GetUnit();
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

    public void AddUnit(UnitStatus unit, int idx)
    {
        if (!_unitSlotDic.ContainsKey(unit.Address))
        {
            _unitSlotDic.Add(unit.Address, new List<int>(5));
        }

        var slotList = _unitSlotDic[unit.Address];


        if (!slotList.Contains(idx))
        {
            slotList.Add(idx);
        }

        Debug.Log($"[슬롯] ! 유닛 : {unit.Data.Name}, 레벨 : {unit.Level}, 갯수 : {GetUnitCount(unit)}");
    }

    public void RemoveUnit(UnitStatus unit, int idx)
    {
        if (_unitSlotDic.ContainsKey(unit.Address))
        {
            _unitSlotDic[unit.Address].Remove(idx);
        }        
    }

    public void RemoveLastUnit(UnitStatus unit)
    {
        var slotList = _unitSlotDic[unit.Address];
        int lastIdx = slotList[slotList.Count - 1];
        ClearSlot(lastIdx);
    }

    /// <summary>
    /// 인 게임 슬롯에서 유닛을 제거합니다.
    /// </summary>    
    public void RemoveInGameSlot(UnitBase unit, int slotIdx, bool isSwitch = false)
    {
        if (unit == null || unit.StatusController == null)
        {
            Debug.LogWarning("RemoveInGameSlot called with invalid unit");
            return;
        }

        if (unit.CurrentSlot != Vector2Int.zero)
        {
            UnitSlot slot = _unitController.GetUnitSlot(unit);
            _unitController.RemoveUnit(slot); // 슬롯에 있는 유닛 삭제
        }

        if(!isSwitch)
            RemoveUnit(unit.StatusController.Status, slotIdx);        
    }

    public void ReturnUnitToUI(UnitBase unit)
    {
        int emptySlotIdx = GetEmptySlot();
        if (emptySlotIdx == -1)
        {
            Debug.LogWarning("No empty UI slots available!");
            return;
        }

        SetSlot(unit.Status, emptySlotIdx);
    }

    public void AddInGameSlot(UnitStatus unit, int slotIdx, Vector2Int pos)
    {
        UnitSlot slot = _unitController.GetUnitSlot(pos);
        _unitController.AddUnit(unit, pos);
    }

    public int GetUnitCount(string address)
    {
        if (_unitSlotDic.TryGetValue(address, out List<int> slotIdxs))
        {
            return slotIdxs.Count;
        }
        return 0;
    }

    public int GetUnitCount(UnitStatus unit)
    {
        return GetUnitCount(unit.Address);
    }
}
