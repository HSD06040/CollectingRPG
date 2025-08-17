using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TreeEditor;
using UnityEngine;

public class BezierProjectile : Projectile
{
    [SerializeField] private Vector2 centerOffset;
    [SerializeField] private float time;

    private Vector2 start;
    private Vector2 end;
    private Vector2 control;
    private float curveIndex = 0f; // 곡선 변화를 위한 인덱스

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
            // 곡선 인덱스 증가 (매번 다른 곡선을 만들기 위해)
            curveIndex += 1f;

            // 베지어 포인트 설정 (현재 위치 기준으로 새로 계산)
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

        // 홀수/짝수 번째마다 반대 방향으로 곡선 그리기
        float direction = (curveIndex % 2 == 0) ? 1f : -1f;
        centerOffset.x = Random.Range(2f, 5f);
        control = (start + end) / 2f + (centerOffset * direction);

        Debug.Log($"CurveIndex: {curveIndex}, Direction: {direction}, Start: {start}, End: {end}, Control: {control}");
    }

    // Vector2를 회전시키는 헬퍼 메서드는 더 이상 필요없음
}