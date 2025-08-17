using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSlotManager : MonoBehaviour
{
    [SerializeField] SlotCreater _slotCreater;
    public Dictionary<Vector2Int, UnitSlot> UnitSlotDic = new Dictionary<Vector2Int, UnitSlot>(500);

    public void Init()
    {
        UnitSlotDic = _slotCreater.Init();
        Debug.Log($"유닛 슬롯 생성 완료. 총 {UnitSlotDic.Count}개의 슬롯이 생성되었습니다.");
    }

    public UnitSlot GetUnitSlot(UnitBase unit)
    {
        return UnitSlotDic.TryGetValue(unit.CurrentSlot, out UnitSlot slot) ? slot : null;
    }
}
