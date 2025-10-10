using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MagicStoneSlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private MagicStoneData _magicStoneData;
    public MagicStoneData MagicStoneData => _magicStoneData;

    public event System.Action<MagicStoneSlot> OnCleared;

    [Header("UI")]
    [SerializeField] private RectTransform _magicStone;
    [SerializeField] private Image _magicStoneIcon;
    [SerializeField] private Image _highlight;
    [SerializeField] private TMP_Text _magicStoneName;
    [SerializeField] Transform _parent;
    private Transform _dragParent;

    [Header("Drag Settings")]
    private Transform _dropAreaPanel;
    private Vector3 _originalPos;

    private bool _isDragging = false;

    private void OnDestroy()
    {
        BattleManager.OnBattleEnded -= ForceStopDrag;
    }

    public void Init(Transform dropArea, Transform dragParent)
    {
        _dragParent = dragParent;
        _dropAreaPanel = dropArea;

        BattleManager.OnBattleEnded += ForceStopDrag;

        ResetMagicStoneRect();
    }

    public void ClearMagicStone()
    {
        _magicStoneData = null;
        MagicStoneUIUpdate();
        OnCleared?.Invoke(this);
    }

    public void SetMagicStone(MagicStoneData magicStoneData)
    {
        _magicStoneData = magicStoneData;
        MagicStoneUIUpdate();
    }

    private void MagicStoneUIUpdate()
    {
        if (MagicStoneData == null)
        {
            gameObject.SetActive(false);
            _magicStoneIcon.color = Color.clear;
            _magicStoneIcon.sprite = null;
            _magicStoneName.text = string.Empty;
            return;
        }

        gameObject.SetActive(true);
        _magicStoneIcon.color = Color.white;
        _magicStoneIcon.sprite = MagicStoneData.Icon;
        _magicStoneName.text = MagicStoneData.Name;
    }

    private void UseMagicStone(Vector2 pos)
    {
        if (MagicStoneData == null || !InGameManager.Instance.IsBattle) return;

        MagicStoneData.UseMagicStone(pos);
        ClearMagicStone();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (MagicStoneData == null || !InGameManager.Instance.IsBattle) return;

        _isDragging = true;
        _originalPos = _magicStone.anchoredPosition;
        DragStart();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!_isDragging || MagicStoneData == null || !InGameManager.Instance.IsBattle) return;

        Vector3 worldPos;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(
            _magicStone,
            eventData.position,
            eventData.pressEventCamera,
            out worldPos))
        {
            _magicStone.position = worldPos;
        }

        bool insideDropArea = RectTransformUtility.RectangleContainsScreenPoint(
            _dropAreaPanel as RectTransform,
            eventData.position,
            eventData.pressEventCamera);

        _highlight.enabled = insideDropArea;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!_isDragging) return;
        _isDragging = false;

        if (MagicStoneData == null || !InGameManager.Instance.IsBattle)
        {
            ForceStopDrag();
            return;
        }

        bool insideDropArea = RectTransformUtility.RectangleContainsScreenPoint(
            _dropAreaPanel as RectTransform,
            eventData.position,
            eventData.pressEventCamera);

        if (insideDropArea)
        {
            UseMagicStone(Camera.main.ScreenToWorldPoint(eventData.position));
        }

        DragEnd();
    }

    private void DragStart()
    {
        _magicStone.SetParent(_dragParent, true);
    }

    private void DragEnd()
    {
        _magicStone.SetParent(_parent, true);
        ResetMagicStoneRect();
        _highlight.enabled = false;
        _isDragging = false;
    }

    private void ForceStopDrag()
    {
        if (!_isDragging) return;

        _isDragging = false;
        _magicStone.SetParent(_parent, true);
        _magicStone.position = _originalPos;
        _highlight.enabled = false;
    }

    private void ResetMagicStoneRect()
    {
        RectTransform rect = _magicStone;
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
    }
}
