using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[CreateAssetMenu(fileName = "RangedSkill", menuName = "Data/Unit/Skill/Ranged")]
public class RangedSkill : AttackSkill
{
    [SerializeField] float projectileSpeed = 10f;

    public override void Active(IAttacker attacker)
    {
        Projectile projectile = ComponentProvider.Get<Projectile>(
            Manager.Resources.Instantiate<GameObject>(
                EffectAddress,
                GetSpawnPoint(attacker),
                true
                )
            );

        Transform target = GetTarget(attacker);

        // 타겟이 없다면 가까이 있는 적을 타겟으로 지정
        if (target == null)
        {
            target = Utils.GetClosestTargetNonAlloc(GetSpawnPoint(attacker), 100f, attacker.TargetLayer);
        }

        projectile.Init(target, attacker.GetStatusController(), Power, DamageType, attacker.TargetLayer, projectileSpeed);
    }

    protected GameObject GetTargetSingle(IAttacker attacker)
    {
        var target = Utils.GetTargetsNonAllocSingle(attacker, SearchType.Circle, 100, new Vector2(1,1), 1, attacker.TargetLayer, GetPriorityFilter());
        return target;
    }

    private Transform GetTarget(IAttacker attacker)
    {
        if (Priority == Priority.None)
            return null;

        if (Priority == Priority.Target)
        {
            if (attacker.GetTarget() == null)
            {
                Debug.Log("[스킬] 타켓이 없습니다.");
                return null;
            }

            return attacker.GetTarget();
        }
        else
        {
            return GetTargetSingle(attacker)?.transform;
        }       
    }
}
