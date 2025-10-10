using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class PanelSwiper : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Settings")]
    [SerializeField] private float _snapDuration = 0.4f;
    [SerializeField] private Ease _easeType = Ease.OutCubic;

    [Header("Swipe Sensitivity")]
    [SerializeField, Range(0.1f, 1f)] private float _dragSensitivity = 0.5f;
    [SerializeField, Range(0f, 200f)] private float _swipeThreshold = 100f;

    private RectTransform _content;
    private Vector2 _startDragPos;
    private Vector2 _contentStartPos;
    private float _panelWidth;
    private int _panelCount;
    private int _currentIndex = 0;
    private bool _isDragging = false;

    private Tween _moveTween;

    public void Init(RectTransform content, int panelCount)
    {
        _content = content;
        _panelCount = panelCount;
        _panelWidth = content.rect.width;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (_content == null) return;

        _isDragging = true;
        _startDragPos = eventData.position;
        _contentStartPos = _content.anchoredPosition;

        _moveTween?.Kill();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!_isDragging) return;

        Vector2 diff = (eventData.position - _startDragPos) * _dragSensitivity;
        Vector2 nextPos = _contentStartPos + new Vector2(diff.x, 0);

        float minX = -_panelWidth * (_panelCount - 1);
        float maxX = 0;

        nextPos.x = Mathf.Clamp(nextPos.x, minX, maxX);
        _content.anchoredPosition = nextPos;
    }

    public async void OnEndDrag(PointerEventData eventData)
    {
        if (_content == null) return;
        _isDragging = false;

        float dragDelta = eventData.position.x - _startDragPos.x;

        if (Mathf.Abs(dragDelta) > _swipeThreshold)
        {
            if (dragDelta > 0)
                _currentIndex = Mathf.Max(0, _currentIndex - 1);
            else
                _currentIndex = Mathf.Min(_panelCount - 1, _currentIndex + 1);
        }
        else
        {
            // 스와이프가 짧으면 현재 패널로 스냅
            float endPosX = _content.anchoredPosition.x;
            int nearestIndex = Mathf.RoundToInt(-endPosX / _panelWidth);
            _currentIndex = Mathf.Clamp(nearestIndex, 0, _panelCount - 1);
        }

        await MoveToIndexAsync(_currentIndex);
    }

    private async UniTask MoveToIndexAsync(int index)
    {
        Vector2 target = new Vector2(-_panelWidth * index, 0);
        _moveTween?.Kill();

        _moveTween = _content.DOAnchorPos(target, _snapDuration)
            .SetEase(_easeType)
            .SetUpdate(true);

        await _moveTween.AsyncWaitForCompletion();
    }

    public async UniTask MoveToIndexPublic(int index)
    {
        index = Mathf.Clamp(index, 0, _panelCount - 1);
        _currentIndex = index;
        await MoveToIndexAsync(index);
    }
}
