using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitUIManager : MonoBehaviour
{   
    [SerializeField] GameObject _battleUI;
    
    [Header("NotBattle")]
    [SerializeField] Button _fightButton;
    [SerializeField] GameObject _notBattleUI;

    [Header("Battle")]
    public FightUnitSlotController FightSlotController;
    public DamageMeterController DamageMeterController;
    public HpMeterController HpMeterController;
    public SkillPopUpController SkillPopUpController;

    public void BattleUIInit()
    {
        _battleUI.SetActive(true);
        _notBattleUI.SetActive(false);
        _fightButton.gameObject.SetActive(false);
    }    
}
