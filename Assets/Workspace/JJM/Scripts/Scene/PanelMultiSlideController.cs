using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class PanelMultiSlideController : MonoBehaviour
{
    public RectTransform[] slideUpPanels;      // 기존 UI 패널들 (위로 올라감)
    public RectTransform[] slideInPanels;      // 새로 올라올 UI 패널들 (아래에서 올라옴)

    private Vector2[] slideUpOriginalPositions;
    private Vector2[] slideInOriginalPositions;

    public float slideDuration = 0.5f;
    public float slideOffset = 2280f;          // 화면 밖으로 이동할 오프셋

    public Button moveButton;
    public Button summonSceneButton;

    
    public Transform backgroundTransform; // Inspector에서 할당
    private Vector3 backgroundOriginalPosition;

    private Vector2[] originalPositions;
    private Vector2 summonSceneOriginalPos;

    public static bool IsBattleActive = false;
    void Awake()
    {
        // 기존 패널 원래 위치 저장
        slideUpOriginalPositions = new Vector2[slideUpPanels.Length];
        for (int i = 0; i < slideUpPanels.Length; i++)
            slideUpOriginalPositions[i] = slideUpPanels[i].anchoredPosition;

        // 새 패널 원래 위치 저장
        slideInOriginalPositions = new Vector2[slideInPanels.Length];
        for (int i = 0; i < slideInPanels.Length; i++)
        {
            slideInOriginalPositions[i] = slideInPanels[i].anchoredPosition;
            // 시작 위치를 아래로 오프셋
            slideInPanels[i].anchoredPosition = slideInOriginalPositions[i] - new Vector2(0, slideOffset);
            slideInPanels[i].gameObject.SetActive(false);
        }

        // 씬 로드 이벤트 등록
        SceneManager.sceneLoaded += OnSceneLoaded;

        if (backgroundTransform != null)
            backgroundOriginalPosition = backgroundTransform.position;


        // 버튼 이벤트 연결
        if (moveButton != null)
            moveButton.onClick.AddListener(SlideUpPanels);

        if (summonSceneButton != null)
            summonSceneButton.onClick.AddListener(SlideDownPanels);
    }
    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    // 씬이 로드될 때마다 백그라운드 오브젝트 자동 할당
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (backgroundTransform == null)
        {
            var bgMap = FindObjectOfType<AreaBackgroundMap>();
            if (bgMap != null)
            {
                backgroundTransform = bgMap.transform;
                backgroundOriginalPosition = backgroundTransform.position;
            }
        }
    }
    // 위로 슬라이드
    public void SlideUpPanels()
    {
        float delayStep = 0.08f;

        // 기존 패널 위로 슬라이드 & 비활성화
        for (int i = 0; i < slideUpPanels.Length; i++)
        {
            int idx = i; // 지역 변수로 캡처
            if (slideUpPanels[idx].gameObject.name == "WaitingBattlePlayer" ||
                slideUpPanels[idx].gameObject.name == "WaitingBattleEnemy")
            {
                // 위치 복구 코드도 삭제
                continue;
            }
            slideUpPanels[idx].DOKill();
            slideUpPanels[idx].DOAnchorPos(slideUpOriginalPositions[idx] + new Vector2(0, slideOffset), slideDuration)
                .SetEase(Ease.InOutQuad)
                .SetDelay(idx * delayStep)
                .OnComplete(() => slideUpPanels[idx].gameObject.SetActive(false));
        }

        // 새 패널 활성화 & 아래에서 위로 슬라이드
        for (int i = 0; i < slideInPanels.Length; i++)
        {
            int idx = i;
            // 전투 중이면 WaitingBattlePlayer, WaitingBattleEnemy 위치 건너뜀
            if (IsBattleActive &&
                (slideInPanels[idx].gameObject.name == "WaitingBattlePlayer" ||
                 slideInPanels[idx].gameObject.name == "WaitingBattleEnemy"))
            {
                // 위치를 원래대로 복구
                slideUpPanels[idx].anchoredPosition = slideUpOriginalPositions[idx];
                continue; 
            }

            slideInPanels[idx].gameObject.SetActive(true);
            slideInPanels[idx].DOKill();
            slideInPanels[idx].anchoredPosition = slideInOriginalPositions[idx] - new Vector2(0, slideOffset);
            slideInPanels[idx].DOAnchorPos(slideInOriginalPositions[idx], slideDuration)
                .SetEase(Ease.OutCubic)
                .SetDelay(idx * delayStep);
        }
        if (backgroundTransform != null)
        {
            Vector3 targetPos = new Vector3(backgroundOriginalPosition.x, 1.1f, backgroundOriginalPosition.z);
            backgroundTransform.DOMove(targetPos, slideDuration)
                .SetEase(Ease.OutCubic)
                .SetDelay((slideUpPanels.Length + 1) * delayStep);
        }
    }

    // 아래로 슬라이드(복귀)
    public void SlideDownPanels()
    {
        float delayStep = 0.08f;

        // 새 패널 아래로 슬라이드 & 비활성화
        for (int i = 0; i < slideInPanels.Length; i++)
        {
            int idx = i;
            slideInPanels[idx].DOKill();
            slideInPanels[idx].DOAnchorPos(slideInOriginalPositions[idx] - new Vector2(0, slideOffset), slideDuration)
                .SetEase(Ease.InOutQuad)
                .SetDelay(idx * delayStep)
                .OnComplete(() => slideInPanels[idx].gameObject.SetActive(false));
        }

        // 기존 패널 활성화 & 위에서 아래로 슬라이드
        for (int i = 0; i < slideUpPanels.Length; i++)
        {
            int idx = i;
            if (slideUpPanels[idx].gameObject.name == "WaitingBattlePlayer" ||
                slideUpPanels[idx].gameObject.name == "WaitingBattleEnemy")
            {
                // 위치 복구 코드도 삭제
                continue;
            }
            slideUpPanels[idx].gameObject.SetActive(true);
            slideUpPanels[idx].DOKill();
            slideUpPanels[idx].DOAnchorPos(slideUpOriginalPositions[idx], slideDuration)
                .SetEase(Ease.OutCubic)
                .SetDelay(idx * delayStep);
        }
        if (backgroundTransform != null)
        {
            backgroundTransform.DOMove(backgroundOriginalPosition, slideDuration)
                .SetEase(Ease.OutCubic)
                .SetDelay((slideUpPanels.Length + 1) * delayStep);
        }
    }
}