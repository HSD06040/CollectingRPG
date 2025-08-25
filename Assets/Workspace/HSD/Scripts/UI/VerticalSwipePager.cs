using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using static UnityEngine.Rendering.DebugUI;

public class VerticalSwipePager : MonoBehaviour, IDragHandler, IEndDragHandler
{
    [Header("UI Content (페이지 묶음)")]
    [SerializeField] RectTransform _content;

    [Header("게임 오브젝트 페이지들")]
    [SerializeField] Transform[] _pages;

    [Header("설정값")]
    [SerializeField] float _swipeThreshold = 200f;
    [SerializeField] float _tweenDuration = 0.3f;
    [SerializeField] Ease _easeType = Ease.OutCubic;
    [SerializeField] float _uiToWorldRatio = 0.01f;
    [SerializeField] int _currentPage = 0;
    private int _totalPages;
    private Vector3[] _originalPagePositions;
    private Vector2 _originalUIPosition;

    private void Start()
    {
        _totalPages = _pages.Length;

        _originalPagePositions = new Vector3[_pages.Length];
        _originalUIPosition = _content.anchoredPosition;

        for (int i = 0; i < _pages.Length; i++)
        {
            _originalPagePositions[i] = _pages[i].position;
        }

        SetAnchorPos();
        MoveToPage(_currentPage, instant: true);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (UnitDragDropSystem.IsDragging)
            return;

        if (_currentPage == 0 && 0 < eventData.delta.y)
            return;
        if (_currentPage == _totalPages - 1 && 0 > eventData.delta.y)
            return;

        _content.anchoredPosition += new Vector2(0, eventData.delta.y);

        SyncGameObjectsWithUI();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (UnitDragDropSystem.IsDragging)
            return;

        if (_currentPage == 0 && 0 < eventData.delta.y)
            return;
        if (_currentPage == _totalPages - 1 && 0 > eventData.delta.y)
            return;

        float diff = eventData.position.y - eventData.pressPosition.y;
        if (Mathf.Abs(diff) > _swipeThreshold)
        {
            if (diff < 0 && _currentPage < _totalPages - 1) // 위로 드래그 다음 페이지
                _currentPage++;
            else if (diff > 0 && _currentPage > 0)         // 아래로 드래그 이전 페이지
                _currentPage--;
        }

        MoveToPage(_currentPage);
    }

    private void MoveToPage(int pageIndex, bool instant = false)
    {
        float height = ((RectTransform)transform).rect.height;
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
        // UI가 원래 위치에서 얼마나 움직였는지 계산
        Vector2 uiOffset = _content.anchoredPosition - _originalUIPosition;

        // UI 오프셋을 월드 좌표로 변환 (Y축만, X축은 필요에 따라 추가)
        Vector3 worldOffset = new Vector3(0, uiOffset.y * _uiToWorldRatio, 0);

        for (int i = 0; i < _pages.Length; i++)
        {
            Vector3 basePos;

            if (_currentPage == 0)
            {
                basePos = Camera.main.transform.position;
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
            panel.offsetMin = new Vector2(0, i * panel.rect.height);
        }
    }

    public void SetUIToWorldRatio(float ratio)
    {
        _uiToWorldRatio = ratio;
        SyncGameObjectsWithUI();
    }

    public void ResetOriginalPositions()
    {
        for (int i = 0; i < _pages.Length; i++)
        {
            _originalPagePositions[i] = _pages[i].position;
        }
        _originalUIPosition = _content.anchoredPosition;
    }
}
