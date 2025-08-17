using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitDragDropSystem : MonoBehaviour
{
    private bool _isDragging;
    private GameObject _currentUnit;
    private Vector2 _offset;
    private Vector2 _pos;
    [SerializeField] LayerMask _targetLayer;
    public event Action<UnitSlot, UnitBase> OnUnitDropped;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
             
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

            if (!_targetLayer.Contain(hit.collider.gameObject.layer))
                return;

            if (hit.collider != null && hit.collider.CompareTag("Unit"))
            {
                _isDragging = true;
                _currentUnit = hit.collider.gameObject;
                
                _offset = (Vector2)_currentUnit.transform.position - mousePosition;
                _pos = _currentUnit.transform.position;
            }
        }

        if (_isDragging && _currentUnit != null && Input.GetMouseButton(0))
        {
            Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _currentUnit.transform.position = mouseWorldPos + _offset;
        }

        if (Input.GetMouseButtonUp(0) && _isDragging)
        {
            _isDragging = false;

            if (_currentUnit != null)
            {
                // 슬롯 체크
                Collider2D slotCollider = Physics2D.OverlapPoint(_currentUnit.transform.position, LayerMask.GetMask("Slot"));
                if (slotCollider != null)
                {
                    UnitSlot slot = slotCollider.GetComponent<UnitSlot>();
                    UnitBase unit = _currentUnit.GetComponent<UnitBase>();
                    OnUnitDropped?.Invoke(slot, unit);
                }
                else
                {
                    _currentUnit.transform.position = _pos; // 원래 위치로 되돌리기
                }
            }

            _currentUnit = null;
        }
    }
}
