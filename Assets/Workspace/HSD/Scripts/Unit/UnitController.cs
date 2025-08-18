using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UnitController : MonoBehaviour
{
    [SerializeField] SynergyController _synergyController;
    [SerializeField] UnitSlotManager _unitSlotManager;
    [SerializeField] UnitDragDropSystem _unitDragDropSystem;

    private UnitBase[,] _unitGrid;
    private Dictionary<Synergy, List<UnitBase>> _synergyUnitDic = new Dictionary<Synergy, List<UnitBase>>(64);
    private Dictionary<ClassSynergy, List<UnitBase>> _classSynergyUnitDic = new Dictionary<ClassSynergy, List<UnitBase>>(64);

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

    public void AddUnit(UnitSlot slot, UnitBase unit)
    {
        UnitBase slotUnit = slot.Unit;

        if (unit.CurrentSlot == Vector2.zero)
        {
            if (slotUnit != null)
            {
                RemoveUnit(slot);
            }

            SetSlot(slot, unit);
            AddSynergyUnit(unit);
        }
        else
        {
            UnitSlot unitSlot = _unitSlotManager.GetUnitSlot(unit);

            // 스왑
            if (slotUnit != null)
            {                
                unitSlot.ClearSlot();
                slot.ClearSlot();

                SetSlot(unitSlot, unit);
                SetSlot(slot, slotUnit);               
            }
            else
            {     
                // 이동
                unitSlot.ClearSlot();
                SetSlot(slot, unit);                
            }
        }
    }

    public void RemoveUnit(UnitSlot slot)
    {
        if (slot.Unit == null) return;

        UnitBase unit = slot.Unit;

        _unitGrid[unit.CurrentSlot.y-1, unit.CurrentSlot.x-1] = null;

        Synergy synergy = unit.Data.EnhancementData.Synergy;
        ClassSynergy classSynergy = unit.Data.EnhancementData.ClassSynergy;

        _synergyController.RemoveSynergy(synergy, classSynergy);

        if (_synergyUnitDic.TryGetValue(synergy, out var synergyList))
            synergyList.Remove(unit);

        if (_classSynergyUnitDic.TryGetValue(classSynergy, out var classList))
            classList.Remove(unit);

        Destroy(unit.gameObject);

        slot.ClearSlot();        
    }

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

    public UnitSlot GetUnitSlot(UnitBase unit)
    {        
        return _unitSlotManager.UnitSlotDic[unit.CurrentSlot];
    }
}
