using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class PanelSwiper : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Settings")]
    [SerializeField] private float _snapDuration = 0.4f;
    [SerializeField] private Ease _easeType = Ease.OutCubic;

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

        Vector2 diff = eventData.position - _startDragPos;
        Vector2 nextPos = _contentStartPos + new Vector2(diff.x, 0);

        float minX = -_panelWidth * (_panelCount - 1);
        float maxX = 0;

        if (nextPos.x > maxX)
        {
            nextPos.x = maxX;
        }
        else if (nextPos.x < minX)
        {
            nextPos.x = minX;
        }

        _content.anchoredPosition = nextPos;
    }

    public async void OnEndDrag(PointerEventData eventData)
    {
        if (_content == null) return;

        _isDragging = false;

        float endPosX = _content.anchoredPosition.x;
        int nearestIndex = Mathf.RoundToInt(-endPosX / _panelWidth);
        nearestIndex = Mathf.Clamp(nearestIndex, 0, _panelCount - 1);
        _currentIndex = nearestIndex;

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
