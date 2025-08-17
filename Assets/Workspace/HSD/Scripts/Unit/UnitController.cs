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
        }
        else
        {
             // 슬롯 유닛의 현재 슬롯을 구한다.

            if (slotUnit != null)
            {
                UnitSlot unitSlot = _unitSlotManager.GetUnitSlot(slotUnit);
                unitSlot.ClearSlot();
                slot.ClearSlot();

                unitSlot.SetUnit(unit); // 슬롯 유닛의 현재 슬롯에 새로운 유닛을 배치한다.
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
        slot.ClearSlot();           
    }
}
