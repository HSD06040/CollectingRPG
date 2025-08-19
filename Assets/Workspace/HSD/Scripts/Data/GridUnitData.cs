using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GridUnitData
{
    public Vector2Int position;
    public UnitData unitData;

    public GridUnitData(Vector2Int pos, UnitData data)
    {
        position = pos;
        unitData = data;
    }
}