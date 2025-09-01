using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MeleeAttack", menuName = "Data/Unit/Attack/Melee")]
public class UnitMeleeAttack : UnitAttackData
{
    [Header("Serch")]
    public SearchType SearchType;
    public float SizeOrRadius;
    public float Angle;
    public Vector2 BoxSize;

    public override void Attack(IAttacker attacker)
    {
        base.Attack(attacker);

        UnitStatusController status = attacker.GetStatusController();
        Transform transform = attacker.GetTransform();

        foreach (GameObject obj in Utils.GetTargetsNonAlloc(attacker,
            (Vector2)transform.position + (new Vector2(AttackPointOffset.x, AttackPointOffset.x) * attacker.GetTargetDir()),
            SearchType.Circle, SizeOrRadius, BoxSize, Angle, status.AttackCount.Value, attacker.TargetLayer
            ))
        {
            Debug.Log($"Melee Attack {obj.name}");
            status.CalculateDamage(AttackPower, DamageType, ComponentProvider.Get<UnitStatusController>(obj));
        }

        status.GetMana();
    }
}
