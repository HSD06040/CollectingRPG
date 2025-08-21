using System.Collections.Generic;
using UnityEngine;

public class UnitController : MonoBehaviour
{
    [SerializeField] SynergyController _synergyController;
    [SerializeField] UnitSlotManager _unitSlotManager;
    [SerializeField] UI_UnitSlotController _uiSlotController;
    [SerializeField] UnitDragDropSystem _unitDragDropSystem;
    public static readonly int UnitMaxCount = 10;

    private UnitBase[,] _unitGrid;
    private Dictionary<string, List<UnitBase>> _unitBaseDic = new Dictionary<string, List<UnitBase>>(300); 
    private Dictionary<Synergy, List<UnitBase>> _synergyUnitDic = new Dictionary<Synergy, List<UnitBase>>(64);
    private Dictionary<ClassSynergy, List<UnitBase>> _classSynergyUnitDic = new Dictionary<ClassSynergy, List<UnitBase>>(64);
    private int _currentUnitCount = 0;

    private void Awake()
    {
        int rows = _unitSlotManager.SlotCreater.Size.y;
        int cols = _unitSlotManager.SlotCreater.Size.x;
        _unitGrid = new UnitBase[rows, cols];

        _unitSlotManager.Init();
        _unitDragDropSystem.OnUnitDropped += AddUnit;
    }

    public void UnitStanby()
    {
        foreach (var unit in _unitGrid)
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
        foreach (var unit in _unitGrid)
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
            if(unit.CurrentSlot == Vector2Int.zero)
            {
                _uiSlotController.SetSlot(unit.Data, _unitDragDropSystem.GetCurrentSlotIdx());
                Destroy(unit.gameObject);
            }
            else
            {
                SetSlot(_unitSlotManager.GetUnitSlot(unit), unit);                
            }
            return;
        }

        if (unit.CurrentSlot == Vector2Int.zero)
        {
            if (slotUnit != null)
            {
                // 기존 유닛을 UI 슬롯으로 돌려보내기
                _uiSlotController.SetSlot(slotUnit.Data, _unitDragDropSystem.GetCurrentSlotIdx());

                RemoveUnit(slot, destroyGameObject: false); // 유닛 데이터만 제거 (Destroy 안 함)
                Destroy(slotUnit.gameObject); // UI로 복제했으니 인게임 오브젝트 제거
            }

            SetSlot(slot, unit);
            AddSynergyUnit(unit);
            AddList(unit);
            _currentUnitCount++;
        }
        else
        {
            UnitSlot unitSlot = _unitSlotManager.GetUnitSlot(unit);

            // 스왑
            if (slotUnit != null)
            {
                ClearSlot(unitSlot, unit);
                ClearSlot(slot, slotUnit);                

                SetSlot(slot, unit);
                SetSlot(unitSlot, slotUnit);             
            }
            else
            {
                ClearSlot(unitSlot, unit);
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

    public void AddUnit(UnitData newUnitData, Vector2Int pos)
    {
        UnitSlot slot = _unitSlotManager.GetUnitSlot(pos);
        UnitBase newUnit = Instantiate(newUnitData.UnitPrefab).GetComponent<UnitBase>();
        newUnit.Data = newUnitData;

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

        Synergy synergy = unit.Data.EnhancementData.Synergy;
        ClassSynergy classSynergy = unit.Data.EnhancementData.ClassSynergy;

        _synergyController.RemoveSynergy(synergy, classSynergy);

        if (_synergyUnitDic.TryGetValue(synergy, out var synergyList))
            synergyList.Remove(unit);

        if (_classSynergyUnitDic.TryGetValue(classSynergy, out var classList))
            classList.Remove(unit);

        _unitBaseDic[unit.Data.Address].Remove(unit);

        ClearSlot(slot, unit);

        if (destroyGameObject)
            Destroy(unit.gameObject);
        
        _currentUnitCount--;
    }

    public Vector2Int RemoveUnit(UnitData unit)
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
        Synergy synergy = unit.Data.EnhancementData.Synergy;
        ClassSynergy classSynergy = unit.Data.EnhancementData.ClassSynergy;

        _synergyController.AddSynergy(synergy, classSynergy);

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

    public void SetSlot(UnitSlot slot, UnitBase unit)
    {
        slot.SetUnit(unit);

        _unitGrid[unit.CurrentSlot.y-1, unit.CurrentSlot.x-1] = unit;
    }

    private void ClearSlot(UnitSlot slot, UnitBase unit)
    {
        slot.ClearSlot();

        _unitGrid[unit.CurrentSlot.y - 1, unit.CurrentSlot.x - 1] = null;
    }

    private void AddList(UnitBase unit)
    {
        if (!_unitBaseDic.TryGetValue(unit.Data.Address, out var list))
        {
            list = new List<UnitBase>(16);
            _unitBaseDic.Add(unit.Data.Address, list);
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

    public int GetUnitCount(UnitData unit)
    {
        return GetUnitCount(unit.Address);
    }

    public UnitBase[] GetUnits()
    {
        List<UnitBase> units = new List<UnitBase>();

        foreach (var unit in _unitGrid)
        {
            if (unit != null)
                units.Add(unit);
        }

        return units.ToArray();
    }

    public bool IsUnitMaxCount()
    {
        return _currentUnitCount >= UnitMaxCount;
    }
}
