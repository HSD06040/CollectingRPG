using UnityEngine;

public class UnitController : MonoBehaviour
{
    [SerializeField] SynergyController _synergyController;
    [SerializeField] UnitSlotManager _unitSlotManager;
    [SerializeField] UnitDragDropSystem _unitDragDropSystem;

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
            _synergyController.AddSynergy(unit.Data.EnhancementData.Synergy, unit.Data.EnhancementData.ClassSynergy);
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
        _synergyController.RemoveSynergy(slot.Unit.Data.EnhancementData.Synergy, slot.Unit.Data.EnhancementData.ClassSynergy);
        slot.ClearSlot();        
    }
}
