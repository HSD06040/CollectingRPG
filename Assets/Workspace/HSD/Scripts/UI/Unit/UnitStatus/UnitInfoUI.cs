using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitInfoUI : MonoBehaviour
{
    [SerializeField] Image _unitIcon;
    [SerializeField] TMP_Text _unitNameText;
    [SerializeField] TMP_Text _levelText;
    [SerializeField] TMP_Text _powerText;
    [SerializeField] Image _synergyImage;
    [SerializeField] TMP_Text _synergyNameText;
    [SerializeField] Image _classImage;
    [SerializeField] TMP_Text _classNameText;

    [Header("HP_MP")]
    [SerializeField] Slider _hpSlider;
    [SerializeField] TMP_Text _hpText;
    [SerializeField] Slider _mpSlider;
    [SerializeField] TMP_Text _mpText;

    [Header("Other UI")]
    [SerializeField] UnitSkillUI _unitSkillUI;
    [SerializeField] UnitStatusUI _unitStatusUI;

    public void Setup(UnitStatus status)
    {
        _unitNameText.text = status.Data.Name;
        _unitIcon.sprite = status.Data.Icon;

        _levelText.text = status.Data.UpgradeCount.ToString();
        _powerText.text = status.CombatPower.ToString();

        _synergyImage.sprite = SynergyController.SynergyDB.GetSynergy((int)status.Data.Synergy).Icon;
        _classImage.sprite = SynergyController.SynergyDB.GetSynergy((int)status.Data.ClassSynergy).Icon;

        UnitStats stat = status.GetCurrentStat();

        _hpSlider.maxValue = stat.MaxHealth;
        _hpSlider.value = stat.MaxHealth;
        _hpText.text = $"{stat.MaxHealth}/{stat.MaxHealth}";

        _mpSlider.maxValue = stat.MaxMana;
        _mpSlider.value = stat.MaxMana;
        _mpText.text = $"{stat.MaxMana}/{stat.MaxMana}";

        _unitSkillUI.Setup(status.Data.Skill);
        _unitStatusUI.Setup(stat);
    }
}
