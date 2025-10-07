using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GachaPanelSlide : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    
    [Header("Panel Settings")]
    public RectTransform panelContainer; 
    public RectTransform[] panels; 
    
    [Header("Animation Settings")]
    public float transitionSpeed = 10f; 
    public float swipeThreshold = 50f; 
    
    [Header("Navigation Buttons (Optional)")]
    public Button leftButton;
    public Button rightButton;
    
    private int currentPanel = 0;
    private Vector2 panelLocation;
    private bool isDragging = false;
    private Vector2 dragStartPos;
    private float panelWidth;

    void Start()
    {
        panelWidth = GetComponent<RectTransform>().rect.width;
        
        if (leftButton != null)
            leftButton.onClick.AddListener(() => MovePanel(-1));
        if (rightButton != null)
            rightButton.onClick.AddListener(() => MovePanel(1));
        
        UpdatePanelPosition(false);
    }

    void Update()
    {
        if (!isDragging)
        {
            Vector2 targetPosition = new Vector2(-currentPanel * panelWidth, 0);
            panelContainer.anchoredPosition = Vector2.Lerp(
                panelContainer.anchoredPosition,
                targetPosition,
                Time.deltaTime * transitionSpeed
            );
        }
    }

    
    public void MovePanel(int direction)
    {
        int newPanel = Mathf.Clamp(currentPanel + direction, 0, panels.Length - 1);
        if (newPanel != currentPanel)
        {
            currentPanel = newPanel;
            UpdatePanelPosition(true);
        }
    }

   
    public void GoToPanel(int index)
    {
        currentPanel = Mathf.Clamp(index, 0, panels.Length - 1);
        UpdatePanelPosition(true);
    }

    
    public void OnBeginDrag(PointerEventData data)
    {
        isDragging = true;
        dragStartPos = data.position;
    }

    
    public void OnDrag(PointerEventData data)
    {
        float difference = data.position.x - dragStartPos.x;
        Vector2 targetPos = new Vector2(-currentPanel * panelWidth + difference, 0);
        panelContainer.anchoredPosition = targetPos;
    }

    // 드래그 종료
    public void OnEndDrag(PointerEventData data)
    {
        isDragging = false;
        float difference = data.position.x - dragStartPos.x;
        
        // 스와이프 거리에 따라 패널 전환
        if (Mathf.Abs(difference) > swipeThreshold)
        {
            if (difference > 0)
                MovePanel(-1); 
            else
                MovePanel(1); 
        }
        else
        {
            UpdatePanelPosition(true);
        }
    }

    private void UpdatePanelPosition(bool animate)
    {
        Vector2 targetPosition = new Vector2(-currentPanel * panelWidth, 0);
        
        if (!animate)
        {
            panelContainer.anchoredPosition = targetPosition;
        }
    }

    // 현재 패널 인덱스 반환
    public int GetCurrentPanel()
    {
        return currentPanel;
    }
}
