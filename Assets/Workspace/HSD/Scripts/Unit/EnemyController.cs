using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] UnitGridDataSO _gridDataSO;
    [SerializeField] UnitSlotManager _slotManager;
    [SerializeField] LayerMask _targetLayer;
    private UnitBase[,] _unitGrid;

    public void Init()
    {
        _slotManager.Init();
        _unitGrid = new UnitBase[_slotManager.SlotCreater.Size.y, _slotManager.SlotCreater.Size.x];

        SetUnit();
    }

    public void SetUnit()
    {
        foreach (var unitDatas in _gridDataSO.unitDatas)
        {
            UnitSlot slot = _slotManager.GetUnitSlot(unitDatas.position + new Vector2Int(1, 1));

            int x = unitDatas.position.x;
            int y = unitDatas.position.y;

            GameObject obj = Instantiate(unitDatas.unitStatus.Data.UnitPrefab);
            UnitBase unit = ComponentProvider.Get<UnitBase>(obj);
            unit.Status = unitDatas.unitStatus;

            unit.transform.position = slot.transform.position;
            unit.transform.SetParent(slot.transform);
            unit.TargetLayer = _targetLayer;
            unit.SetBattleUnit();

            // 레이어 변경 (자식 포함)
            SetLayerRecursively(unit.gameObject, 6);

            if (unit.transform.localScale.x < 0)
                unit.transform.localScale = new Vector3(-unit.transform.localScale.x, unit.transform.localScale.y, unit.transform.localScale.z);

            _unitGrid[y, x] = unit;
        }
    }

    public void EnemyFight()
    {
        foreach (var unit in _unitGrid)
        {
            if (unit == null)
                continue;

            unit.Fight();
        }
    }

    public void EnemyStandby()
    {
        foreach (var unit in _unitGrid)
        {
            if (unit == null || unit.StatusController.IsDead)
                continue;

            unit.Standby();
        }
    }

    private void SetLayerRecursively(GameObject obj, int layer)
    {
        obj.layer = layer;

        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, layer);
        }
    }

    public UnitBase[] GetUnits()
    {
        List<UnitBase> units = new List<UnitBase>();

        foreach (var unit in _unitGrid)
        {
            if (unit != null)
                units.Add(unit);
        }

        return units.ToArray();
    }
}
