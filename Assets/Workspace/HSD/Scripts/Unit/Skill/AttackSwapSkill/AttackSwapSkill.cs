using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackSwapSkill", menuName = "Data/Unit/Skill/AttackSwapSkill")]
public class AttackSwapSkill : UnitSkill
{
    [SerializeField] string _attackAddress;
    [SerializeField] float _duration;

    public override void Active(IAttacker attacker)
    {
        attacker.GetStatusController().AttackDataChange(Manager.Resources.Get<UnitAttackData>(_attackAddress), _duration);
    }
}
