using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackSkill", menuName = "Data/Unit/Skill/Attack")]
public class AttackSkill : UnitSkill
{
    [Header("Effect")]
    public string EffectAddress;
    public EffectSpawnType EffectSpawnType;
    public Vector2 SpawnPointOffset;

    [Header("Enums")]
    public DamageType DamageType;
    public Priority Priority;

    [Header("Attack")]
    public SearchType SearchType;
    public Vector2 AttackPointOffset;
    public float SizeOrRadius;
    public Vector2 BoxSize;
    public float Angle;

    public int SearchCount;
    public float Fov;

    public override void Active(IAttacker attacker)
    {
        base.Active(attacker);

        Attack(attacker);
        SpawnEffect(attacker);
    }

    private void SpawnEffect(IAttacker attacker)
    {
        if (EffectSpawnType == EffectSpawnType.Target)
        {
            Manager.Resources.Destroy(
                Manager.Resources.Instantiate<GameObject>(
                    EffectAddress,
                    attacker.GetTarget().gameObject.GetCenter(),
                    true
                ), 
             2f);
        }
        else
        {
            Vector2 spawnPosition = GetSpawnPoint(attacker);

            GameObject prefab = Manager.Resources.Get<GameObject>(EffectAddress);
            GameObject obj = Manager.Resources.Instantiate<GameObject>(prefab, spawnPosition, true);

            Vector3 finalScale = 
                prefab.transform.localScale * Mathf.Abs(attacker.GetTransform().localScale.x);

            if (obj.transform.GetFacingDir() == attacker.GetTransform().GetFacingDir())
            {
                finalScale.x *= -1;
            }

            obj.transform.localScale = finalScale;

            Manager.Resources.Destroy(obj, 2f);
        }
    }

    private void Attack(IAttacker attacker)
    {
        if (Priority == Priority.Target)
        {
            if (attacker.GetTarget() == null)
            {
                Debug.Log("[스킬] 타켓이 없습니다.");
                return;
            }

            attacker.GetStatusController().CalculateDamage(
                Power,
                DamageType,
                ComponentProvider.Get<UnitBase>(attacker.GetTarget().gameObject).StatusController
                );
        }
        else
        {
            foreach (var target in GetTargets(attacker))
            {
                attacker.GetStatusController().CalculateDamage(
                Power,
                DamageType,
                ComponentProvider.Get<UnitBase>(target.gameObject).StatusController
                );
            }
        }
    }

    protected GameObject[] GetTargets(IAttacker attacker)
    {
        Vector2 attackPoint = GetAttackPoint(attacker);

        return Utils.GetTargetsNonAlloc(attacker,
            attackPoint,
            SearchType,
            SizeOrRadius,
            BoxSize,
            Angle,
            MaxCount,
            attacker.TargetLayer);
    }

    protected GameObject GetTargetSingle(IAttacker attacker)
    {
        var target = Utils.GetTargetsNonAllocSingle(attacker, SearchType.Circle, SizeOrRadius, BoxSize, Angle, attacker.TargetLayer, GetPriorityFilter());
        return target;
    }

    protected System.Func<IAttacker, List<GameObject>, GameObject> GetPriorityFilter()
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

    protected GameObject Close(IAttacker attacker, List<GameObject> targets)
    {
        if (targets == null || targets.Count == 0)
            return null; // 대상 없음

        return targets[0];
    }

