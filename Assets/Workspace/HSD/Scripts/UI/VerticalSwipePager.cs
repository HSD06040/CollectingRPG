using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class VerticalSwipePager : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    [SerializeField] SlotPositionSetter _slotPositionSetter;
    [SerializeField] SwitchButtonController _switchButtonController;
    private enum DragDirection { None, Horizontal, Vertical }
    private DragDirection _dragDirection = DragDirection.None;

    [Header("UI Content")]
    [SerializeField] RectTransform _content;

    [Header("GameObject Pages")]
    [SerializeField] Transform[] _pages;

    [Header("Settings")]
    [SerializeField] float _swipeThreshold = 200f;
    [SerializeField] float _tweenDuration = 0.3f;
    [SerializeField] Ease _easeType = Ease.OutCubic;
    [SerializeField] int _currentPage = 0;
    [SerializeField] Vector2 _cameraOffset;

    [Header("Horizontal Settings")]
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
    [SerializeField] float _xDragSensitivity;
    [SerializeField] float _xTweenDuration = .5f;
    private float _initialCameraX;
    private float _targetCameraX;

    private int _totalPages;
    private Vector3[] _originalPagePositions;
    private Vector2 _originalUIPosition;

    private float _lastDragDeltaX;
    private bool _isBattle => InGameManager.Instance.IsBattle;
    private Camera cam;

    #region LifeCycle
    private void Start()
    {
        cam = Camera.main;
        _slotPositionSetter.SetPositions();
        Init();
    }

    private void OnEnable()
    {
        BattleManager.OnGameStanby += _slotPositionSetter.SetPositions;
    }

    private void OnDisable()
    {
        BattleManager.OnGameStanby -= _slotPositionSetter.SetPositions;
    }
    #endregion    

    private void SetEnemyPosition()
    {
        _targetCameraX = _initialCameraX + XLimit;
    }

    private void Init()
    {
        _totalPages = _pages.Length;

        _originalPagePositions = new Vector3[_pages.Length];
        _originalUIPosition = _content.anchoredPosition;

        for (int i = 0; i < _pages.Length; i++)
        {
            _originalPagePositions[i] = _pages[i].position;
        }

        _initialCameraX = cam.transform.position.x;

        SetAnchorPos();
        MoveToPage(_currentPage, instant: true);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _dragDirection = DragDirection.None;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (UnitDragDropSystem.IsDragging || _isBattle)
            return;

        if (_dragDirection == DragDirection.None)
        {
            if (Mathf.Abs(eventData.delta.x) > Mathf.Abs(eventData.delta.y))
                _dragDirection = DragDirection.Horizontal;
            else
                _dragDirection = DragDirection.Vertical;
        }

        if (_dragDirection == DragDirection.Horizontal && _currentPage == 1)
        {
            float deltaX = -eventData.delta.x * 0.01f;
            _lastDragDeltaX = deltaX;

            _targetCameraX = Mathf.Clamp(
                cam.transform.position.x + deltaX,
                _initialCameraX,
                _initialCameraX + XLimit
            );

            Vector3 camPos = cam.transform.position;
            camPos.x = _targetCameraX;
            cam.transform.position = camPos;
            return;
        }

        if (_dragDirection == DragDirection.Vertical)
        {
            if (_currentPage == 0 && 0 < eventData.delta.y)
                return;
            if (_currentPage == _totalPages - 1 && 0 > eventData.delta.y)
                return;

            _content.anchoredPosition += new Vector2(0, eventData.delta.y);
            SyncGameObjectsWithUI();
        }
    }


    public void OnEndDrag(PointerEventData eventData)
    {
        if (UnitDragDropSystem.IsDragging || _isBattle)
            return;

        if (_dragDirection == DragDirection.Horizontal && _currentPage == 1)
        {
            float flingDistance = _lastDragDeltaX * _xDragSensitivity;

            float targetX = cam.transform.position.x + flingDistance;

            _targetCameraX = Mathf.Clamp(targetX, _initialCameraX, _initialCameraX + XLimit);

            cam.transform.DOMoveX(_targetCameraX, _xTweenDuration)
            .OnComplete(OnCameraMoved);

            _lastDragDeltaX = 0;
        }
        else if (_dragDirection == DragDirection.Vertical)
        {
            if (_currentPage == 0 && 0 < eventData.delta.y)
                return;
            if (_currentPage == _totalPages - 1 && 0 > eventData.delta.y)
                return;

            float diff = eventData.position.y - eventData.pressPosition.y;
            if (Mathf.Abs(diff) > _swipeThreshold)
            {
                if (diff < 0 && _currentPage < _totalPages - 1)
                    _currentPage++;
                else if (diff > 0 && _currentPage > 0)
                    _currentPage--;
            }

            MoveToPage(_currentPage);
        }

        _dragDirection = DragDirection.None;
    }


    public void MoveToPage(int pageIndex, bool instant = false)
    {
        _currentPage = pageIndex;

        float height = Screen.height;
        Vector2 targetPos = new Vector2(0, -pageIndex * height);

        if (instant)
        {
            _content.anchoredPosition = targetPos;
            SyncGameObjectsWithUI();
        }
        else
        {
            _content.DOAnchorPos(targetPos, _tweenDuration)
                .SetEase(_easeType)
                .OnUpdate(() => SyncGameObjectsWithUI());
        }
    }

    private void SyncGameObjectsWithUI()
    {
        Vector2 uiOffset = _content.anchoredPosition - _originalUIPosition;

        Vector3 screenOffset = new Vector3(0, uiOffset.y, 0);

        Vector3 worldOffset = cam.ScreenToWorldPoint(
            cam.WorldToScreenPoint(Vector3.zero) + screenOffset
        ) - cam.ScreenToWorldPoint(Camera.main.WorldToScreenPoint(Vector3.zero));

        for (int i = 0; i < _pages.Length; i++)
        {
            Vector3 basePos;

            if (i == 1)
            {
                basePos = cam.transform.position + (Vector3)_cameraOffset;
            }
            else
            {
                basePos = _originalPagePositions[i];
            }

            _pages[i].position = basePos + worldOffset;
        }
    }

    private void SetAnchorPos()
    {
        for (int i = 0; i < _content.childCount; i++)
        {
            RectTransform panel = _content.GetChild(i).GetComponent<RectTransform>();
            panel.anchoredPosition = new Vector2(0, i * panel.rect.height);
        }
    }

    public void MoveToEnemy()
    {
        float targetX = _initialCameraX + XLimit;
        cam.transform.DOMoveX(targetX, 0.3f)
            .OnComplete(OnCameraMoved);
    }

    public void MoveToBattle()
    {
        cam.transform.DOMoveX(_initialCameraX, 0.3f)
            .OnComplete(OnCameraMoved);
    }

    private void OnCameraMoved()
    {
        if (cam.transform.position.x > XLimit / 2)
        {
            _switchButtonController.EnemySlotButtonSetting();
        }
        else
        {
            _switchButtonController.EnemyBattleSlotButtonSetting();
        }
    }
}
