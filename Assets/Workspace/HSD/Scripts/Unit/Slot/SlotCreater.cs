using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SlotCreater : MonoBehaviour
{
    [SerializeField] GameObject _slotPrefab;
    [SerializeField] Transform _slotParent;
    [SerializeField] Vector2 _size;
    [SerializeField] Vector2 _offset;    

    public Dictionary<Vector2, UnitSlot> Init()
    {
        Dictionary<Vector2, UnitSlot> unitSlotDic = new Dictionary<Vector2, UnitSlot>();

        for (int i = 0; i < _size.y; i++)
        {
            for (int j = 0; j < _size.x; j++)
            {
                UnitSlot slot = Instantiate(_slotPrefab, GetPos(j, i), Quaternion.identity, _slotParent).GetComponent<UnitSlot>();
                Vector2 pos = new Vector2(j, i);
                slot.Init(j, pos);
                unitSlotDic.Add(pos, slot);
            }
        }

        return unitSlotDic;
    }

    private Vector2 GetPos(int x, int y)
    {
        Vector2 pos = _slotParent.position;
        return pos + new Vector2(x * _offset.x, y * _offset.y);
    }
}
