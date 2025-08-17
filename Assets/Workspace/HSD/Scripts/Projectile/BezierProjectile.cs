using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierProjectile : Projectile
{
    private Vector2 startPoint; // 시작점
    [SerializeField] private Vector2 centerOffset;
    [SerializeField] private float height = 1f; // 곡선의 높이

    public override void Init(Transform target, UnitStatusController status, float attackPower, DamageType damageType, LayerMask targetLayer, float speed)
    {
        base.Init(target, status, attackPower, damageType, targetLayer, speed);
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
    }

    public void SetPoints()
    {
        startPoint = transform.position;
        Vector2 targetPoint = _target.position;
    }
}
