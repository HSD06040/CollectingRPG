using System.Collections.Generic;
using UnityEngine;

public class UnitController : MonoBehaviour
{
    public SynergyController SynergyController;
    public UI_UnitSlotController _uiSlotController;

    [SerializeField] UnitSlotManager _unitSlotManager;
    [SerializeField] UnitDragDropSystem _unitDragDropSystem;
    [SerializeField] BattleManager _battleManager;

    public static readonly int UnitMaxCount = 10;

    private UnitBase[,] _unitGrid;
    private Dictionary<string, List<UnitBase>> _unitBaseDic = new Dictionary<string, List<UnitBase>>(300);
    private Dictionary<Synergy, List<UnitBase>> _synergyUnitDic = new Dictionary<Synergy, List<UnitBase>>(64);
    private Dictionary<ClassType, List<UnitBase>> _classSynergyUnitDic = new Dictionary<ClassType, List<UnitBase>>(64);

    private int _currentUnitCount = 0;
    public int CurrentUnitCount
    {
        get => _currentUnitCount;
        private set
        {
            if (_currentUnitCount != value)
                _currentUnitCount = value;

            OnUnitCountChanged?.Invoke(_currentUnitCount);
        }
    }

    public event System.Action<int> OnUnitCountChanged;
    public event System.Action<int> OnUnitPowerChanged;
    public event System.Action<UnitBase[]> OnUnitChanged;

    public void Init()
    {
        int rows = _unitSlotManager.SlotCreater.Size.y;
        int cols = _unitSlotManager.SlotCreater.Size.x;
        _unitGrid = new UnitBase[rows, cols];

        _unitSlotManager.Init();
        _unitDragDropSystem.OnUnitDropped += AddUnit;
    }

    public void UnitStanby()
    {
        foreach (var unit in _battleManager.GetUnitGrid())
        {
            if (unit == null)
                continue;

            unit.transform.position = _unitSlotManager.GetUnitSlot(unit.CurrentSlot).transform.position;
            unit.Stanby();
        }

        _unitDragDropSystem.enabled = true;
    }

    public void UnitFight()
    {
        foreach (var unit in _battleManager.GetUnitGrid())
        {
            if (unit == null)
                continue;

            unit.Fight();
        }

        _unitDragDropSystem.enabled = false;
    }

    #region AddUnit
    /// <summary>
    /// 유닛을 슬롯에 추가합니다.
    /// </summary>    
    public void AddUnit(UnitSlot slot, UnitBase unit)
    {
        UnitBase slotUnit = slot.Unit;

        if (IsUnitMaxCount())
        {
            if (unit.CurrentSlot == Vector2Int.zero)
            {
                _uiSlotController.SetSlot(unit.Status, _unitDragDropSystem.GetCurrentSlotIdx());
                Destroy(unit.gameObject);
            }
            else
            {
                SetSlot(_unitSlotManager.GetUnitSlot(unit), unit);
                _battleManager.AddUnit(_unitSlotManager.GetUnitSlot(unit), unit);
            }
            return;
        }

        if (unit.CurrentSlot == Vector2Int.zero)
        {
            if (slotUnit != null)
            {
                // 기존 유닛을 UI 슬롯으로 돌려보내기
                _uiSlotController.SetSlot(slotUnit.Status, _unitDragDropSystem.GetCurrentSlotIdx());

                RemoveUnit(slot, destroyGameObject: false); // 유닛 데이터만 제거 (Destroy 안 함)
                Destroy(slotUnit.gameObject); // UI로 복제했으니 인게임 오브젝트 제거
                _battleManager.RemoveUnit(slot, slotUnit);
            }

            SetSlot(slot, unit);
            AddSynergyUnit(unit);
            AddList(unit);
            CurrentUnitCount++;

            OnUnitPowerChanged?.Invoke(unit.Status.CombatPower);
            _battleManager.AddUnit(slot, unit);
            OnUnitChanged?.Invoke(GetUnits());
        }
        else
        {
            UnitSlot unitSlot = _unitSlotManager.GetUnitSlot(unit);

            // 스왑
            if (slotUnit != null && slotUnit != unit)
            {
                ClearSlot(unitSlot, unit);
                ClearSlot(slot, slotUnit);

                _battleManager.RemoveUnit(unitSlot, unit);
                _battleManager.RemoveUnit(slot, slotUnit);

                SetSlot(slot, unit);
                SetSlot(unitSlot, slotUnit);

                _battleManager.AddUnit(slot, unit);
                _battleManager.AddUnit(unitSlot, slotUnit);
            }
            else
            {
                ClearSlot(unitSlot, unit);

                _battleManager.MoveUnit(unitSlot, slot, unit);
                SetSlot(slot, unit);
            }
        }
    }

    public void AddUnit(UnitBase newUnit, Vector2Int pos)
    {
        UnitSlot slot = _unitSlotManager.GetUnitSlot(pos);

        newUnit.transform.SetParent(slot.transform);
        newUnit.transform.position = slot.transform.position;

        AddUnit(slot, newUnit);
    }

    public void AddUnit(UnitStatus newUnitData, Vector2Int pos)
    {
        UnitSlot slot = _unitSlotManager.GetUnitSlot(pos);
        UnitBase newUnit = Instantiate(newUnitData.Data.UnitPrefab).GetComponent<UnitBase>();
        newUnit.Status = newUnitData;

        newUnit.transform.SetParent(slot.transform);
        newUnit.transform.position = slot.transform.position;

        AddUnit(slot, newUnit);
    }
    #endregion

