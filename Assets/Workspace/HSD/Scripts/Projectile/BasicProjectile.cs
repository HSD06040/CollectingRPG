using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class BasicProjectile : Projectile
{
    public override void Init(Transform target, UnitStatusController status, float attackPower, DamageType damageType,
        LayerMask targetLayer, float speed, float distance)
    {
        base.Init(target, status, attackPower, damageType, targetLayer, speed);
        transform.right = _targetDir;
    }

    protected override async UniTask MoveAndDestroyAsync(float duration)
    {
        float elapsed = 0f;

        // 발사체가 파괴되면 자동 취소되도록 CancellationToken 연결
        CancellationToken token = this.GetCancellationTokenOnDestroy();

        while (elapsed < duration && !token.IsCancellationRequested)
        {            
            transform.Translate(_targetDir * _speed * Time.deltaTime, Space.World);
            elapsed += Time.deltaTime;
            await UniTask.Yield(PlayerLoopTiming.Update, token); // 프레임마다 대기
        }

        if (!token.IsCancellationRequested)
            Destroy(gameObject);
    }
}
