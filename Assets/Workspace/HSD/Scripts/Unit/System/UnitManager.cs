using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    [SerializeField] UnitUIManager _unitUIManager;
    [SerializeField] UI_UnitSlotController _unitSlotController;
    [SerializeField] UnitController _unitController;
    [SerializeField] EnemyController _enemyController;

    [SerializeField] UnitData[] _testDatas;
    [SerializeField] int _upgradeNeedCount = 3;

    public void Fight()
    {
        _unitController.UnitFight();
        _enemyController.EnemyFight();

        _unitUIManager.Init();

        _unitUIManager.FightSlotController.Init(_unitController.GetUnits());
        _unitUIManager.DamageMeterController.Init(_unitController.GetUnits());
        _unitUIManager.HpMeterController.Init(_unitController.GetUnits(), _enemyController.GetUnits());
    }

    public void RandomSpawn()
    {
        UnitData unit = _testDatas[Random.Range(0, _testDatas.Length)];
        UnitStatus unitStatus = new UnitStatus(unit);

        AddUnit(unitStatus);
    }

    public void AddUnit(UnitStatus unit)
    {
        int slotIdx = _unitSlotController.GetEmptySlot();

        if (slotIdx == -1)
        {
            Debug.Log("슬롯이 부족합니다.");
            return;
        }

        SetSlot(unit, slotIdx);
    }

    private void SetSlot(UnitStatus unit, int idx)
    {
        _unitSlotController.SetSlot(unit, idx);

        CheckUpgrade(unit);
    }

    private void CheckUpgrade(UnitStatus unit)
    {
        if (unit.Level == 2)
        {
            Debug.Log($"최종 유닛 {unit.Data.Name} 업그레이드 불가");
            return;
        }

        if (GetUnitCount(unit) >= 3)
        {
            UpgradeUnit(unit);
        }
    }

    private void UpgradeUnit(UnitStatus unit)
    {
        UnitStatus newUnit = new UnitStatus(unit.Data, unit.Level + 1);

        int upgradeNeedCount = _upgradeNeedCount;

        int slotCount = _unitSlotController.GetUnitCount(unit);
        int unitCount = _unitController.GetUnitCount(unit);

        Debug.Log($"SlotCount : {slotCount}, UnitCount : {unitCount}");
        Vector2Int pos = Vector2Int.zero;

        for (int i = 0; i < slotCount; i++)
        {
            upgradeNeedCount--;
            _unitSlotController.RemoveLastUnit(unit);
        }

        for (int i = 0; i < upgradeNeedCount; i++)
        {
            pos = _unitController.RemoveUnit(unit);
        }

        if (pos != Vector2Int.zero && !_unitController.IsUnitMaxCount())
        {
            UnitBase unitBase = Instantiate(newUnit.Data.UnitPrefab).GetComponent<UnitBase>();
            unitBase.Status = newUnit;
            unitBase.Init();

            _unitController.AddUnit(unitBase, pos);            
        }
        else
        {
            _unitSlotController.SetSlot(newUnit, _unitSlotController.GetEmptySlot());
        }
    }

    private int GetUnitCount(UnitStatus unit)
    {
        Debug.Log($"SlotCount : {_unitSlotController.GetUnitCount(unit)}, UnitCount : {_unitController.GetUnitCount(unit)}");
        return _unitController.GetUnitCount(unit) + _unitSlotController.GetUnitCount(unit);
    }
}
