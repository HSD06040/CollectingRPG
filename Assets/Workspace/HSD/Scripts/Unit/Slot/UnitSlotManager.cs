using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSlotManager : MonoBehaviour
{
    public SlotCreater SlotCreater;
    public Dictionary<Vector2Int, UnitSlot> UnitSlotDic = new Dictionary<Vector2Int, UnitSlot>(500);

    public void Init()
    {
        UnitSlotDic = SlotCreater.Init();
    }

    public UnitSlot GetUnitSlot(UnitBase unit)
    {
        return UnitSlotDic.TryGetValue(unit.CurrentSlot, out UnitSlot slot) ? slot : null;
    }
}
