using System.Collections.Generic;
using UnityEngine;

public class StageDialogManager : MonoBehaviour
{
    [Header("Dialog Events")]
    [SerializeField] private List<StageDialogEventSO> _eventSOs = new List<StageDialogEventSO>(); 
    [SerializeField] private List<DialogEvent> _allEvents = new List<DialogEvent>(); 
    
    [Header("Auto Load Default Events")]
    [Tooltip("기본 7가지 이벤트를 자동으로 로드")]
    [SerializeField] private bool _useDefaultEvents = true;
    
    [Header("UI References")]
    [SerializeField] private UnityEngine.UI.Text _dialogText;           
    [SerializeField] private UnityEngine.UI.Image _eventImage;          
    [SerializeField] private Transform _choiceButtonContainer;         
    [SerializeField] private GameObject _choiceButtonPrefab;            
    
    [Header("Result UI")]
    [SerializeField] private GameObject _resultPanel;                   
    [SerializeField] private UnityEngine.UI.Text _resultText;           
    [SerializeField] private UnityEngine.UI.Text _rewardText;           
    [SerializeField] private UnityEngine.UI.Button _continueButton;     

    public static StageDialogManager Instance { get; private set; }
    
    private DialogEvent _currentEvent;
    private List<GameObject> _activeChoiceButtons = new List<GameObject>();

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
        if (_continueButton != null)
            _continueButton.onClick.AddListener(OnContinueClicked);
            
