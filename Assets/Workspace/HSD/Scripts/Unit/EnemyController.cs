using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] UnitGridDataSO _gridDataSO;
    [SerializeField] UnitSlotManager _slotManager;
    private UnitBase[,] _unitBases;

    private void Awake()
    {
        _slotManager.Init();
        _unitBases = new UnitBase[_slotManager.SlotCreater.Size.y, _slotManager.SlotCreater.Size.x];
        SetUnit();
    }

    public void SetUnit()
    {
        foreach (var unitDatas in _gridDataSO.unitDatas)
        {
            UnitSlot slot = _slotManager.GetUnitSlot(unitDatas.position + new Vector2Int(1,1));

            int x = unitDatas.position.x;
            int y = unitDatas.position.y;
            
            UnitBase unit = Instantiate(unitDatas.unitData.UnitPrefab).GetComponent<UnitBase>();
            
            unit.transform.position = slot.transform.position;
            unit.transform.SetParent(slot.transform);

            _unitBases[y, x] = unit;
        }        
    }

    public void EnemyFight()
    {
        foreach (var unit in _unitBases)
        {
            if (unit == null)
                continue;

            unit.Fight();
        }
    }
}
