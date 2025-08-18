using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] UnitBase[] _unitBases;

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
