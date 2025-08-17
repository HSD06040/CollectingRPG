using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitController : MonoBehaviour
{
    [SerializeField] SynergyController _synergyController;
    [SerializeField] UnitSlotManager _unitSlotManager;

    private void Awake()
    {
        _unitSlotManager.Init();
    }
}
