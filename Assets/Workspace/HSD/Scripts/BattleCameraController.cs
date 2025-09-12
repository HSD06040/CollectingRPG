using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleCameraController : MonoBehaviour
{
    private float _xLimit;
    public float XLimit
    {
        get 
        {
            return _xLimit;
        }
        set 
        {
            _xLimit = value;
            SetEnemyPosition();
        }
    }

    [SerializeField] float _sensitivity = 2f;
    [SerializeField] float _smoothSpeed = 5f;

    private Vector3 _initialPosition;
    private Vector3 _targetPosition;
    private bool _isDragging = false;

    private Vector3 _battlePosition;
    private Vector3 _enemyPosition;

    private void Awake()
    {
        _initialPosition = transform.position;
        _targetPosition = _initialPosition;

        _battlePosition = _initialPosition;
    }

    private void Update()
    {
        //MoveCamera();
    }

    private void SetEnemyPosition()
    {
        _enemyPosition = _battlePosition + new Vector3(XLimit, 0, 0);
    }

    public void StartDrag()
    {
        _isDragging = true;
    }

    public void EndDrag()
    {
        _isDragging = false;
    }

    public void CameraUpdate(Vector2 dragDelta)
    {
        if (_isDragging)
        {
            UpdateTargetPosition(dragDelta);
        }
    }

    private void UpdateTargetPosition(Vector2 deltaPosition)
    {
        Vector2 adjustedDelta = deltaPosition * _sensitivity * 0.01f;

        Vector3 movement = new Vector3(-adjustedDelta.x, 0, 0);

        _targetPosition += movement;

        float clampedX = Mathf.Clamp(_targetPosition.x, _initialPosition.x, _initialPosition.x + XLimit);
        _targetPosition = new Vector3(clampedX, _targetPosition.y, _targetPosition.z);
    }

    private void MoveCamera()
    {
        transform.position = Vector3.Lerp(transform.position, _targetPosition, _smoothSpeed * Time.deltaTime);
    }

    public void ResetCamera()
    {
        _targetPosition = _initialPosition;
    }

    public void MoveToEnemy()
    {
        transform.DOMove(_enemyPosition, .3f);
    }

    public void MoveToBattle()
    {
        transform.DOMove(_battlePosition, .3f);
    }
}