using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "BuffRangedSkill", menuName = "Data/Unit/Skill/BuffThrow")]
public class BuffRangedSkill : RangedSkill
{
    [Header("Type")]
    [SerializeField] ActivationCondition _activationCondition;
    [SerializeField] ThrowType _throwType;    
    [SerializeField] float _parabolaYOffset;
    [SerializeField] bool _isAttack;

    [Header("Buff")]
    [SerializeField] bool _isModifier;
    [SerializeField] bool _isBuff;
    [SerializeField] bool _isAlly;
    [SerializeField] float _radius;
    [SerializeField] BuffEffectData _buffEffectData;
    [SerializeField] StatEffectModifier _statModifier;
    
    public override void Active(IAttacker attacker)
    {
        GameObject spawnObject = Manager.Resources.Load<GameObject>(EffectAddress);

        SplashBuffProjectile projectile = ComponentProvider.Get<SplashBuffProjectile>(
            Manager.Resources.Instantiate<GameObject>(
                spawnObject,
                GetSpawnPoint(attacker),
                true
                )
            );

        Transform target = GetTarget(attacker);

        projectile.transform.right = (target.position - projectile.transform.position).normalized;
        projectile.transform.Rotate(0, 0, spawnObject.transform.rotation.eulerAngles.z);

        if (target == null)
        {
            target = Utils.GetClosestTargetNonAlloc(GetSpawnPoint(attacker), 10f, GetLayerMask(attacker));
        }

        if (_isBuff)
        {
            projectile.Init(_buffEffectData, _radius, _throwType, _activationCondition, _isAttack, _isModifier,
            target, attacker.GetStatusController(), Power, DamageType, GetLayerMask(attacker), _projectileSpeed, GetExplosionEffect(),
            _parabolaYOffset);
        }
        else if (!_isBuff)
        {
            projectile.Init(_statModifier, _radius, _throwType, _activationCondition, _isAttack, _isModifier,
            target, attacker.GetStatusController(), Power, DamageType, GetLayerMask(attacker), _projectileSpeed, GetExplosionEffect(),
            _parabolaYOffset);
        }
    }

    protected override GameObject GetTargetSingle(IAttacker attacker)
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
#if UNITY_EDITOR
    public override void DrawGizmos(IAttacker attacker)
    {
        base.DrawGizmos(attacker);

        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(attacker.GetCenter(), _radius);
    }
#endif
}
