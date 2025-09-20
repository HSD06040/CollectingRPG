using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillAndAttackTest : MonoBehaviour
{
    [SerializeField] UnitBase unit;
    [SerializeField] UnitStatus unitStatus;

    [Header("Skill")]
    [SerializeField] UnitSkill skill;
    [Space]

    [Header("Attack")]
    [SerializeField] UnitAttackData attackData;

    [ContextMenu("UseSkill")]
    public void UseSkill()
    {
        unit.Status = unitStatus;
        skill.Active(unit);
    }

    [ContextMenu("UseAttack")]
    public void UseAttack()
    {
        unit.Status = unitStatus;
        attackData.Attack(unit);
    }

    private void OnDrawGizmos()
    {
        skill.DrawGizmos(unit);               

        if(attackData is SplashAttack splash)
        {
            splash.DrawGizmos(unit);
        }
    }
}
