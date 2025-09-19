using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BuffRangedSkill : RangedSkill
{    
    [Header("Buff")]
    [SerializeField] bool _isAlly;
    [SerializeField] float _buffRadius;
    [SerializeField] StatEffectModifier _statEffectModifier;

    public override void Active(IAttacker attacker)
    {
        SplashProjectile projectile = ComponentProvider.Get<SplashProjectile>(
            Manager.Resources.Instantiate<GameObject>(
                EffectAddress,
                GetSpawnPoint(attacker),
                true
                )
            );

        projectile.transform.localScale = attacker.GetTransform().localScale;

        Transform target = GetTarget(attacker);

        if (target == null)
        {
            target = Utils.GetClosestTargetNonAlloc(GetSpawnPoint(attacker), 100f, GetLayerMask(attacker));
        }

        projectile.Init(_statEffectModifier, _buffRadius,
            target, attacker.GetStatusController(), Power, DamageType, GetLayerMask(attacker), _projectileSpeed, GetExplosionEffect());
    }

    private GameObject GetTargetSingle(IAttacker attacker)
    {
        var target = Utils.GetTargetsNonAllocSingle(attacker, SearchType.Circle, 100, Vector2.zero, 1, GetLayerMask(attacker), GetPriorityFilter());
        return target;
    }

    private LayerMask GetLayerMask(IAttacker attacker)
    {
        if (_isAlly)
        {
            return attacker.GetAllyLayerMask();
        }
        else
            return attacker.TargetLayer;
    }

    protected Transform GetTarget(IAttacker attacker)
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
