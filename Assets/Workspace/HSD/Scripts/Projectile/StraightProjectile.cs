using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StraightProjectile : Projectile
{
    [Header("Straight")]
    [SerializeField] float _distance;

    protected override async UniTask MoveAndDestroyAsync(float duration)
    {
        _dir = new Vector2(_status.transform.GetFacingDir(), 0);

        await transform.DOMoveX(transform.position.x + _distance, duration)
            .AsyncWaitForCompletion();

        Manager.Resources.Destroy(gameObject);
    }
}
