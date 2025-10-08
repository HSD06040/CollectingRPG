using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Cysharp.Threading.Tasks;

public class PanelSwitcher : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Panels")]
    public GameObject[] panels;
    public Animator[] panelAnimators;
    
    [Header("Navigation Buttons")]
    public Button leftButton;
    public Button rightButton;
    
    [Header("Swipe Settings")]
    public float swipeThreshold = 50f;
    
    [Header("Animation Settings")]
    public string showAnimationName = "GachaPanel";
    public string hideAnimationName = "GachaPanel2";
    public float animationDelay = 0.3f;
    
    private int currentPanelIndex = 0;
    private bool isTransitioning = false;
    private Vector2 dragStartPos;

    void Start()
    {
        if (leftButton != null)
            leftButton.onClick.AddListener(PreviousPanel);
        if (rightButton != null)
            rightButton.onClick.AddListener(NextPanel);
        
        InitializePanels();
    }

    void InitializePanels()
    {
        for (int i = 0; i < panels.Length; i++)
        {
            panels[i].SetActive(i == currentPanelIndex);
        }
        UpdateButtonStates();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (isTransitioning) return;
        dragStartPos = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (isTransitioning) return;
        
        float dragDistance = eventData.position.x - dragStartPos.x;
        
        if (Mathf.Abs(dragDistance) > swipeThreshold)
        {
            if (dragDistance > 0)
            {
                PreviousPanel();
            }
            else
            {
                NextPanel();
            }
        }
    }

    public void PreviousPanel()
    {
        if (currentPanelIndex > 0 && !isTransitioning)
        {
            ShowPanel(currentPanelIndex - 1);
        }
    }

    public void NextPanel()
    {
        if (currentPanelIndex < panels.Length - 1 && !isTransitioning)
        {
            ShowPanel(currentPanelIndex + 1);
        }
    }

    public void ShowPanel(int index)
    {
        if (index < 0 || index >= panels.Length || isTransitioning) return;
        
        TransitionToPanel(index).Forget();
    }

    async UniTask TransitionToPanel(int newIndex)
    {
        isTransitioning = true;
        
        if (currentPanelIndex < panelAnimators.Length && panelAnimators[currentPanelIndex] != null)
        {
            panelAnimators[currentPanelIndex].Play(hideAnimationName);
        }
        
        await UniTask.Delay((int)(animationDelay * 1000));
        
        panels[currentPanelIndex].SetActive(false);
        panels[newIndex].SetActive(true);
        currentPanelIndex = newIndex;
        
        if (currentPanelIndex < panelAnimators.Length && panelAnimators[currentPanelIndex] != null)
        {
            panelAnimators[currentPanelIndex].Play(showAnimationName);
        }
        
        UpdateButtonStates();
        
        await UniTask.Delay((int)(animationDelay * 1000));
        isTransitioning = false;
    }

    void UpdateButtonStates()
    {
        if (leftButton != null)
            leftButton.interactable = currentPanelIndex > 0;
        
        if (rightButton != null)
            rightButton.interactable = currentPanelIndex < panels.Length - 1;
    }

    public int GetCurrentPanelIndex()
    {
        return currentPanelIndex;
    }
    
    public void JumpToPanel(int index)
    {
        if (index < 0 || index >= panels.Length) return;
        
        panels[currentPanelIndex].SetActive(false);
        currentPanelIndex = index;
        panels[currentPanelIndex].SetActive(true);
        
        UpdateButtonStates();
    }
}