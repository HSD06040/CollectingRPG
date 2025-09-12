using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

/// <summary>
/// 여러 UI 패널의 슬라이드 애니메이션 및 배경 이동을 관리하는 컨트롤러
/// 플레이어/적 각각의 패널 전환을 지원
/// </summary>
public class PanelMultiSlideController : MonoBehaviour
{
    public RectTransform[] slideUpPanels;      // 플레이어용: 위로 사라질 패널들
    public RectTransform[] slideInPanels;      // 플레이어용: 아래에서 올라올 패널들

    private Vector2[] slideUpOriginalPositions;
    private Vector2[] slideInOriginalPositions;

    public RectTransform[] enemySlideUpPanels;    // 적용: 위로 사라질 패널들
    public RectTransform[] enemySlideInPanels;    // 적용: 아래에서 올라올 패널들

    private Vector2[] enemySlideUpOriginalPositions;
    private Vector2[] enemySlideInOriginalPositions;

    public float slideDuration = 0.5f;          // 슬라이드 애니메이션 지속 시간(초)
    public float slideOffset = 2280f;           // 패널이 이동할 오프셋(화면 밖 위치)

    // UI 버튼
    public Button moveButton;
    public Button summonSceneButton;

    // 배경 오브젝트(Inspector에서 할당)
    public Transform backgroundTransform; 
    private Vector3 backgroundOriginalPosition;

    private Vector2[] originalPositions;
    private Vector2 summonSceneOriginalPos;

    public static bool IsBattleActive = false; // 전투 중 여부(패널 전환 예외 처리용)

    /// <summary>
    /// 패널 원래 위치 저장 및 초기화, 버튼 이벤트 연결
    /// </summary>
    void Awake()
    {
        // 플레이어 패널 원래 위치 저장 및 초기화
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

        // 적 패널 원래 위치 저장 및 초기화
        if (enemySlideUpPanels != null)
        {
            enemySlideUpOriginalPositions = new Vector2[enemySlideUpPanels.Length];
            for (int i = 0; i < enemySlideUpPanels.Length; i++)
            {
                enemySlideUpOriginalPositions[i] = enemySlideUpPanels[i].anchoredPosition;
                // 시작 시 아래로 오프셋 + 비활성화
                enemySlideUpPanels[i].anchoredPosition = enemySlideUpOriginalPositions[i] - new Vector2(0, slideOffset);
                enemySlideUpPanels[i].gameObject.SetActive(false);
            }
        }
        if (enemySlideInPanels != null)
        {
            enemySlideInOriginalPositions = new Vector2[enemySlideInPanels.Length];
            for (int i = 0; i < enemySlideInPanels.Length; i++)
            {
                enemySlideInOriginalPositions[i] = enemySlideInPanels[i].anchoredPosition;
                // SummonScene은 원래 위치에서 활성화
                enemySlideInPanels[i].anchoredPosition = enemySlideInOriginalPositions[i];
                enemySlideInPanels[i].gameObject.SetActive(true);
            }
        }

        // 씬 로드 이벤트 등록(배경 자동 할당)
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
    /// <summary>
    /// 씬이 로드될 때마다 배경 오브젝트 자동 할당
    /// </summary>
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
    /// <summary>
    /// 플레이어용: 기존 패널 위로 슬라이드(사라짐), 새 패널 아래에서 슬라이드(등장)
    /// 배경도 y=1.1로 이동
    /// </summary>
    public void SlideUpPanels()
    {
        float delayStep = 0.08f;

        // 기존 패널 위로 슬라이드 & 비활성화
        for (int i = 0; i < slideUpPanels.Length; i++)
        {
            int idx = i; 
            if (slideUpPanels[idx].gameObject.name == "WaitingBattlePlayer" ||
                slideUpPanels[idx].gameObject.name == "WaitingBattleEnemy")
            {
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
            if (IsBattleActive &&
                (slideInPanels[idx].gameObject.name == "WaitingBattlePlayer" ||
                 slideInPanels[idx].gameObject.name == "WaitingBattleEnemy"))
            {
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
        // 배경도 y=1.1로 이동
        if (backgroundTransform != null)
        {
            Vector3 targetPos = new Vector3(backgroundOriginalPosition.x, 1.1f, backgroundOriginalPosition.z);
            backgroundTransform.DOMove(targetPos, slideDuration)
                .SetEase(Ease.OutCubic)
                .SetDelay((slideUpPanels.Length + 1) * delayStep);
        }
    }

    /// <summary>
    /// 플레이어용: 새 패널 아래로 슬라이드(사라짐), 기존 패널 위에서 아래로 슬라이드(복귀)
    /// 배경도 원래 위치로 복귀
    /// </summary>
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
                continue;
            }
            slideUpPanels[idx].gameObject.SetActive(true);
            slideUpPanels[idx].DOKill();
            slideUpPanels[idx].DOAnchorPos(slideUpOriginalPositions[idx], slideDuration)
                .SetEase(Ease.OutCubic)
                .SetDelay(idx * delayStep);
        }
        // 배경 원래 위치로 복귀
        if (backgroundTransform != null)
        {
            backgroundTransform.DOMove(backgroundOriginalPosition, slideDuration)
                .SetEase(Ease.OutCubic)
                .SetDelay((slideUpPanels.Length + 1) * delayStep);
        }
    }
    /// <summary>
    /// 적용: 기존 패널 아래로 슬라이드(사라짐), 새 패널 위에서 아래로 슬라이드(등장)
    /// 배경도 y=1.1 또는 원하는 값으로 이동
    /// </summary>
    public void SlideEnemyToSummonScene()
    {
        float delayStep = 0.08f;
        // 기존 패널 아래로 슬라이드 & 비활성화
        for (int i = 0; i < enemySlideUpPanels.Length; i++)
        {
            int idx = i;
            enemySlideUpPanels[idx].DOKill();
            enemySlideUpPanels[idx].DOAnchorPos(enemySlideUpOriginalPositions[idx] - new Vector2(0, slideOffset), slideDuration)
                .SetEase(Ease.InOutQuad)
                .SetDelay(idx * delayStep)
                .OnComplete(() => enemySlideUpPanels[idx].gameObject.SetActive(false));
        }
        // 새 패널 위에서 아래로 슬라이드 & 활성화
        for (int i = 0; i < enemySlideInPanels.Length; i++)
        {
            int idx = i;
            enemySlideInPanels[idx].gameObject.SetActive(true);
            enemySlideInPanels[idx].DOKill();
            enemySlideInPanels[idx].anchoredPosition = enemySlideInOriginalPositions[idx] + new Vector2(0, slideOffset);
            enemySlideInPanels[idx].DOAnchorPos(enemySlideInOriginalPositions[idx], slideDuration)
                .SetEase(Ease.OutCubic)
                .SetDelay(idx * delayStep);
        }
        // 배경도 y값 1.1로 이동
        if (backgroundTransform != null)
        {
            Vector3 targetPos = new Vector3(backgroundOriginalPosition.x, 2.7f, backgroundOriginalPosition.z);
            backgroundTransform.DOKill();
            backgroundTransform.DOMove(targetPos, slideDuration)
                .SetEase(Ease.OutCubic)
                .SetDelay((enemySlideUpPanels.Length + 1) * delayStep);
        }
    }


}