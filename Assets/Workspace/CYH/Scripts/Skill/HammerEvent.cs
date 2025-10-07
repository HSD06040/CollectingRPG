using System;
using UnityEngine;

public class HammerEvent : MonoBehaviour
{
    public Action OnHammerDestroyed;
    private float _lifeTime = 1f;

    private void Start()
    { 
        Destroy(gameObject, _lifeTime);
    }

    private void OnDestroy()
    {
        OnHammerDestroyed?.Invoke();
    }
}