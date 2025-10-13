using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitDragDropSystem : MonoBehaviour
{
    public static bool IsDragging;
    public ToolTipController ToolTipController;

    [Header("Press Settings")]
    [SerializeField] private float _dragThreshold = 0.3f; // 이 시간 이상 누르면 드래그 시작
    private float _currentPressTime;
    private bool _isPressing;
    private bool _dragStarted;
    private bool _isUI;
    private Vector2 _pressStartPosition;
    private GameObject _pressedObject; // 누른 대상 오브젝트

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
#if UNITY_EDITOR || UNITY_STANDALONE
        // --- 에디터/PC 전용 입력 ---
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            StartPress(Input.mousePosition);
        }

        if (_isPressing && Input.GetMouseButton(0))
        {
            UpdatePress(Input.mousePosition);
        }

        if (IsDragging && _currentUnit != null && Input.GetMouseButton(0))
        {
            DragUnit(Input.mousePosition);
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (_isPressing && !_dragStarted)
            {
                // 드래그 시작 전에 뗐다면 짧은 클릭으로 처리
                HandleShortClick();
            }

            if (IsDragging)
            {
                ReleaseUnit();
            }

            EndPress();
        }

#elif UNITY_ANDROID
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                StartPress(touch.position);
            }

            if (_isPressing && (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved))
            {
                UpdatePress(touch.position);
            }

            if (IsDragging && _currentUnit != null && touch.phase == TouchPhase.Moved)
            {
                DragUnit(touch.position);
            }

            if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                if (IsDragging)
                {
                    ReleaseUnit();
                }
                else if (_isPressing)
                {
                    HandleShortClick();
                }
                EndPress();
            }
        }
