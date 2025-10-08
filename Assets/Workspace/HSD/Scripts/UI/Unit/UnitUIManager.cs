using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitUIManager : MonoBehaviour
{           
    [SerializeField] Button _fightButton;
    [SerializeField] CanvasGroup _battleUI;
    [SerializeField] CanvasGroup _notBattleUI;

    [SerializeField] private float _fadeDuration = 0.3f;

    [Header("Battle")]
    public DamageMeterController DamageMeterController;
    public HpMeterController HpMeterController;
    public SkillPopUpController SkillPopUpController;
    public UnitHealthBarManager UnitHealthBarManager;
    public BattleUISwitch BattleUISwitch;

    [Header("Not Battle")]
    public GradeChancePanel GradeChancePanel;
    public SynergyPanel SynergyPanel;
    public SynergyPanel SynergySlotPanel;
    public UnitCountPanel UnitCountPanel;
    public UnitTotalPowerPanel[] UnitTotalPowerPanel;

    public void BattleUISetting()
    {
        _battleUI.FadeIn(_fadeDuration).Forget();
        BattleUISwitch.Init();
    }

    public void StandbyUISetting()
    {
        UnitHealthBarManager.Clear();

        _battleUI.FadeOut(_fadeDuration).Forget();        

        _fightButton.gameObject.SetActive(true);

        _notBattleUI.FadeIn(_fadeDuration).Forget();
    }

    public void StandbyUIDeActive()
    {
        _notBattleUI.FadeOut(_fadeDuration).Forget();

        _fightButton.gameObject.SetActive(false);
    }
}
