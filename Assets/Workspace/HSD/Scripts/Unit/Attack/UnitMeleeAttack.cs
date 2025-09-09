using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MeleeAttack", menuName = "Data/Unit/Attack/Melee")]
public class UnitMeleeAttack : UnitAttackData
{
    public override void Attack(IAttacker attacker)
    {
        base.Attack(attacker);

        UnitStatusController status = attacker.GetStatusController();        
        
        if(attacker.GetTarget() == null)
        {
            Debug.Log("[일반 공격] 타겟이 없습니다.");
            return;
        }

        status.CalculateDamage(
            AttackPower,
            DamageType,
            ComponentProvider.Get<UnitBase>(attacker.GetTarget()?.gameObject).StatusController
            );

        Manager.Resources.Instantiate<GameObject>(
                EffectAddress,
                attacker.GetTarget().gameObject.GetCenter(),
                true
                );

        status.GetMana();
    }
}
