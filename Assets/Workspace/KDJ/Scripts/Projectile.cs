using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectile : MonoBehaviour
{
    [SerializeField] private float _duration = 1.5f;
    public void Launch(Vector3 targetPoint, float launchHeight)
    {
        transform.SetParent(null);
        transform.DOJump(targetPoint, launchHeight, 1, _duration);
    }
}