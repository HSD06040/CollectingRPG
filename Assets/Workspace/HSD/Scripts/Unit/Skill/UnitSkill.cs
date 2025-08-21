using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class UnitSkill : ScriptableObject
{
    public int ManaCost;
    public DamageType DamageType;
    public Priority Priority;

    public float SizeOrRadius;
    public Vector2 BoxSize;
    public float Angle;

    public int MaxCount;
    public int SearchCount;
    public float Fov;

    public abstract void Active(IAttacker attacker);
    

    protected GameObject[] GetTargets(IAttacker attacker)
    {
        return Utils.GetTargetsNonAlloc(attacker,
            attacker.GetTransform().position,
            SearchType.Circle,
            SizeOrRadius,
            BoxSize,
            Angle,
            MaxCount,
            attacker.TargetLayer);
    }

    protected GameObject GetTargetSingle(IAttacker attacker)
    {        
        return Utils.GetTargetsNonAllocSingle(attacker, SearchType.Circle, SizeOrRadius, BoxSize, Angle, attacker.TargetLayer, GetPriorityFilter());
    }

    private System.Func<IAttacker, List<GameObject>, GameObject> GetPriorityFilter()
    {
        switch (Priority)
        {
            case Priority.Close:
                return Close;
            case Priority.Far:
                return Far;
            case Priority.LowHp:
                return LowHp;
            case Priority.HightHp:
                return HighHp;
            case Priority.Tank:
                return ClassFilter(ClassType.Tank);
            case Priority.Melee:
                return ClassFilter(ClassType.Melee);
            case Priority.Ranged:
                return ClassFilter(ClassType.Ranged);
            case Priority.Support:
                return ClassFilter(ClassType.Support);
            default:
                return null;
        }
    }

    private GameObject Close(IAttacker attacker, List<GameObject> targets)
    {
        return targets[0];
    }

    private GameObject Far(IAttacker attacker, List<GameObject> targets)
    {
        Vector2 origin = attacker.GetTransform().position;

        // 거리 역순으로 정렬
        targets.Sort((a, b) =>
        {
            Vector2 posA = a.transform.position;
            Vector2 posB = b.transform.position;
            float distA = (origin - posA).sqrMagnitude;
            float distB = (origin - posB).sqrMagnitude;
            return distB.CompareTo(distA); // 역순
        });

        return targets[0];
    }

    private GameObject LowHp(IAttacker attacker, List<GameObject> targets)
    {
        List<GameObject> validTargets = new List<GameObject>();

        foreach (var target in targets)
        {
            var statusController = ComponentProvider.Get<UnitStatusController>(target);
            if (statusController != null && !statusController.IsDead)
            {
                validTargets.Add(target);
            }
        }

        // HP 낮은 순으로 정렬
        validTargets.Sort((a, b) =>
        {
            var statusA = ComponentProvider.Get<UnitStatusController>(a);
            var statusB = ComponentProvider.Get<UnitStatusController>(b);

            float hpPercentA = (float)statusA.CurHp.Value / statusA.MaxHealth.Value;
            float hpPercentB = (float)statusB.CurHp.Value / statusB.MaxHealth.Value;

            return hpPercentA.CompareTo(hpPercentB);
        });

        return validTargets[0];
    }

    private GameObject HighHp(IAttacker attacker, List<GameObject> targets)
    {
        List<GameObject> validTargets = new List<GameObject>();

        foreach (var target in targets)
        {
            var statusController = ComponentProvider.Get<UnitStatusController>(target);
            if (statusController != null && !statusController.IsDead)
            {
                validTargets.Add(target);
            }
        }

        // HP 높은 순으로 정렬
        validTargets.Sort((a, b) =>
        {
            var statusA = ComponentProvider.Get<UnitStatusController>(a);
            var statusB = ComponentProvider.Get<UnitStatusController>(b);

            float hpPercentA = (float)statusA.CurHp.Value / statusA.MaxHealth.Value;
            float hpPercentB = (float)statusB.CurHp.Value / statusB.MaxHealth.Value;

            return hpPercentB.CompareTo(hpPercentA); // 역순
        });

        return validTargets[0];
    }

    private System.Func<IAttacker, List<GameObject>, GameObject> ClassFilter(ClassType classType)
    {
        return (attacker, targets) =>
        {
            foreach (var target in targets)
            {
                var statusController = ComponentProvider.Get<UnitStatusController>(target);
                if (statusController != null && !statusController.IsDead)
                {
                    if (statusController.Status.Data.EnhancementData.ClassSynergy == classType)
                    {
                        return target;
                    }
                }
            }

            return targets[Random.Range(0, targets.Count)];
        };
    }

    protected GameObject[] GetConeTargets(IAttacker attacker, GameObject[] targets, int searchCount)
    {
        Transform transform = attacker.GetTransform();

        List<GameObject> searchTargets = new List<GameObject>(searchCount);

        foreach (var target in targets)
        {
            if (Vector2.Dot(transform.up, attacker.GetTargetDir()) >= Mathf.Cos(Fov / 2 * Mathf.Deg2Rad))
            {
                searchTargets.Add(target);
            }
        }

        return searchTargets.ToArray();
    }
}