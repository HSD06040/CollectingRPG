using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitDragDropSystem : MonoBehaviour
{
    private bool _isDragging;
    private GameObject _currentUnit;
    private Vector2 _offset;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
             
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

            if (hit.collider != null && hit.collider.CompareTag("Unit"))
            {
                _isDragging = true;
                _currentUnit = hit.collider.gameObject;
                
                _offset = (Vector2)_currentUnit.transform.position - mousePosition;
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
                    Debug.Log("1");
                    UnitSlot slot = slotCollider.GetComponent<UnitSlot>();                    
                    slot.SetUnit(_currentUnit.GetComponent<UnitBase>());
                }
            }

            _currentUnit = null;
        }
    }
}
