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

    private void OnDestroy()
    {
        BattleManager.OnBattleEnded -= DragEnd;
    }

    public void Init(Transform dropArea, Transform dragParent)
    {
        _dragParent = dragParent;
        _dropAreaPanel = dropArea;

        BattleManager.OnBattleEnded += DragEnd;
        DragEnd();
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
        _originalPos = _highlight.rectTransform.position;
        DragStart();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (MagicStoneData == null || !InGameManager.Instance.IsBattle) return;

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
        if (MagicStoneData == null || !InGameManager.Instance.IsBattle) return;

        bool insideDropArea = RectTransformUtility.RectangleContainsScreenPoint(
            _dropAreaPanel as RectTransform,
            eventData.position,
            eventData.pressEventCamera);

        if (insideDropArea)
        {
            UseMagicStone(Camera.main.ScreenToWorldPoint(eventData.position));
        }

        _magicStone.position = _originalPos;

        _highlight.enabled = false;

        DragEnd();
    }

    private void DragStart()
    {
        _highlight.transform.SetParent(_dragParent, true);
    }

    private void DragEnd()
    {
        _highlight.transform.SetParent(_parent, true);
    }
}
