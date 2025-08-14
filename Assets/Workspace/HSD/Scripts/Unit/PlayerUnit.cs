using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerUnit : UnitBase
{
    public UnitData UnitData { get; private set; }
    

    protected override void Awake()
    {
        base.Awake();

        UnitData = Data as UnitData;        
    }
}