    #region RemoveUnit
    public void RemoveUnit(UnitBase unit)
    {
        UnitSlot slot = GetUnitSlot(unit);

        RemoveUnit(slot);
    }

    public void RemoveUnit(UnitSlot slot, bool destroyGameObject = true)
    {
        if (slot.Unit == null) return;

        UnitBase unit = slot.Unit;

        _unitGrid[unit.CurrentSlot.y - 1, unit.CurrentSlot.x - 1] = null;

        Synergy synergy = unit.Status.Data.EnhancementData.Synergy;
        ClassType classSynergy = unit.Status.Data.EnhancementData.ClassSynergy;

        SynergyController.RemoveSynergy(synergy, classSynergy);

        if (_synergyUnitDic.TryGetValue(synergy, out var synergyList))
            synergyList.Remove(unit);

        if (_classSynergyUnitDic.TryGetValue(classSynergy, out var classList))
            classList.Remove(unit);

        _unitBaseDic[unit.Status.Address].Remove(unit);

        ClearSlot(slot, unit);

        if (destroyGameObject)
        {
            Destroy(unit.gameObject);
            _battleManager.RemoveUnit(slot, unit);
        }

        CurrentUnitCount--;

        OnUnitPowerChanged?.Invoke(-unit.Status.CombatPower);
        OnUnitChanged?.Invoke(GetUnits());
    }

    public Vector2Int RemoveUnit(UnitStatus unit)
    {
        UnitBase unitBase = _unitBaseDic[unit.Address][0];
        UnitSlot slot = _unitSlotManager.GetUnitSlot(unitBase);

        Vector2Int pos = unitBase.CurrentSlot;
        RemoveUnit(slot);

        return unitBase.CurrentSlot;
    }
    #endregion

    private void AddSynergyUnit(UnitBase unit)
    {
        Synergy synergy = unit.Status.Data.EnhancementData.Synergy;
        ClassType classSynergy = unit.Status.Data.EnhancementData.ClassSynergy;

        SynergyController.AddSynergy(synergy, classSynergy);

        if (!_synergyUnitDic.TryGetValue(synergy, out var synergyList))
        {
            synergyList = new List<UnitBase>(16);
            _synergyUnitDic[synergy] = synergyList;
        }
        synergyList.Add(unit);

        if (!_classSynergyUnitDic.TryGetValue(classSynergy, out var classList))
        {
            classList = new List<UnitBase>(16);
            _classSynergyUnitDic[classSynergy] = classList;
        }

        classList.Add(unit);
    }

    private void SetSlot(UnitSlot slot, UnitBase unit)
    {
        slot.SetUnit(unit);

        _unitGrid[unit.CurrentSlot.y - 1, unit.CurrentSlot.x - 1] = unit;
    }

    private void ClearSlot(UnitSlot slot, UnitBase unit)
    {
        slot.ClearSlot();

        _unitGrid[unit.CurrentSlot.y - 1, unit.CurrentSlot.x - 1] = null;
    }

    private void AddList(UnitBase unit)
    {
        if (!_unitBaseDic.TryGetValue(unit.Status.Address, out var list))
        {
            list = new List<UnitBase>(16);
            _unitBaseDic.Add(unit.Status.Address, list);
        }
        list.Add(unit);
    }



    public UnitSlot GetUnitSlot(UnitBase unit)
    {
        return _unitSlotManager.UnitSlotDic[unit.CurrentSlot];
    }

    public UnitSlot GetUnitSlot(Vector2Int pos)
    {
        return _unitSlotManager.GetUnitSlot(pos);
    }

    public int GetUnitCount(string address)
    {
        return _unitBaseDic.TryGetValue(address, out var unitList) ? unitList.Count : 0;
    }

    public int GetUnitCount(UnitStatus unit)
    {
        return GetUnitCount(unit.Address);
    }
    /// <summary>
    /// GC 할당 없이 현재 유닛들을 반환합니다.
    /// 반환된 배열의 유효한 요소는 처음부터 GetUnitsCount()개까지입니다.
    /// </summary>
    public UnitBase[] GetUnits()
    {
        return _battleManager.GetUnits();
    }

    /// <summary>
    /// GetUnits()로 반환된 배열에서 유효한 유닛의 개수를 반환합니다.
    /// </summary>
    public int GetUnitsCount()
    {
        return _battleManager.GetUnitsCount();
    }

    /// <summary>
    /// 특정 인덱스의 유닛을 반환합니다. (범위 체크 포함)
    /// </summary>
    public UnitBase GetUnit(int index)
    {
        return _battleManager.GetUnit(index);
    }

    public bool IsUnitMaxCount()
    {
        return _currentUnitCount >= UnitMaxCount;
    }

    public UnitSlot GetEmptyLineSlot(int line)
    {        
        for (int y = 1; y <= _unitSlotManager.SlotCreater.Size.y; y++)
        {
            Vector2Int pos = new Vector2Int(line, y);

            if (_unitGrid[y - 1, line - 1] == null)
            {
                return _unitSlotManager.GetUnitSlot(pos);
            }
        }

        return null;
    }
}