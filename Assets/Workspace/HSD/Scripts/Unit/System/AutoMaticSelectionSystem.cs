using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AutoMaticSelectionSystem : MonoBehaviour
{
    [Serializable]
    public class AutoUnitInfo
    {
        public UnitStatus Status;

        public UI_UnitSlot UI_UnitSlot;
        public AutoUnitType AutoUnitType;

        public AutoUnitInfo(AutoUnitType autoUnitType, UnitStatus status = null, UI_UnitSlot unitSlot = null)
        {
            Status = status;
            UI_UnitSlot = unitSlot;
            AutoUnitType = autoUnitType;
        }
    }

    [SerializeField] UnitManager _unitManager;
    private UI_UnitSlot[] _uiUnitSlots => _unitManager.UnitController.UISlotController.GetUnitSlots();
    private UnitBase[] _battleUnits => _unitManager.UnitController.GetUnits();
    private List<AutoUnitInfo> units = new List<AutoUnitInfo>(20);
    private List<UnitStatus> sortedUnits = new List<UnitStatus>(20);

    public void AutoSelectCharacters()
    {
#if UNITY_EDITOR
        TestUtils.TimerStart();
#endif
        units.Clear();
        sortedUnits.Clear();

        foreach (var battleUnit in _battleUnits)
        {
            if (battleUnit != null && battleUnit.Status != null && battleUnit.Status.Data != null)
            {
                units.Add(new AutoUnitInfo(AutoUnitType.Unit, status: battleUnit.Status));
            }
        }

        foreach (var uiUnitSlot in _uiUnitSlots)
        {
            if (uiUnitSlot.GetUnit() != null && uiUnitSlot.GetUnit().Data != null)
            {
                units.Add(new AutoUnitInfo(AutoUnitType.Slot, uiUnitSlot.GetUnit(), uiUnitSlot));
            }
        }

        foreach (var info in units)
        {
            if (info.AutoUnitType == AutoUnitType.Unit)
            {
                sortedUnits.Add(info.Status);
                _unitManager.UnitController.RemoveUnitGetPosition(info.Status);
            }
            else
            {
                sortedUnits.Add(info.UI_UnitSlot.GetUnit());
                _unitManager.UnitController.UISlotController.ClearSlot(info.UI_UnitSlot.GetSlotIdx());
            }
        }

        var battleUnits = units
            .OrderByDescending(u => u.Status.CombatPower)
            .Take(10)
            .Select(u => u.Status)
            .ToList();

        var slotUnits = units
            .OrderByDescending(u => u.Status.CombatPower)
            .Skip(10)
            .Select(u => u.Status)
            .ToList();

        SetBattleUnit(battleUnits);

        foreach (var status in slotUnits)
        {
            _unitManager.UnitController.UISlotController.AddEmptySlotUnit(status);
        }

#if UNITY_EDITOR
        TestUtils.TimerStop();
#endif
    }

    private void SetBattleUnit(List<UnitStatus> units)
    {  
        for (int i = 0; i < units.Count; i++)
        {
            int perferredLine = units[i].Data.PerferredLine;
            UnitSlot slot = _unitManager.UnitController.GetEmptyLineSlot(perferredLine);

            if (slot == null)
            {
                // 먼저 오른쪽(증가 방향)으로 끝까지 검사
                for (int line = perferredLine + 1; line <= 5 && slot == null; line++)
                {
                    slot = _unitManager.UnitController.GetEmptyLineSlot(line);
                }

                // 오른쪽에서도 못 찾으면 왼쪽(감소 방향)으로 검사
                for (int line = perferredLine - 1; line >= 1 && slot == null; line--)
                {
                    slot = _unitManager.UnitController.GetEmptyLineSlot(line);
                }
            }

            // 슬롯을 찾았다면 유닛 배치
            if (slot != null)
            {
                _unitManager.AddBattleUnit(units[i], slot);
            }
        }
    }
}
