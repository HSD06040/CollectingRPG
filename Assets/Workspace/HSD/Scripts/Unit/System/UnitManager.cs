using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    [SerializeField] UnitUIManager _unitUIManager;
    [SerializeField] UI_UnitSlotController _unitSlotController;
    [SerializeField] UnitController _unitController;
    [SerializeField] UnitData[] _testDatas;
    [SerializeField] int _upgradeNeedCount = 3;

    public void Fight()
    {
        _unitController.UnitFight();

        _unitUIManager.Init();

        _unitUIManager.FightSlotController.Init(_unitController.GetUnits());
        _unitUIManager.DamageMeterController.Init(_unitController.GetUnits());
    }

    public void RandomSpawn()
    {
        UnitData unit = _testDatas[Random.Range(0, _testDatas.Length)];

        AddUnit(unit);
    }

    public void AddUnit(UnitData unit)
    {
        int slotIdx = _unitSlotController.GetEmptySlot();

        if (slotIdx == -1)
        {
            Debug.Log("슬롯이 부족합니다.");
            return;
        }

        SetSlot(unit, slotIdx);
    }

    private void SetSlot(UnitData unit, int idx)
    {
        _unitSlotController.SetSlot(unit, idx);

        CheckUpgrade(unit);
    }

    private void CheckUpgrade(UnitData unit)
    {
        if (unit.Level == 3)
        {
            Debug.Log($"최종 유닛 {unit.Name} 업그레이드 불가");
            return;
        }

        if (GetUnitCount(unit) >= 3)
        {
            UpgradeUnit(unit);
        }
    }

    private void UpgradeUnit(UnitData unit)
    {
        string key = $"Data/Unit/{unit.Name}_{unit.Level + 1}";

        UnitData newUnit = Resources.Load<UnitData>(key);

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
            UnitBase unitBase = Instantiate(newUnit.UnitPrefab).GetComponent<UnitBase>();
            unitBase.Data = newUnit;
            unitBase.Init();

            _unitController.AddUnit(unitBase, pos);            
        }
        else
        {
            _unitSlotController.SetSlot(newUnit, _unitSlotController.GetEmptySlot());
        }
    }

    private int GetUnitCount(UnitData unit)
    {
        Debug.Log($"SlotCount : {_unitSlotController.GetUnitCount(unit)}, UnitCount : {_unitController.GetUnitCount(unit)}");
        return _unitController.GetUnitCount(unit) + _unitSlotController.GetUnitCount(unit);
    }
}
