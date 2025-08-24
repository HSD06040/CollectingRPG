using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitUIManager : MonoBehaviour
{   
    [SerializeField] GameObject _battleUI;
    [SerializeField] Button _fightButton;

    [Header("Battle")]
    public FightUnitSlotController FightSlotController;
    public DamageMeterController DamageMeterController;
    public HpMeterController HpMeterController;

    public void BattleUIInit()
    {
        _battleUI.SetActive(true);
        _fightButton.gameObject.SetActive(false);
    }    
}
