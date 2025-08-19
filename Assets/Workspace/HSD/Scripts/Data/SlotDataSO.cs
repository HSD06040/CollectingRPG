using UnityEngine;
using System.Collections.Generic;
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

[CreateAssetMenu(fileName = "UnitGridData", menuName = "Data/UnitGridData")]
public class UnitGridDataSO : ScriptableObject
{
    public string gridName;
    public List<GridUnitData> unitDatas = new List<GridUnitData>();

    public UnitData GetUnitData(Vector2Int position)
    {
        var unitInfo = unitDatas.Find(u => u.position == position);
        return unitInfo?.unitData;
    }

    public void SetUnitData(Vector2Int position, UnitData unitData)
    {
        var existingUnit = unitDatas.Find(u => u.position == position);
        if (existingUnit != null)
        {
            existingUnit.unitData = unitData;
        }
        else
        {
            unitDatas.Add(new GridUnitData(position, unitData));
        }
    }

    public void RemoveUnitData(Vector2Int position)
    {
        unitDatas.RemoveAll(u => u.position == position);
    }

    public void ClearAllUnitDatas()
    {
        unitDatas.Clear();
    }
}