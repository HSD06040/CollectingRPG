using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MeleeAttack", menuName = "Data/Unit/Attack/Melee")]
public class UnitMeleeAttack : UnitAttackData
{
    public override void Attack(IAttacker attacker)
    {
        UnitStatusController status = attacker.GetStatusController();
        Transform transform = attacker.GetTransform();

        foreach (GameObject obj in Utils.GetTargetsNonAlloc(
            (Vector2)transform.position + (Offset * transform.GetFacingDir()), SearchType.Circle, 
            SizeOrRadius, BoxSize, Angle, status.AttackCount.Value, attacker.TargetLayer
            ))
        {
            Debug.Log(obj.name);
            obj.GetComponent<IDamageable>().TakeDamage(Utils.CalculateBaseDamage(status, AttackPower, DamageType), DamageType);
        }

        status.GetMana();
    }
}
