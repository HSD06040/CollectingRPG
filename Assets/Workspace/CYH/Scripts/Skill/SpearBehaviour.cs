using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearBehaviour : MonoBehaviour
{
    private float _speed;
    private bool _initialized = false;
    private Vector2 _targetPos;
    private Action<Vector2> _onArrive;

    private void Update()
    {
        if (!_initialized) return;

        transform.position = Vector2.MoveTowards(transform.position, _targetPos, _speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, _targetPos) <= 0.1f)
        {
            _onArrive?.Invoke(_targetPos);
            Destroy(gameObject);
        }
    }

    public void Init(Vector2 targetPos, float speed, Action<Vector2> onArrive)
    {
        _targetPos = targetPos;
        _speed = speed;
        _onArrive = onArrive;
        _initialized = true;

        // 날아가는 방향
        Vector2 dir = (_targetPos - (Vector2)transform.position).normalized;
    }
}
