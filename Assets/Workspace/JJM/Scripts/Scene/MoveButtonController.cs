using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;


public class MoveButtonController : MonoBehaviour
{
    [Header("활성/비활성화할 오브젝트들")]
    public GameObject[] activateOnMove;
    public GameObject[] deactivateOnMove;

    [Header("Grid Transform")]
    public Transform gridTransform;

    [Header("MoveButton")]
    public Button moveButton;

    [Header("슬라이드 방향 선택")]
    public SlideDirection slideDirection = SlideDirection.Up;

    // 슬라이드 이동에 걸리는 시간(초)
    public float slideDuration = 0.5f;

    // 오브젝트별 원래 자리 저장용
    private Dictionary<GameObject, Vector2> originalPositions = new Dictionary<GameObject, Vector2>();
    public float slideStartOffset = 2340f; // 슬라이드 시작 위치 오프셋

    private Vector3 gridOriginalPosition; // Grid의 원래 위치 저장용

    private Dictionary<RectTransform, Vector2> parentOriginalPositions = new Dictionary<RectTransform, Vector2>();


    private void Awake()
    {
        if (moveButton == null)
            moveButton = GetComponent<Button>();

        moveButton.onClick.AddListener(OnMoveButtonClick);

        // Additive 씬 로드 후 Grid 자동 할당
        SceneManager.sceneLoaded += OnSceneLoaded;

        // 원래 자리 저장 (오프셋 적용 전, 반드시 활성화 상태에서)
        foreach (var go in activateOnMove)
        {
            var rect = go?.GetComponent<RectTransform>();
            if (rect != null)
            {
                originalPositions[go] = rect.anchoredPosition;
            }
        }
        foreach (var go in deactivateOnMove)
        {
            var rect = go?.GetComponent<RectTransform>();
            if (rect != null)
            {
                originalPositions[go] = rect.anchoredPosition;
            }
        }
        foreach (var go in activateOnMove)
        {
            var rect = go?.GetComponent<RectTransform>();
            if (rect != null)
            {
                originalPositions[go] = rect.anchoredPosition;
                var parentRect = rect.parent as RectTransform;
                if (parentRect != null && !parentOriginalPositions.ContainsKey(parentRect))
                    parentOriginalPositions[parentRect] = parentRect.anchoredPosition;
            }
        }
    }
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    private void Start()
    {
        // Grid 오브젝트를 이름으로 찾아서 할당 (씬에 "Grid"라는 이름이 있어야 함)
        if (gridTransform == null)
        {
            var gridObj = GameObject.Find("Grid");
            if (gridObj != null)
                gridTransform = gridObj.transform;
        }
        if (gridTransform != null)
        {
            gridOriginalPosition = gridTransform.position; // Grid의 원래 위치 저장
        }
        // 원래 자리 저장 & 오브젝트를 아래로 이동 후 비활성화
        foreach (var go in activateOnMove)
        {
            var rect = go?.GetComponent<RectTransform>();
            if (rect != null)
            {
                rect.anchoredPosition = originalPositions[go] - GetOffset(slideStartOffset);
                go.SetActive(false);
            }
        }
    }
    // Additive 씬이 로드될 때마다 Grid 오브젝트 자동 할당
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (gridTransform == null)
        {
            var gridObj = GameObject.Find("Grid");
            if (gridObj != null)
                gridTransform = gridObj.transform;
            else
                Debug.LogWarning("Grid 오브젝트를 찾을 수 없습니다. 이름이 'Grid'인지 확인하세요.");
        }
    }
    // 슬라이드 방향에 따라 오프셋 계산
    private Vector2 GetOffset(float offset)
    {
        switch (slideDirection)
        {
            case SlideDirection.Up: return new Vector2(0, offset);
            case SlideDirection.Down: return new Vector2(0, -offset);
            case SlideDirection.Left: return new Vector2(-offset, 0);
            case SlideDirection.Right: return new Vector2(offset, 0);
            default: return new Vector2(0, offset);
        }
    }
    private void OnMoveButtonClick()
    {
        float deactivateOffset = 2340f;
        int deactivateTotal = 0;
        int halfTriggered = 0;

        // 1. 비활성화 오브젝트 슬라이드 퇴장 (동시 실행)
        foreach (var go in deactivateOnMove)
        {
            if (go != null)
            {
                var rect = go.GetComponent<RectTransform>();
                if (rect != null)
                {
                    deactivateTotal++;
                    Vector2 endPos = rect.anchoredPosition + GetOffset(deactivateOffset);
                    bool halfDone = false;

                    Tweener tween = null; // 미리 선언
                    tween = rect.DOAnchorPos(endPos, slideDuration)
                        .SetEase(Ease.InCubic)
                        .OnUpdate(() =>
                        {
                            // 진행률 계산
                            float percent = tween.Elapsed() / tween.Duration();
                            if (!halfDone && percent >= 0.5f)
                            {
                                halfDone = true;
                                halfTriggered++;
                                if (halfTriggered == deactivateTotal)
                                    ActivatePanels();
                            }
                        })
                        .OnComplete(() =>
                        {
                            go.SetActive(false);
                        });
                }
                else
                {
                    go.SetActive(false);
                }
            }
        }

        // 만약 비활성화 대상이 없으면 바로 활성화
        if (deactivateTotal == 0)
            ActivatePanels();
    }

    // 활성화 오브젝트 슬라이드 등장 및 Grid 이동
    private void ActivatePanels()
    {
        foreach (var go in activateOnMove)
        {
            if (go != null)
            {
                var rect = go.GetComponent<RectTransform>();
                if (rect != null && originalPositions.ContainsKey(go))
                {
                    Vector2 targetPos = originalPositions[go];
                    go.SetActive(true);
                    rect.DOAnchorPos(targetPos, slideDuration).SetEase(Ease.OutCubic);
                }
                else
                {
                    go.SetActive(true);
                }
            }
        }

        // Grid 슬라이드 이동
        if (gridTransform != null)
        {
            Vector3 targetPos = new Vector3(gridTransform.position.x, 1.1f, gridTransform.position.z);
            gridTransform.DOMove(targetPos, slideDuration).SetEase(Ease.OutCubic);
        }
    }
    /// <summary>
    /// 모든 활성화/비활성화 오브젝트의 위치와 상태를 초기 UI 상태로 복원
    /// </summary>
    public void ResetUIPositions()
    {
        foreach (var kvp in parentOriginalPositions)
        {
            kvp.Key.anchoredPosition = kvp.Value;
        }
        // 활성화 오브젝트: 원래 자리로 이동 후 비활성화
        foreach (var go in activateOnMove)
        {
            var rect = go?.GetComponent<RectTransform>();
            if (rect != null && originalPositions.ContainsKey(go))
            {
                rect.DOKill(); // Tween 중지
                rect.anchoredPosition = originalPositions[go];
            }
            go.SetActive(false);
        }

        // 비활성화 오브젝트: 원래 자리로 이동 후 비활성화
        foreach (var go in deactivateOnMove)
        {
            var rect = go?.GetComponent<RectTransform>();
            if (rect != null && originalPositions.ContainsKey(go))
            {
                rect.DOKill(); // Tween 중지
                rect.anchoredPosition = originalPositions[go];
            }
            go.SetActive(false);
        }
        
        // Grid 위치도 원래 자리로 복원 (필요시)
        if (gridTransform != null)
        {
            gridTransform.position = gridOriginalPosition;
        }
    }
}

