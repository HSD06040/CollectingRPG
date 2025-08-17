using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSlotManager : MonoBehaviour
{
    [SerializeField] SlotCreater _slotCreater;
    public Dictionary<Vector2, UnitSlot> UnitSlotDic = new Dictionary<Vector2, UnitSlot>();

    public void Init()
    {
        UnitSlotDic = _slotCreater.Init();
    }


}
