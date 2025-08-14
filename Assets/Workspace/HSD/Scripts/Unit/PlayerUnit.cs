using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerUnit : UnitBase
{
    [SerializeField] BaseFSM<PlayerUnit> _fsm;
    public UnitData Data;

    private void Start()
    {
        _fsm.Init(this);
    }
}
