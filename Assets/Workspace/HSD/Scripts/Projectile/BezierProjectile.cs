using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class BezierProjectile : Projectile
{
    [SerializeField] Vector2 randomX;
    [SerializeField] Vector2 randomY;
    [SerializeField] float _time = 5;
    [SerializeField] float _zRotateOffset;

    private Vector2 _start;
    private Vector2 _end;
    private Vector2 _control;    
    private float _randomDirection;
    private float _randomOffsetX;
    private float _randomOffsetY;

    public override void Init(Transform target, UnitStatusController status, float attackPower, DamageType damageType,
    LayerMask targetLayer, float speed, float distance)
    {
        _randomDirection = Random.value > 0.5f ? 1f : -1f;
        _randomOffsetY = Random.Range(randomY.x, randomY.y);
        _randomOffsetX = Random.Range(randomX.x, randomX.y);

        base.Init(target, status, attackPower, damageType, targetLayer, speed);
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
    }

    protected override async UniTask MoveAndDestroyAsync(float duration)
    {
        SetPoints();

        CancellationToken token = this.GetCancellationTokenOnDestroy();

        float destroyElapsed = 0f;
        float elapsed = 0f;
        float totalDuration = _time / _speed;

        while (elapsed < totalDuration && !token.IsCancellationRequested)
        {
            if(_target != null)
                _end = _target.position;
            
            elapsed += Time.deltaTime;
            destroyElapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / totalDuration);

            Vector2 pos = Mathf.Pow(1 - t, 2) * _start +
                          2 * (1 - t) * t * _control +
                          Mathf.Pow(t, 2) * _end;

            transform.right = (pos - (Vector2)transform.position).normalized;
            transform.position = pos;

            Vector3 euler = transform.rotation.eulerAngles;
            euler.z += _zRotateOffset;
            transform.rotation = Quaternion.Euler(euler);

            await UniTask.Yield(PlayerLoopTiming.Update, token);
        }

        Vector2 dir = (_end - _control).normalized;

        transform.right = dir;
        Vector3 lastEuler = transform.rotation.eulerAngles;
        lastEuler.z += _zRotateOffset;
        transform.rotation = Quaternion.Euler(lastEuler);

        while (destroyElapsed < _lifeTime && !token.IsCancellationRequested)
        {
            destroyElapsed += Time.deltaTime;
            transform.position += (Vector3)(dir * _speed * Time.deltaTime);
            await UniTask.Yield(PlayerLoopTiming.Update, token);
        }

        ProjectileDestroy();
    }


    public void SetPoints()
    {
        _start = (Vector2)transform.position + new Vector2(Random.Range(-0.3f, 0.3f), Random.Range(-1f, 1f));
        _end = (Vector2)_target.position;

        // 발사 방향
        Vector2 dir = (_end - _start).normalized;

        // 진행 방향의 법선 (왼쪽 / 오른쪽으로 수직)
        Vector2 normal = new Vector2(-dir.y, dir.x) * _randomDirection;

        _control = (_start + _end) / 2f
                 + dir * _randomOffsetX
                 + normal * _randomOffsetY;

        //float distance = Vector2.Distance(_start, _end);
        //float factor = Mathf.Clamp01(distance / 5f); // 5f는 최대 사거리 정도로 조정

        //Vector2 dir = (_end - _start).normalized;
        //Vector2 normal = new Vector2(-dir.y, dir.x) * _randomDirection;

        //_control = (_start + _end) / 2f
        //         + dir * (_randomOffsetX * factor)
        //         + normal * (_randomOffsetY * factor);
    }
}