#endif
    }

    private void StartPress(Vector2 inputPosition)
    {
        _isPressing = true;
        _currentPressTime = 0f;
        _dragStarted = false;
        _pressStartPosition = inputPosition;
        _pressedObject = null;
        _isUI = false;

        Vector2 worldMouse = GetWorldMouseFromScreenPosition(inputPosition);
        RaycastHit2D[] hits = Physics2D.RaycastAll(worldMouse, Vector2.zero);

        if (hits.Length > 0)
        {
            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].collider != null)
                {
                    if (hits[i].collider.CompareTag("UnitTrigger") ||
                        hits[i].collider.CompareTag("Unit") ||
                        hits[i].collider.CompareTag("BattleUnit"))
                    {
                        _pressedObject = hits[i].collider.gameObject;
                        return;
                    }
                }
            }
        }
        else
        {
            PointerEventData pointerData = new PointerEventData(EventSystem.current)
            {
                position = Input.mousePosition
            };

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, results);

            foreach (var result in results)
            {
                if (!result.gameObject.CompareTag("Slot"))
                    continue;

                _isUI = true;
                _pressedObject = result.gameObject;
                break;
            }
        }
    }

    /// <summary>
    /// 누르기 업데이트 (시간 체크 및 드래그 시작 판단)
    /// </summary>
    private void UpdatePress(Vector2 currentPosition)
    {
        if (!_isPressing || _dragStarted)
            return;

        _currentPressTime += Time.deltaTime;

        if (_currentPressTime >= _dragThreshold)
        {
            _dragStarted = true;
            StartDrag();
        }
    }

    /// <summary>
    /// 드래그 시작
    /// </summary>
    private void StartDrag()
    {
        if (_pressedObject == null)
            return;

        if (!_isUI)
        {
            if (_pressedObject.CompareTag("UnitTrigger"))
            {
                SetUnit(_pressedObject);
            }
        }
        else
        {
            _pressedObject.GetComponent<UI_UnitSlot>().OnBeginDrag();
        }
    }

    /// <summary>
    /// 짧은 클릭 처리 (UnitPanel 표시)
    /// </summary>
    private void HandleShortClick()
    {
        if (_pressedObject != null)
        {
            bool isEnemy = false;

            if (_pressedObject.CompareTag("Unit") || _pressedObject.CompareTag("UnitTrigger"))
            {
                UnitBase unitBase = _pressedObject.GetComponentInParent<UnitBase>();
                if (unitBase != null)
                {
                    isEnemy = unitBase.GetAllyLayerMask().Contain(LayerMask.NameToLayer("Enemy"));

                    ToolTipController.UnitToolTip.Show(
                        unitBase.Status,
                        false,
                        true,
                        isEnemy
                    );

                    SynergyToolTipClose();
                }
            }
            else if (_pressedObject.CompareTag("BattleUnit"))
            {
                UnitBase unitBase = ComponentProvider.Get<UnitBase>(_pressedObject);
                if (unitBase != null)
                {
                    isEnemy = unitBase.GetAllyLayerMask().Contain(LayerMask.NameToLayer("Enemy"));

                    ToolTipController.UnitToolTip.Show(
                        unitBase.Status,
                        false,
                        false,
                        isEnemy
                    );

                    SynergyToolTipClose();
                }
            }
            return;
        }

        ToolTipController.UnitToolTip.Close();
        SynergyToolTipClose();
    }

    private void SynergyToolTipClose()
    {
        //ToolTipController.SynergyToolTip.Close();
    }

    /// <summary>
    /// 누르기 종료
    /// </summary>
    private void EndPress()
    {
        _isPressing = false;
        _currentPressTime = 0f;
        _dragStarted = false;
        _pressedObject = null;
    }

    private void DragUnit(Vector3 inputPosition)
    {
        inputPosition.z = -Camera.main.transform.position.z;
        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(inputPosition);

        _currentUnitBase.transform.position = mouseWorldPos + _offset;
    }

    private void ReleaseUnit()
    {
        bool isSlot;
        CheckUISlot(out isSlot);

        IsDragging = false;

        if (isSlot)
        {
            return;
        }

        if (_currentUnit != null)
        {
            CheckSlot();
        }

        Clear();
    }

    private void CheckUISlot(out bool isSlot)
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        isSlot = false;
        foreach (var result in results)
        {
            if (!result.gameObject.CompareTag("Slot"))
                continue;

            if (result.gameObject.TryGetComponent<UI_UnitSlot>(out var dropHandler))
            {
                dropHandler.OnDrop(pointerData);

                isSlot = true;
                break;
            }
        }
    }

    private void CheckSlot()
    {
        // 슬롯 체크
        Collider2D slotCollider = Physics2D.OverlapPoint(_currentUnitBase.transform.position, LayerMask.GetMask("Slot"));

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
            if (_currentUnitBase != null)
            {
                if (_currentUnitBase.CurrentSlot == Vector2Int.zero)
                    Destroy(_currentUnitBase.gameObject);
                else
                    _currentUnitBase.transform.position = _pos; // 원래 위치로 되돌리기

                _currentUnitBase.Idle();
            }
        }
    }

    private static Vector2 GetWorldMouse()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = -Camera.main.transform.position.z;
        Vector2 worldMouse = Camera.main.ScreenToWorldPoint(mousePos);
        return worldMouse;
    }

    private static Vector2 GetWorldMouseFromScreenPosition(Vector2 screenPosition)
    {
        Vector3 pos = screenPosition;
        pos.z = -Camera.main.transform.position.z;
        return Camera.main.ScreenToWorldPoint(pos);
    }

    public void SetUnit(GameObject unit)
    {
        IsDragging = true;
        _currentUnit = unit;
        _currentUnitBase = _currentUnit.GetComponentInParent<UnitBase>();
        _currentUnitBase.Drag();
        _offset = Vector2.zero;
        _pos = _currentUnitBase.transform.position;
    }

    public void SetUnit(GameObject unit, Action<Collider2D, UnitBase> action, int slotIdx)
    {
        OnSlotChanged = action;

        _currentSlotIdx = slotIdx;
        IsDragging = true;
        _currentUnit = unit;
        _currentUnitBase = _currentUnit.GetComponentInParent<UnitBase>();
        _currentUnitBase.Drag();

        _offset = Vector2.zero;
        _pos = _currentUnitBase.transform.position;
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

    private bool IsPointerOverUI()
    {
        if (EventSystem.current == null)
            return false;

        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);
        return results.Count > 0;
    }
}