using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SlotCreater : MonoBehaviour
{
    [SerializeField] GameObject _slotPrefab;
    [SerializeField] Transform _slotParent;
    [SerializeField] Vector2 _offset;    
    public Vector2Int Size;    

    public Dictionary<Vector2Int, UnitSlot> Init()
    {
        Dictionary<Vector2Int, UnitSlot> unitSlotDic = new Dictionary<Vector2Int, UnitSlot>();

        for (int i = 0; i < Size.y; i++)
        {
            for (int j = 0; j < Size.x; j++)
            {
                UnitSlot slot = Instantiate(_slotPrefab, GetPos(j, i), Quaternion.identity, _slotParent).GetComponent<UnitSlot>();
                Vector2Int pos = new Vector2Int(j + 1, i + 1);
                slot.Init(j + 1, pos);
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
