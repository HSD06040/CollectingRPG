using UnityEngine;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    [Header("UI Panels")]
    [SerializeField] private GameObject _storePanel;
    [SerializeField] private GameObject _eventPanel;
    [SerializeField] private GameObject _battleTransitionPanel;
    
    [Header("Animation Settings")]
    [SerializeField] private float _panelAnimationDuration = 0.3f;
    [SerializeField] private Ease _panelAnimationEase = Ease.OutCubic;

    public static UIManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // 모든 패널 초기에 비활성화
        HideAllPanels();
    }

    public void ShowStorePanel()
    {
        Debug.Log("상점 진입");
        ShowPanel(_storePanel);
        
        // TODO: 상점 로직 여기에 추가
        // StageStoreManager.Instance.OpenStore();
    }

    public void ShowEventPanel()
    {
        Debug.Log("Dialog 시스템 시작");
        ShowPanel(_eventPanel);
        
        // Stage Dialog 시스템 시작
        if (StageDialogManager.Instance != null)
            StageDialogManager.Instance.StartRandomEvent();
    }

    public void ShowBattleTransition()
    {
        Debug.Log("전투 시작");
        ShowPanel(_battleTransitionPanel);
        
        // TODO: 전투 씬으로 전환 또는 전투 UI 활성화
        // StageBattleManager.Instance.StartBattle();
    }

    private void ShowPanel(GameObject panel)
    {
        if (panel == null) 
        {
            Debug.LogWarning("패널이 할당되지 않음");
            return;
        }

        // 다른 패널들 먼저 숨기기
        HideAllPanels();
        
        // 패널 활성화 및 애니메이션
        panel.SetActive(true);
        
        // 페이드 인 애니메이션 
        CanvasGroup canvasGroup = panel.GetComponent<CanvasGroup>();
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0f;
            canvasGroup.DOFade(1f, _panelAnimationDuration).SetEase(_panelAnimationEase);
        }

        // 맵 플레이어 잠금 (패널이 열린 동안 맵 클릭 방지)
        if (MapPlayerTracker.Instance != null)
            MapPlayerTracker.Instance.Locked = true;
    }

    public void HideAllPanels()
    {
        if (_storePanel != null) _storePanel.SetActive(false);
        if (_eventPanel != null) _eventPanel.SetActive(false);
        if (_battleTransitionPanel != null) _battleTransitionPanel.SetActive(false);

        // 맵 플레이어 잠금 해제
        if (MapPlayerTracker.Instance != null)
            MapPlayerTracker.Instance.Locked = false;
    }

    public void ClosePanelAndContinue()
    {
        // 패널 닫기 애니메이션 후 맵으로 복귀
        HideAllPanels();
        
        // 다음 노드 활성화 등의 로직
        // TODO: MapView 인스턴스 참조 방식 확인 필요
        /*
        if (MapView.Instance != null)
        {
            MapView.Instance.SetAttainableNodes();
            MapView.Instance.SetLineColors();
        }
        */
    }

    // 에디터에서 테스트용
    [ContextMenu("Test Store Panel")]
    void TestStorePanel() => ShowStorePanel();
    
    [ContextMenu("Test Event Panel")]
    void TestEventPanel() => ShowEventPanel();
    
    [ContextMenu("Test Battle Transition")]
    void TestBattleTransition() => ShowBattleTransition();
}