        if (_resultPanel != null)
            _resultPanel.SetActive(false);
            
       
        InitializeEvents();
    }

    private void InitializeEvents()
    {
        
        _allEvents.Clear();
        
       
        if (_eventSOs.Count > 0)
        {
            Debug.Log($"ScriptableObject에서 {_eventSOs.Count}개 이벤트 로드");
            foreach (var eventSO in _eventSOs)
            {
                if (eventSO != null)
                    _allEvents.Add(eventSO.dialogEvent);
            }
        }
        
        // 기본 7가지 이벤트 로드
        if (_useDefaultEvents && _allEvents.Count == 0)
        {
            Debug.Log("기본 7가지 Stage Dialog 이벤트 로드");
            _allEvents.AddRange(StageDialogEvents.GetDefault7Events());
        }
        
    }

    public void StartRandomEvent()
    {
        if (_allEvents.Count == 0)
        {
            Debug.LogWarning("Stage Dialog 이벤트가 설정되지 않았습니다");
            return;
        }

        // 랜덤 이벤트 선택
        _currentEvent = _allEvents[Random.Range(0, _allEvents.Count)];
        DisplayEvent(_currentEvent);
        
    }

    public void StartEventById(string eventId)
    {
        DialogEvent targetEvent = _allEvents.Find(e => e.eventId == eventId);
        if (targetEvent != null)
        {
            _currentEvent = targetEvent;
            DisplayEvent(_currentEvent);
      
        }
    }

    private void DisplayEvent(DialogEvent eventData)
    {
        // 메인 대화 텍스트 표시
        if (_dialogText != null)
            _dialogText.text = eventData.dialogText;
            
        // 이벤트 이미지 표시
        if (_eventImage != null && eventData.eventImage != null)
            _eventImage.sprite = eventData.eventImage;
            
        // 기존 선택지 버튼들 제거
        ClearChoiceButtons();
        
        // 새 선택지 버튼들 생성
        CreateChoiceButtons(eventData.choices);
        
        
        if (eventData.eventSound != null)
        {
            AudioSource audioSource = GetComponent<AudioSource>();
            if (audioSource != null)
                audioSource.PlayOneShot(eventData.eventSound);
        }
    }

    private void CreateChoiceButtons(List<DialogChoice> choices)
    {
        if (_choiceButtonContainer == null || _choiceButtonPrefab == null) return;
        
        for (int i = 0; i < choices.Count; i++)
        {
            GameObject buttonObj = Instantiate(_choiceButtonPrefab, _choiceButtonContainer);
            UnityEngine.UI.Button button = buttonObj.GetComponent<UnityEngine.UI.Button>();
            UnityEngine.UI.Text buttonText = buttonObj.GetComponentInChildren<UnityEngine.UI.Text>();
            
            if (buttonText != null)
                buttonText.text = choices[i].choiceText;
                
            // 클로저 문제 해결을 위한 로컬 변수
            int choiceIndex = i;
            if (button != null)
                button.onClick.AddListener(() => OnChoiceSelected(choiceIndex));
                
            _activeChoiceButtons.Add(buttonObj);
        }
    }

    private void ClearChoiceButtons()
    {
        foreach (GameObject button in _activeChoiceButtons)
        {
            if (button != null)
                Destroy(button);
        }
        _activeChoiceButtons.Clear();
    }

    public void OnChoiceSelected(int choiceIndex)
    {
        if (_currentEvent == null || choiceIndex >= _currentEvent.choices.Count)
        {
            Debug.LogError("잘못된 선택지 인덱스");
            return;
        }

        DialogChoice selectedChoice = _currentEvent.choices[choiceIndex];
        
        Debug.Log($"선택지 선택됨: {selectedChoice.choiceText}");
        
        // 랜덤하게 결과 선택
        DialogResult selectedResult = selectedChoice.GetRandomResult();
        
        if (selectedResult != null)
        {
            Debug.Log($"결과 [ID: {selectedResult.resultId}]: {selectedResult.resultText}");
            
            // 보상 적용
            ApplyReward(selectedResult);
            
            // 결과 표시
            ShowResult(selectedResult);
        }
        else
        {
            Debug.LogError("선택된 선택지에 결과가 없습니다");
        }
    }

    private void ApplyReward(DialogResult result)
    {
        // TODO: 실제 보상 시스템 구현
        Debug.Log($"TODO: 보상 적용 - {result.rewardDescription}");
        
        /*
        // 예시 보상 시스템 (나중에 구현)
        switch (result.rewardType)
        {
            case 1: // 골드
                StageGameManager.Instance.AddGold(result.rewardAmount);
                break;
            case 2: // 체력
                StagePlayerStats.Instance.HealHealth(result.rewardAmount);
                break;
            case 3: // 카드
                StageCardManager.Instance.AddRandomCards(result.rewardAmount);
                break;
            // ... 기타 보상 타입들
        }
        */
    }

    private void ShowResult(DialogResult result)
    {
        // 선택지 버튼들 숨기기
        ClearChoiceButtons();
        
        // 결과 패널 표시
        if (_resultPanel != null)
        {
            _resultPanel.SetActive(true);
            
            if (_resultText != null)
                _resultText.text = result.resultText;
                
            if (_rewardText != null)
                _rewardText.text = result.rewardDescription;
        }
        else
        {
            // 결과 패널이 없으면 즉시 계속
            Debug.Log($"Stage Dialog 결과: {result.resultText} | 보상: {result.rewardDescription}");
            OnContinueClicked();
        }
    }

    private void OnContinueClicked()
    {
        // 결과 패널 숨기기
        if (_resultPanel != null)
            _resultPanel.SetActive(false);
            
        // 이벤트 완료
        CompleteEvent();
    }

    private void CompleteEvent()
    {
        Debug.Log($"Stage Dialog 이벤트 완료 [ID: {_currentEvent?.eventId}]");
        _currentEvent = null;
        
        // 맵으로 돌아가기
        if (MapPlayerTracker.Instance != null)
            MapPlayerTracker.Instance.OnEventComplete();
    }

    // 에디터 테스트용
    [ContextMenu("Test Random Stage Dialog Event")]
    void TestRandomEvent()
    {
        StartRandomEvent();
    }
    
    [ContextMenu("Test Event 1001")]
    void TestEvent1001()
    {
        StartEventById("1001");
    }
    
    [ContextMenu("Test Event 1002")]
    void TestEvent1002()
    {
        StartEventById("1002");
    }
    
    [ContextMenu("Test Event 1003")]
    void TestEvent1003()
    {
        StartEventById("1003");
    }
}