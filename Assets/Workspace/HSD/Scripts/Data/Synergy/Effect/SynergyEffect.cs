using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(fileName = "SynergyEffect", menuName = "Data/Synergy/Effect")]
public class SynergyEffect : ScriptableObject
{
    [Header("MetaData")]
    public string Key = Guid.NewGuid().ToString();
    [TextArea]
    public string Description;

    [Header("AttackType")]
    public EffectAttackType EffectAttackType;
    public GameObject Prefab;
    public AssetReference ObjRef;
    public string Address;

    [Header("EffectType")]
    public EffectType EffectType; // Increase
    public SynergyBuffData[] SynergyBuffDatas;
    public SynergyStatModifier[] StatModifiers;

    [Header("TriggerType")]
    public TriggerType TriggerType;
    public float Interval;
    public uint MaxActivations;

    [Header("TargetType")]
    public EffectTargetType TargetType;

    private UnitBase[] _currentUnits;

    public void ApplyEffect(UnitBase[] units, int synergy)
    {
        _currentUnits = units;

        if (TargetType == EffectTargetType.Cross)
        {
            ActiveCross(units, synergy);
            return;
        }

        foreach (var unit in GetTarget(units, synergy))
        {
            unit.StatusController.PassiveController.AddPassiveEffect(this);
        }
    }

    public void RemoveEffect(UnitBase[] units, int synergy)
    {
        foreach (var unit in GetTarget(units, synergy))
        {
            unit.StatusController.PassiveController.RemovePassiveEffect(this);
        }
    }

    private void ActiveCross(UnitBase[] units, int synergy)
    {
        foreach (UnitBase unit in units)
        {
            int synergyCount = 0;

            var currentSlot = unit.CurrentSlot;

            var directions = new Vector2Int[]
            {
                    new Vector2Int(0, 1),
                    new Vector2Int(0, -1),
                    new Vector2Int(-1, 0),
                    new Vector2Int(1, 0)
            };

            foreach (var dir in directions)
            {
                var targetPos = new Vector2Int(currentSlot.x + dir.x, currentSlot.y + dir.y);

                // 해당 위치에 유닛이 있는지 찾기
                var targetUnit = units.FirstOrDefault(u => u.CurrentSlot.x == targetPos.x &&
                                                           u.CurrentSlot.y == targetPos.y);

                if (targetUnit != null)
                {
                    int unitSynergy = (int)targetUnit.Status.Data.EnhancementData.Synergy;
                    int unitClassSynergy = (int)targetUnit.Status.Data.EnhancementData.ClassSynergy;

                    if (unitSynergy == synergy || unitClassSynergy == synergy)
                    {
                        synergyCount++;
                    }
                }
            }

            if (synergyCount == 0)
                return;

            unit.StatusController.PassiveController.AddPassiveEffect(this, synergyCount, true);
        }
    }

    protected UnitBase[] GetTarget(UnitBase[] units, int synergy)
    {
        if (TargetType == EffectTargetType.Ally)
            return units;

        List<UnitBase> unitBases = new List<UnitBase>();

        switch (TargetType)
        {
            case EffectTargetType.SameSynergy:
                foreach (var unit in units)
                {
                    if ((int)unit.Status.Data.EnhancementData.Synergy == synergy ||
                        (int)unit.Status.Data.EnhancementData.ClassSynergy == synergy)
                        unitBases.Add(unit);
                }
                break;
            case EffectTargetType.Column:
                var targetColumns = new HashSet<int>();

                foreach (var unit in units)
                {
                    if ((int)unit.Status.Data.EnhancementData.Synergy == synergy ||
                        (int)unit.Status.Data.EnhancementData.ClassSynergy == synergy)
                    {
                        int column = unit.CurrentSlot.x;
                        targetColumns.Add(column);
                    }
                }

                foreach (var unit in units)
                {
                    int unitColumn = unit.CurrentSlot.x;
                    if (targetColumns.Contains(unitColumn))
                    {
                        unitBases.Add(unit);
                    }
                }
                break;
            case EffectTargetType.Row:
                var targetRows = new HashSet<int>();

                foreach (var unit in units)
                {
                    if ((int)unit.Status.Data.EnhancementData.Synergy == synergy ||
                        (int)unit.Status.Data.EnhancementData.ClassSynergy == synergy)
                    {
                        int Row = unit.CurrentSlot.y;
                        targetRows.Add(Row);
                    }
                }

                foreach (var unit in units)
                {
                    int unitRow = unit.CurrentSlot.y;
                    if (targetRows.Contains(unitRow))
                    {
                        unitBases.Add(unit);
                    }
                }
                break;
            case EffectTargetType.ColumnAndRow:
                var targetCols = new HashSet<int>();
                var targetRowsSet = new HashSet<int>();
                foreach (var unit in units)
                {
                    if ((int)unit.Status.Data.EnhancementData.Synergy == synergy ||
                        (int)unit.Status.Data.EnhancementData.ClassSynergy == synergy)
                    {
                        int column = unit.CurrentSlot.x;
                        int row = unit.CurrentSlot.y;
                        targetCols.Add(column);
                        targetRowsSet.Add(row);
                    }
                }
                foreach (var unit in units)
                {
                    int unitColumn = unit.CurrentSlot.x;
                    int unitRow = unit.CurrentSlot.y;
                    if (targetCols.Contains(unitColumn) || targetRowsSet.Contains(unitRow))
                    {
                        unitBases.Add(unit);
                    }
                }
                break;
        }

        return unitBases.ToArray();
    }
}
