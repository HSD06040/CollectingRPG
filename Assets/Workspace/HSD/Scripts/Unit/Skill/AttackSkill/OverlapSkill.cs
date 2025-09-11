using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackSkill", menuName = "Data/Unit/Skill/Attack")]
public class OverlapSkill : AttackSkill
{    
    [Header("Overlap")]
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
                Debug.Log($"[오버렙 스킬 공격 적중] {target.name}");
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
            attacker.GetTransform().GetFacingDir() * AttackPointOffset.x * ((1 + Mathf.Abs(attacker.GetTransform().localScale.x))/2),
            AttackPointOffset.y * Mathf.Abs(attacker.GetTransform().localScale.y
            )
        );
    }

#if UNITY_EDITOR
    public override void DrawGizmos(IAttacker attacker) // 씬 창에서 부채꼴 범위 그리기
    {
        /*
        //Transform transform = attacker.GetTransform();
        //Handles.color = Color.yellow;

        //// 시야의 시작 방향 벡터 계산
        //Vector2 startDirection = Quaternion.Euler(0, 0, Fov / 2) * attacker.GetTargetDir();

        //// DrawSolidArc 함수를 이용하여 시야 범위를 나타내는 부채꼴 그리기
        //Handles.DrawSolidArc(transform.position, Vector3.back, startDirection, Fov, SizeOrRadius);
        */
        base.DrawGizmos(attacker);

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
    }
#endif
}