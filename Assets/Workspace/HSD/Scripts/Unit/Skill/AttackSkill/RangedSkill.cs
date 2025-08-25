using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RangedSkill", menuName = "Data/Unit/Skill/Ranged")]
public class RangedSkill : AttackSkill
{
    [SerializeField] GameObject _projectilePrefab;
    [SerializeField] float projectileSpeed = 10f;

    public override void Active(IAttacker attacker)
    {
        base.Active(attacker);

        Vector2 offset = AttackPointOffset;
        offset.x *= attacker.GetTransform().GetFacingDir();

        Vector2 spawnPoint = (Vector2)attacker.GetTransform().position + offset;

        GameObject obj = Instantiate(_projectilePrefab, spawnPoint, Quaternion.identity);
        Projectile projectile = ComponentProvider.Get<Projectile>(obj);
        Transform target = GetTargetSingle(attacker).transform;

        projectile.Init(target, attacker.GetStatusController(), Power, DamageType, attacker.TargetLayer, projectileSpeed);
    }
}
