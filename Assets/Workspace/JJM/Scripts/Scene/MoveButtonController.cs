using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MoveButtonController : MonoBehaviour
{
    [Header("활성/비활성화할 오브젝트들")]
    public GameObject[] activateOnMove;
    public GameObject[] deactivateOnMove;

    [Header("Grid Transform")]
    public Transform gridTransform;

    [Header("MoveButton")]
    public Button moveButton;

    // 슬라이드 이동에 걸리는 시간(초)
    public float slideDuration = 0.5f;

    // 오브젝트별 원래 자리 저장용
    private Dictionary<GameObject, Vector2> originalPositions = new Dictionary<GameObject, Vector2>();
    public float slideStartOffset = 2340f; // 슬라이드 시작 위치 오프셋

    private void Awake()
    {
        if (moveButton == null)
            moveButton = GetComponent<Button>();

        moveButton.onClick.AddListener(OnMoveButtonClick);
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
        // 원래 자리 저장 & 오브젝트를 아래로 이동 후 비활성화
        foreach (var go in activateOnMove)
        {
            var rect = go?.GetComponent<RectTransform>();
            if (rect != null)
            {
                originalPositions[go] = rect.anchoredPosition; // 원래 자리 저장
                rect.anchoredPosition = rect.anchoredPosition - new Vector2(0, slideStartOffset); // 아래로 이동
                go.SetActive(false); // 비활성화
            }
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
                    Vector2 endPos = rect.anchoredPosition + new Vector2(0, deactivateOffset);
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
}

