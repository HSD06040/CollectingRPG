using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GradeChancePanel : MonoBehaviour
{
    [SerializeField] private GradeChanceSlot[] _gradeChanceSlots;
    private UnitSpawnChanceData _unitSpawnChanceData;

    public void Init(UnitSpawnChanceData unitSpawnChanceData)
    {
        _unitSpawnChanceData = unitSpawnChanceData;
    }

    public void SetGradeChance(int floor)
    {
        if (floor == -1)
            return;

        for (int i = 0; i < _gradeChanceSlots.Length; i++)
        {
            Grade grade = (Grade)i;

            _gradeChanceSlots[i].SetGradeChance(grade, _unitSpawnChanceData.GetCurrentFloorChance(floor).GetChance(grade));
        }
    }
}
