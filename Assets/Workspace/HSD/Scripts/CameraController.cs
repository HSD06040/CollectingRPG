using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] float _sensitivity = 2f;
    [SerializeField] float _xLimit = 10f;
    [SerializeField] float _smoothSpeed = 5f;

    private Vector3 _initialPosition;
    private Vector3 _targetPosition;
    private bool _isDragging = false;

    private void Awake()
    {
        _initialPosition = transform.position;
        _targetPosition = _initialPosition;
    }

    private void Update()
    {
        // 부드러운 카메라 이동은 항상 업데이트
        MoveCamera();
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

        Vector3 movement = new Vector3(-adjustedDelta.x, 0, 0); // 반대 방향

        _targetPosition += movement;

        // X축 리미트 적용
        float clampedX = Mathf.Clamp(_targetPosition.x, _initialPosition.x, _initialPosition.x + _xLimit);
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
}