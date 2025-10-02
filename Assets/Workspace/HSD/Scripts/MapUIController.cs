using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class MapUIController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] float _duration = 0.5f;
    [SerializeField] Ease _ease = Ease.InBack;

    private RectTransform _rectTransform;

    private float _upY;
    private float _downY;

#if UNITY_EDITOR
    [Button("MapEnter")]
    public void MapEnterButton() => MapEnter().Forget();
    [Button("MapExit")]
    public void MapExitButton() => MapExit().Forget();
#endif
    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        if (_rectTransform == null)
        {
            enabled = false;
            return;
        }

        _downY = 0f;

        RectTransform canvasRect = transform.root.GetComponent<RectTransform>();

        float canvasHalfHeight = canvasRect.rect.height / 2f;
        float rectHalfHeight = _rectTransform.rect.height / 2f;

        _upY = canvasHalfHeight + rectHalfHeight;

        _rectTransform.anchoredPosition = new Vector2(_rectTransform.anchoredPosition.x, _downY);
    }

    public async UniTask MapEnter()
    {
        await MapEnterAnimation();
    }

    public async UniTask MapExit()
    {
        await MapExitAnimation();
    }

    private async UniTask MapEnterAnimation()
    {
        await _rectTransform.DOAnchorPosY(_downY, _duration)
            .SetEase(_ease)
            .AsyncWaitForCompletion();
    }

    private async UniTask MapExitAnimation()
    {
        await _rectTransform.DOAnchorPosY(_upY, _duration)
            .SetEase(_ease)
            .AsyncWaitForCompletion();
    }
}