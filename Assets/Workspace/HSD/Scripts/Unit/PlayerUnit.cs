using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerUnit : UnitBase
{
    public UnitData UnitData;
    [SerializeField] UnitStatusController<UnitData> _unitStatusController;

    private void Awake()
    {
        UnitData = Data as UnitData;
        _unitStatusController.Init(UnitData);
    }
}
