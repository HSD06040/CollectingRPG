using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UnitStatus
{
    public UnitData Data;
    public string Address => $"{Data.Name}_{Level}";
    public int Level;

    public int CombatPower
    {
        get => 10;  // 계산식이 들어갈 예정
    }

    public UnitStats GetCurrentStat()
    {
        return Data.GetUnitStat(Level);
    }

    public UnitStatus(UnitData data, int level = 0)
    {
        Data = data;
        Level = level;
    }
}
