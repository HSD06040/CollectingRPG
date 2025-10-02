using System;
using UnityEngine;

public class MeteorBehaviour : MonoBehaviour
{
    private float _speed;
    private bool _initialized = false;
    private Vector2 _targetPos;
    private Action<Vector2> _onArrive;
    private float _timer;

    private void Update()
    {
        if (!_initialized) return;

        transform.position = Vector2.MoveTowards(transform.position, _targetPos, _speed * Time.deltaTime);
        _timer += Time.deltaTime;

        if (Vector2.Distance(transform.position, _targetPos) <= 0.1f || _timer >= 2f)
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

        Vector2 dir = (_targetPos - (Vector2)transform.position).normalized;
        transform.right = dir;
    }
}