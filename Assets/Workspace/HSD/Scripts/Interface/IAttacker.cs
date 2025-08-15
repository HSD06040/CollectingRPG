using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttacker 
{
    public float AttackPower { get; set; }
    public LayerMask TargetLayer { get; set; }
    public Transform GetTransform();
    public Transform GetTarget();
    public UnitData GetUnitData();
}
