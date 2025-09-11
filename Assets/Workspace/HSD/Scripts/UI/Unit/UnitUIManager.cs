using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitUIManager : MonoBehaviour
{           
    [SerializeField] Button _fightButton;
    [SerializeField] GameObject _battleUI;
    [SerializeField] GameObject _notBattleUI;

    [Header("Battle")]
    public FightUnitSlotController FightSlotController;
    public DamageMeterController DamageMeterController;
    public HpMeterController HpMeterController;
    public SkillPopUpController SkillPopUpController;
    public UnitHealthBarManager UnitHealthBarManager;

    [Header("Not Battle")]
    public SynergyPanel SynergyPanel;
    public SynergySlotPanel SynergySlotPanel;
    public UnitCountPanel UnitCountPanel;
    public UnitTotalPowerPanel[] UnitTotalPowerPanel;

    public void BattleUISetting()
    {
        _battleUI.SetActive(true);
        _notBattleUI.SetActive(false);
        _fightButton.gameObject.SetActive(false);
    }

    public void StandbyUISetting()
    {
        _battleUI.SetActive(false);
        _notBattleUI.SetActive(true);
        _fightButton.gameObject.SetActive(true);
    }
}
