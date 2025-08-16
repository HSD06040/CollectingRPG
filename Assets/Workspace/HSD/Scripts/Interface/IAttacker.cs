using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttacker
{
    public LayerMask TargetLayer { get; set; }
    public UnitStatusController GetStatusController();
    public Transform GetTransform();
    public Transform GetTarget();
    public UnitData GetUnitData();
}
