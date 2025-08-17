using System.Collections.Generic;
using UnityEngine;

public class UnitController : MonoBehaviour
{
    [SerializeField] SynergyController _synergyController;
    [SerializeField] UnitSlotManager _unitSlotManager;
    [SerializeField] UnitDragDropSystem _unitDragDropSystem;

    private Dictionary<Synergy, List<UnitBase>> _synergyUnitDic = new Dictionary<Synergy, List<UnitBase>>(64);
    private Dictionary<ClassSynergy, List<UnitBase>> _classSynergyUnitDic = new Dictionary<ClassSynergy, List<UnitBase>>(64);

    private void Awake()
    {
        _unitSlotManager.Init();
        _unitDragDropSystem.OnUnitDropped += AddUnit;
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

            slot.SetUnit(unit);
            AddSynergyUnit(unit);
        }
        else
        {
            if (slotUnit != null)
            {
                UnitSlot unitSlot = _unitSlotManager.GetUnitSlot(unit);

                unitSlot.ClearSlot();
                slot.ClearSlot();

                unitSlot.SetUnit(unit);
                slot.SetUnit(slotUnit);
            }
            else
            {
                UnitSlot unitSlot = _unitSlotManager.GetUnitSlot(unit);
                unitSlot.ClearSlot();
                slot.SetUnit(unit);
            }
        }
    }

    public void RemoveUnit(UnitSlot slot)
    {
        if (slot.Unit == null) return;

        Destroy(slot.Unit.gameObject);

        Synergy synergy = slot.Unit.Data.EnhancementData.Synergy;
        ClassSynergy classSynergy = slot.Unit.Data.EnhancementData.ClassSynergy;

        _synergyController.RemoveSynergy(synergy, classSynergy);

        _synergyUnitDic[synergy].Remove(slot.Unit);
        _classSynergyUnitDic[classSynergy].Remove(slot.Unit);

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
}
