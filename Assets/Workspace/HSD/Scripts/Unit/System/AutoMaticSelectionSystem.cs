using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AutoMaticSelectionSystem : MonoBehaviour
{
    [SerializeField] UnitManager _unitManager;
    UI_UnitSlot[] _uiUnitSlots => _unitManager._unitController._uiSlotController.GetUnitSlots();
    UnitBase[] _battleUnits => _unitManager._unitController.GetUnits();

    public void AutoSelectCharacters()
    {
        List<UnitStatus> sortedUnits = new List<UnitStatus>(20);

        foreach (var battleUnit in _battleUnits)
        {
            if (battleUnit != null)
            {
                if (_unitManager._unitController._uiSlotController.GetEmptySlot() == -1)
                    break;                

                sortedUnits.Add(battleUnit.Status);
                _unitManager._unitController.RemoveUnit(battleUnit);
            }
        }

        Set(sortedUnits);
        sortedUnits.Clear();

        foreach (var uiUnitSlot in _uiUnitSlots)
        {
            if (uiUnitSlot.GetUnit() != null && uiUnitSlot.GetUnit().Data != null)
            {
                sortedUnits.Add(uiUnitSlot.GetUnit());
                _unitManager._unitController._uiSlotController.ClearSlot(uiUnitSlot.GetSlotIdx());
            }
        }

        Set(sortedUnits);
    }

    private void Set(List<UnitStatus> units)
    {
        units = units
        .Where(u => u.Data != null)
        .OrderByDescending(u => u.CombatPower)
        .ToList();
        
        for (int i = 0; i < Mathf.Min(UnitController.UnitMaxCount, units.Count); i++)
        {
            int preferredLine = units[i].Data.PerferredLine;
            UnitSlot slot = _unitManager._unitController.GetEmptyLineSlot(preferredLine);

            if (slot == null)
            {
                // 먼저 오른쪽(증가 방향)으로 끝까지 검사
                for (int line = preferredLine + 1; line <= 5 && slot == null; line++)
                {
                    slot = _unitManager._unitController.GetEmptyLineSlot(line);
                }

                // 오른쪽에서도 못 찾으면 왼쪽(감소 방향)으로 검사
                for (int line = preferredLine - 1; line >= 1 && slot == null; line--)
                {
                    slot = _unitManager._unitController.GetEmptyLineSlot(line);
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
