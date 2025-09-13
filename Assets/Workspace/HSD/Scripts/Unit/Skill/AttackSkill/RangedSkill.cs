using UnityEngine;


[CreateAssetMenu(fileName = "RangedSkill", menuName = "Data/Unit/Skill/Ranged")]
public class RangedSkill : AttackSkill
{
    [SerializeField] float _projectileSpeed = 10f;
    [SerializeField] float _distance = 5f;

    public override void Active(IAttacker attacker)
    {
        Projectile projectile = ComponentProvider.Get<Projectile>(
            Manager.Resources.Instantiate<GameObject>(
                EffectAddress,
                GetSpawnPoint(attacker),
                true
                )
            );

        projectile.transform.localScale = attacker.GetTransform().localScale;

        Transform target = GetTarget(attacker);

        // 타겟이 없다면 가까이 있는 적을 타겟으로 지정
        if (target == null)
        {
            target = Utils.GetClosestTargetNonAlloc(GetSpawnPoint(attacker), 100f, attacker.TargetLayer);
        }

        projectile.Init(target, attacker.GetStatusController(), Power, DamageType, attacker.TargetLayer, _projectileSpeed, _distance);
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
