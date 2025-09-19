using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

[CreateAssetMenu(fileName = "AUGEffect", menuName = "Data/AUG/AUGEffect")]
public class AUGData : ScriptableObject
{
    public string AUGID;
    public string AUGName;
    [TextArea]
    public string AUGDescription;
    public SubGrade Grade;

    public EffectTargetType TargetType;
    public TriggerType Trigger;
    public EffectType EffectType;
    public EffectTime ApplyTime;

    private int CurrentRate;
    public int[] Rate;

    public void OnEnable()
    {
        ApplyRate();
    }

    public void ApplyRate()
    {
        switch(Grade)
        {
            case SubGrade.SILVER: CurrentRate = Rate[0]; break;
            case SubGrade.GOLD: CurrentRate = Rate[1]; break;
            case SubGrade.PRISM: CurrentRate = Rate[2]; break;
            default : CurrentRate = 0; break;
        }
    }


    public void ApplyEffect(UnitBase[] units)
    {
        if(TargetType == EffectTargetType.Ally)
        {
            units[0].Status.Data.s
        }
        else if(TargetType == EffectTargetType.SameClassType)
        {

        }
        else if(TargetType == EffectTargetType.Leader)
        {

        }
    }

    public void RemoveEffect(UnitBase[] units)
    {
        
    }

    protected UnitBase[] GetTarget(UnitBase[] units, int synergy)
    {
        List<UnitBase> unitBases = new List<UnitBase>();

        if (TargetType == EffectTargetType.Ally)
        {
            foreach (var unit in units)
            {
                if (unit != null)
                    unitBases.Add(unit);
            }
            return unitBases.ToArray();
        }

        switch (TargetType)
        {
            case EffectTargetType.SameSynergy:
                foreach (var unit in units)
                {
                    if (unit == null) continue;

                    if ((int)unit.Status.Data.Synergy == synergy ||
                        (int)unit.Status.Data.ClassSynergy == synergy)
                        unitBases.Add(unit);
                }
                break;
            case EffectTargetType.Column:
                var targetColumns = new HashSet<int>();

                foreach (var unit in units)
                {
                    if (unit == null) continue;

                    if ((int)unit.Status.Data.Synergy == synergy ||
                        (int)unit.Status.Data.ClassSynergy == synergy)
                    {
                        int column = unit.CurrentSlot.x;
                        targetColumns.Add(column);
                    }
                }

                foreach (var unit in units)
                {
                    if (unit == null) continue;

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
                    if (unit == null) continue;

                    if ((int)unit.Status.Data.Synergy == synergy ||
                        (int)unit.Status.Data.ClassSynergy == synergy)
                    {
                        int Row = unit.CurrentSlot.y;
                        targetRows.Add(Row);
                    }
                }

                foreach (var unit in units)
                {
                    if (unit == null) continue;

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
                    if (unit == null) continue;

                    if ((int)unit.Status.Data.Synergy == synergy ||
                        (int)unit.Status.Data.ClassSynergy == synergy)
                    {
                        int column = unit.CurrentSlot.x;
                        int row = unit.CurrentSlot.y;
                        targetCols.Add(column);
                        targetRowsSet.Add(row);
                    }
                }
                foreach (var unit in units)
                {
                    if (unit == null) continue;

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