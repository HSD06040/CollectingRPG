using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitDragDropSystem : MonoBehaviour
{
    private bool _isDragging;    
    private GameObject _currentUnit;
    private UnitBase _currentUnitBase;
    private Vector2 _offset;
    private Vector2 _pos;
    private int _currentSlotIdx = 0;
    [SerializeField] LayerMask _targetLayer;

    public event Action<UnitSlot, UnitBase> OnUnitDropped;

    private Action<Collider2D, UnitBase> OnSlotChanged;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Vector2 worldMouse = GetWorldMouse();

            RaycastHit2D hit = Physics2D.Raycast(worldMouse, Vector2.zero);

            if (hit.collider == null || !_targetLayer.Contain(hit.collider.gameObject.layer))
                return;

            if (hit.collider != null && hit.collider.CompareTag("Unit"))
            {
                SetUnit(hit.collider.gameObject);
            }
        }

        if (_isDragging && _currentUnit != null && Input.GetMouseButton(0))
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = -Camera.main.transform.position.z;
            Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePos);

            _currentUnit.transform.position = mouseWorldPos + _offset;
        }

        if (Input.GetMouseButtonUp(0) && _isDragging)
        {
            PointerEventData pointerData = new PointerEventData(EventSystem.current)
            {
                position = Input.mousePosition
            };

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, results);

            bool isSlot = false;

            foreach (var result in results)
            {
                if (!result.gameObject.CompareTag("Slot"))
                    return;

                var dropHandler = result.gameObject.GetComponent<UI_UnitSlot>();
                if (dropHandler != null)
                {
                    dropHandler.OnDrop(pointerData);

                    isSlot = true;
                    break;
                }
            }

            _isDragging = false;

            if (isSlot)
                return;

            if (_currentUnit != null)
            {
                // 슬롯 체크
                Collider2D slotCollider = Physics2D.OverlapPoint(_currentUnit.transform.position, LayerMask.GetMask("Slot"));

                if (_currentSlotIdx != -1 && _currentUnitBase != null)
                {
                    OnSlotChanged?.Invoke(slotCollider, _currentUnitBase);
                }

                if (slotCollider != null)
                {
                    UnitSlot slot = slotCollider.GetComponent<UnitSlot>();                    

                    OnUnitDropped?.Invoke(slot, _currentUnitBase);
                }
                else
                {   
                    if(_currentUnitBase != null)
                    {
                        if(_currentUnitBase.CurrentSlot == Vector2Int.zero)
                            Destroy(_currentUnit);
                        else
                            _currentUnit.transform.position = _pos; // 원래 위치로 되돌리기
                    }                    
                }                   
            }
            Clear();
        }
    }

    private static Vector2 GetWorldMouse()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = -Camera.main.transform.position.z;
        Vector2 worldMouse = Camera.main.ScreenToWorldPoint(mousePos);
        return worldMouse;
    }

    public void SetUnit(GameObject unit)
    {
        _isDragging = true;
        _currentUnit = unit;
        _currentUnitBase = _currentUnit.GetComponent<UnitBase>();
        _offset = Vector2.zero;
        _pos = _currentUnit.transform.position;
    }

    public void SetUnit(GameObject unit, Action<Collider2D, UnitBase> action, int slotIdx)
    {
        OnSlotChanged = action;

        _currentSlotIdx = slotIdx;
        _isDragging = true;
        _currentUnit = unit;
        _currentUnitBase = _currentUnit.GetComponent<UnitBase>();

        _offset = Vector2.zero;
        _pos = _currentUnit.transform.position;
    }

    private void Clear()
    {
        _currentUnit = null;
        _currentUnitBase = null;
        OnSlotChanged = null;
        _currentSlotIdx = -1;
    }

    public GameObject GetCurrentUnit()
    {
        return _currentUnit;
    }

    public UnitBase GetCurrentUnitBase()
    {
        return _currentUnitBase;
    }
    public int GetCurrentSlotIdx()
    {
        return _currentSlotIdx;
    }
}
