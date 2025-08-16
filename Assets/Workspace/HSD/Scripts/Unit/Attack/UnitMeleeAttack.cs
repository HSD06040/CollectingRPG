using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MeleeAttack", menuName = "Data/Unit/Attack/Melee")]
public class UnitMeleeAttack : UnitAttackData
{
    public override void Attack(IAttacker attacker)
    {
        UnitData data = attacker.GetUnitData();
        Transform transform = attacker.GetTransform();

        foreach (GameObject obj in Utils.GetTargetsNonAlloc(
            (Vector2)transform.position + (Offset * transform.GetFacingDir()), SearchType.Circle, 
            SizeOrRadius, BoxSize, Angle, data.AttackCount.Value, attacker.TargetLayer
            ))
        {
            Debug.Log(obj.name);
            obj.GetComponent<IDamageable>().TakeDamage(Utils.CalculateBaseDamage(data, AttackPower, DamageType), DamageType);
        }

        attacker.GetStatusController().CurMana.Value += data.ManaGain.Value;

        Debug.Log($"마나 : {attacker.GetStatusController().CurMana.Value}");
    }
}
