using UnityEngine;
using System.Collections.Generic;


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