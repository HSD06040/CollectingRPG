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
    [SerializeField] TMP_Text _skillPhysicalPowerText;
    [SerializeField] TMP_Text _skillMagicPowerText;
    [SerializeField] TMP_Text _skillDescriptionText;

    public void Setup(UnitStatus status)
    {
        UnitSkill skill = status.Data.Skill;

        _skillIcon.sprite = skill.Icon;
        _skillNameText.text = skill.SkillName;
        _manaText.text = skill.ManaCost.ToString();
        _skillPhysicalPowerText.text = skill.PhysicalPower.ToString();
        _skillMagicPowerText.text = skill.AbilityPower.ToString();
        _skillDescriptionText.text = GetDescription(skill, status);
    }

    private string GetDescription(UnitSkill skill, UnitStatus status)
    {
        return skill.Description.Replace("{value}", skill.GetCalculateValueString(status));
    }
}
