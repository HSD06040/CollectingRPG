using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitUIManager : MonoBehaviour
{   
    [SerializeField] GameObject _battleUI;

    [Header("Battle")]
    public FightUnitSlotController FightSlotController;
    public DamageMeterController DamageMeterController;
    public HpMeterController HpMeterController;

    public void BattleUIInit()
    {
        _battleUI.SetActive(true);
    }    
}
