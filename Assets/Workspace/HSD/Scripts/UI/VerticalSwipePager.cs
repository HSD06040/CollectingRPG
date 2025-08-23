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

    [SerializeField] private int _currentPage = 0;
    private int _totalPages;

    void Start()
    {
        _totalPages = _pages.Length;

        SetAnchorPos();
        MoveToPage(_currentPage, instant: true);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (_currentPage == 0 && 0 < eventData.delta.y)
            return;
        if (_currentPage == _totalPages - 1 && 0 > eventData.delta.y)
            return;

        _content.anchoredPosition += new Vector2(0, eventData.delta.y);

        foreach (Transform page in _pages)
        {
            page.position += new Vector3(0, eventData.delta.y * (1f / _content.lossyScale.y), 0);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
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
            _content.anchoredPosition = targetPos;
        else
            _content.DOAnchorPos(targetPos, _tweenDuration).SetEase(_easeType);
    }

    private void SetAnchorPos()
    {
        for (int i = 0; i < _content.childCount; i++)
        {
            RectTransform panel = _content.GetChild(i).GetComponent<RectTransform>();
            panel.offsetMin = new Vector2(0, i * panel.rect.height);
        }
    }
}
