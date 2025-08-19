using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] UnitBase[,] _unitBasess;
    [SerializeField] UnitBase[] _unitBases;
    [SerializeField] UnitSlotManager _slotManager;

    private void Awake()
    {
        _slotManager.Init();
    }

    public void SetUnit()
    {

    }

    public void EnemyFight()
    {
        foreach (var unit in _unitBases)
        {
            if (unit == null)
                continue;

            unit.Fight();
        }
    }
}
