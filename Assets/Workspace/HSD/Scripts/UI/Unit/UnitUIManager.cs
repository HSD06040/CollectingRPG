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
    public SynergyPanel SynergyPanel;
    public SynergyPanel SynergySlotPanel;
    public UnitCountPanel UnitCountPanel;
    public UnitTotalPowerPanel[] UnitTotalPowerPanel;

    public void BattleUISetting()
    {
        UIFadeIn(_battleUI);
        BattleUISwitch.Init();
    }

    public void StandbyUISetting()
    {
        UnitHealthBarManager.Clear();

        UIFadeOut(_battleUI);

        _fightButton.gameObject.SetActive(true);

        UIFadeIn(_notBattleUI);
    }

    public void StandbyUIDeActive()
    {
        UIFadeOut(_notBattleUI);

        _fightButton.gameObject.SetActive(false);
    }

    private void UIFadeIn(CanvasGroup canvasGroup)
    {
        canvasGroup.alpha = 0f;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;

        canvasGroup.DOFade(1f, _fadeDuration).SetUpdate(true);;

        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;
    }

    private void UIFadeOut(CanvasGroup canvasGroup)
    {
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;

        canvasGroup.DOFade(0f, _fadeDuration).SetUpdate(true);
    }
}
