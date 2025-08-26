using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitSkillUI : MonoBehaviour
{
    [SerializeField] Image _skillIcon;
    [SerializeField] TMP_Text _skillNameText;
    [SerializeField] TMP_Text _manaText;
    [SerializeField] TMP_Text _skillPowerText;
    [SerializeField] TMP_Text _skillDescriptionText;
    [SerializeField] TMP_Text _skillAttackCountText;

    public void Setup(UnitSkill skill)
    {
        _skillIcon.sprite = skill.Icon;
        _skillNameText.text = skill.SkillName;
        _manaText.text = skill.ManaCost.ToString();
        _skillPowerText.text = skill.Power.ToString();
        _skillDescriptionText.text = skill.Description;
        _skillAttackCountText.text = skill.MaxCount.ToString();
    }
}
