using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

[CreateAssetMenu(fileName = "RangedAttack", menuName = "Data/Unit/Attack/Ranged")]
public class UnitRangedAttack : UnitAttackData
{
    [SerializeField] protected GameObject projectilePrefab;
    [SerializeField] protected float projectileSpeed = 10f;

    public override void Attack(IAttacker attacker)
    {
        Vector2 offset = AttackPointOffset;
        offset.x *= attacker.GetTransform().GetFacingDir();

        Vector2 spawnPoint = (Vector2)attacker.GetTransform().position + offset;

        GameObject obj = Instantiate(projectilePrefab, spawnPoint, Quaternion.identity);
        Projectile projectile = ComponentProvider.Get<Projectile>(obj);

        attacker.GetStatusController().GetMana();        

        projectile.Init(attacker.GetTarget(), attacker.GetStatusController(), AttackPower, DamageType, attacker.TargetLayer, projectileSpeed);
    }
}
