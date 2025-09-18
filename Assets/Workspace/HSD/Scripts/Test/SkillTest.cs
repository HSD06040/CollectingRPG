using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTest : MonoBehaviour
{
    [SerializeField] UnitBase unit;
    [SerializeField] UnitSkill skill;
    [SerializeField] UnitStatus unitStatus;

    [ContextMenu("UseSkill")]
    public void UseSkill()
    {
        unit.Status = unitStatus;
        skill.Active(unit);
    }
}
