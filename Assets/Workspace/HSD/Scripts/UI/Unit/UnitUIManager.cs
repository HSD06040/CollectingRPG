using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitUIManager : MonoBehaviour
{    
    [SerializeField] GameObject _stanbyUI;
    [SerializeField] GameObject _battleUI;

    [Header("Battle")]
    public FightUnitSlotController FightSlotController;
    public DamageMeterController DamageMeterController;
    public HpMeterController HpMeterController;

    public void BattleUIInit()
    {
        _stanbyUI.SetActive(false);
        _battleUI.SetActive(true);
        HpMeterController.gameObject.SetActive(true);
        FightSlotController.gameObject.SetActive(true);
        DamageMeterController.gameObject.SetActive(true);
    }    
}
