using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;

public class Mangonel : MonoBehaviour
{
    [Header(“projectile”)]
    [SerializeField] private projectile _projectile;
    [SerializeField] private Transform _targetPoint;
    [SerializeField] private float _launchHeight;
    [SerializeField] private float _launchDelay;
    [Header(“Arm”)]
    [SerializeField] private Transform _arm;
    [SerializeField] private Vector3 _armTargetRotation;
    [SerializeField] private float _armAnimationDuration;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            LaunchProjectile();
        }
    }

    Private void launchProjectile()
    {
        _arm.transform.DOLocalRotate(_armTargetRotation, _armAnimationDuration);
        DOVirtual.DelayedCall(_launchDelay, callback: () =>
        {
            var targetPointPosition:Vector3 = _targetPoint.transform.position;
            _projectile.Launch(targetPointPosition, _launchHeight);
        });
    }
}