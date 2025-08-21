using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class BezierProjectile : Projectile
{
    [SerializeField] private Vector2 centerOffset;
    [SerializeField] private float time;

    private Vector2 start;
    private Vector2 end;
    private Vector2 control;
    private int curveIndex;

    public override void Init(Transform target, UnitStatusController status, float attackPower, DamageType damageType, LayerMask targetLayer, float speed)
    {
        base.Init(target, status, attackPower, damageType, targetLayer, speed);
        SetPoints();
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
    }

    protected override async UniTask MoveAndDestroyAsync(float duration)
    {
        CancellationToken token = this.GetCancellationTokenOnDestroy();

        float destroyElapsed = 0f;
        while (destroyElapsed < _lifeTime && !token.IsCancellationRequested && _target != null)
        {
            curveIndex++;

            SetPoints();

            float elapsed = 0f;
            float totalDuration = time / _speed;

            // time 동안 곡선 따라가기
            while (elapsed < totalDuration && !token.IsCancellationRequested)
            {
                elapsed += Time.deltaTime;
                destroyElapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / totalDuration);

                Vector2 pos = Mathf.Pow(1 - t, 2) * start +
                              2 * (1 - t) * t * control +
                              Mathf.Pow(t, 2) * end;

                transform.position = pos;
                await UniTask.Yield(PlayerLoopTiming.Update, token);
            }
        }

        Destroy(gameObject);
    }

    public void SetPoints()
    {
        _direction = (_target.position - transform.position).normalized;
        start = transform.position;
        end = (Vector2)_target.position;

        float direction = (curveIndex % 2 == 0) ? 1f : -1f;
        centerOffset.x = Random.Range(2f, 5f);
        control = (start + end) / 2f + (centerOffset * direction);
    }
}