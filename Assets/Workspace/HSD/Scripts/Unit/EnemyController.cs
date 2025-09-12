using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] UnitGridDataSO _gridDataSO;
    [SerializeField] UnitSlotManager _slotManager;
    [SerializeField] LayerMask _targetLayer;
    private UnitBase[,] _unitGrid;

    [Header("Parent")]
    public Transform BattleParent;

    public void Init()
    {
        _slotManager.Init();
        _unitGrid = new UnitBase[_slotManager.SlotCreater.Size.y, _slotManager.SlotCreater.Size.x];

        SetUnit();
    }

    public void SetUnit()
    {
        foreach (var unitData in _gridDataSO.unitDatas)
        {
            UnitSlot slot = _slotManager.GetUnitSlot(unitData.position + new Vector2Int(1, 1));

            int x = unitData.position.x;
            int y = unitData.position.y;

            GameObject obj = Instantiate(unitData.unitStatus.Data.UnitPrefab);
            UnitBase unit = ComponentProvider.Get<UnitBase>(obj);

            if(unitData.unitStatus == null)
                Debug.Log($"UnitStatus is null at position {unitData.position}");
            unit.Status = unitData.unitStatus;

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

    public void EnemyMove()
    {
        foreach (var unit in _unitGrid)
        {
            if (unit == null)
                continue;

            unit.Move();
        }
    }

    public void EnemyIdle()
    {
        foreach (var unit in _unitGrid)
        {
            if (unit == null)
                continue;

            unit.Idle();
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
    public void SlotsDeActive()
    {
        foreach (var unit in _unitGrid)
        {
            if (unit == null)
                continue;

            unit.transform.SetParent(BattleParent);
        }

        _slotManager.SlotCreater.DeActiveSlots();
    }

    public void ResetEnemy()
    {
        ClearAllEnemyUnits();
        _slotManager.SlotCreater.ActiveSlots();
    }

    private void ClearAllEnemyUnits()
    {
        foreach (var unit in _unitGrid)
        {
            if (unit != null)
            {
                _slotManager.GetUnitSlot(unit).ClearSlot();
                Destroy(unit.gameObject);
            }
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