    protected GameObject Far(IAttacker attacker, List<GameObject> targets)
    {
        if (targets == null || targets.Count == 0)
            return null;

        Vector2 origin = attacker.GetTransform().position;

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

    protected GameObject LowHp(IAttacker attacker, List<GameObject> targets)
    {
        if (targets == null || targets.Count == 0)
            return null;

        List<GameObject> validTargets = new List<GameObject>();

        foreach (var target in targets)
        {
            var statusController = ComponentProvider.Get<UnitBase>(target).StatusController;
            if (statusController != null && !statusController.IsDead)
            {
                validTargets.Add(target);
            }
        }

        if (validTargets.Count == 0)
            return null;

        validTargets.Sort((a, b) =>
        {
            var statusA = ComponentProvider.Get<UnitBase>(a).StatusController;
            var statusB = ComponentProvider.Get<UnitBase>(b).StatusController;

            float hpPercentA = (float)statusA.CurHp.Value / statusA.MaxHealth.Value;
            float hpPercentB = (float)statusB.CurHp.Value / statusB.MaxHealth.Value;

            return hpPercentA.CompareTo(hpPercentB);
        });

        return validTargets[0];
    }


    protected GameObject HighHp(IAttacker attacker, List<GameObject> targets)
    {
        if (targets == null || targets.Count == 0)
            return null;

        List<GameObject> validTargets = new List<GameObject>();

        foreach (var target in targets)
        {
            var statusController = ComponentProvider.Get<UnitBase>(target).StatusController;
            if (statusController != null && !statusController.IsDead)
            {
                validTargets.Add(target);
            }
        }

        if (validTargets.Count == 0)
            return null;

        // HP 높은 순으로 정렬
        validTargets.Sort((a, b) =>
        {
            var statusA = ComponentProvider.Get<UnitBase>(a).StatusController;
            var statusB = ComponentProvider.Get<UnitBase>(b).StatusController;

            float hpPercentA = (float)statusA.CurHp.Value / statusA.MaxHealth.Value;
            float hpPercentB = (float)statusB.CurHp.Value / statusB.MaxHealth.Value;

            return hpPercentB.CompareTo(hpPercentA); // 역순
        });

        return validTargets[0];
    }

    protected System.Func<IAttacker, List<GameObject>, GameObject> ClassFilter(ClassType classType)
    {
        return (attacker, targets) =>
        {
            foreach (var target in targets)
            {
                var statusController = ComponentProvider.Get<UnitBase>(target).StatusController;
                if (statusController != null && !statusController.IsDead)
                {
                    if (statusController.Status.Data.ClassSynergy == classType)
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

    private Vector2 GetAttackPoint(IAttacker attacker)
    {
        return attacker.GetCenter()
            + new Vector2(
            attacker.GetTransform().GetFacingDir() * AttackPointOffset.x * Mathf.Abs(attacker.GetTransform().localScale.x),
            AttackPointOffset.y * Mathf.Abs(attacker.GetTransform().localScale.y
            )
        );
    }
    private Vector2 GetSpawnPoint(IAttacker attacker)
    {
        return attacker.GetCenter()
            + new Vector2(
            attacker.GetTransform().GetFacingDir() * SpawnPointOffset.x * Mathf.Abs(attacker.GetTransform().localScale.x),
            SpawnPointOffset.y * Mathf.Abs(attacker.GetTransform().localScale.y
            )
        );
    }

#if UNITY_EDITOR
    public void DrawGizmos(IAttacker attacker) // 씬 창에서 부채꼴 범위 그리기
    {
        /*
        //Transform transform = attacker.GetTransform();
        //Handles.color = Color.yellow;

        //// 시야의 시작 방향 벡터 계산
        //Vector2 startDirection = Quaternion.Euler(0, 0, Fov / 2) * attacker.GetTargetDir();

        //// DrawSolidArc 함수를 이용하여 시야 범위를 나타내는 부채꼴 그리기
        //Handles.DrawSolidArc(transform.position, Vector3.back, startDirection, Fov, SizeOrRadius);
        */

        if (attacker == null) return;

        Vector2 attackPoint = GetAttackPoint(attacker);

        Handles.color = Color.yellow;

        switch (SearchType)
        {
            case SearchType.Circle:
                Handles.DrawWireDisc(attackPoint, Vector3.forward, SizeOrRadius);
                break;

            case SearchType.Box:
                Vector3 boxCenter = attackPoint;
                Quaternion rot = Quaternion.Euler(0, 0, Angle);
                Handles.DrawWireCube(boxCenter, BoxSize);
                Handles.matrix = Matrix4x4.TRS(boxCenter, rot, Vector3.one);
                Handles.DrawWireCube(Vector3.zero, BoxSize);
                Handles.matrix = Matrix4x4.identity;
                break;
        }

        // 공격 포인트 위치 표시
        Handles.color = Color.red;
        Handles.DrawSolidDisc(attackPoint, Vector3.forward, 0.05f);

        Handles.color = Color.blue;
        Handles.DrawSolidDisc(GetSpawnPoint(attacker), Vector3.forward, 0.05f);
    }
#endif
}