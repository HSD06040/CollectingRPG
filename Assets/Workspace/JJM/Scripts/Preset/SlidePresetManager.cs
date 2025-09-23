using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SlidePresetManager : MonoBehaviour
{
    [Header("원래 편성 유닛 부모")]
    public Transform unitParent; // SelectedCharacterUnit들이 자식

    [Header("걸어오는 유닛 부모")]
    public Transform walkingUnitParent; // 걷는 유닛 임시 부모

    [Header("유닛이 왼쪽에서 시작할 오프셋")]
    public float unitOffScreenX = 4f; // 월드 좌표 기준 오프셋

    [Header("유닛이 자리로 이동하는 시간(초)")]
    public float walkDuration = 0.7f;

    [Header("프리셋 데이터베이스")]
    public PresetDatabase presetDB;

    private void Awake()
    {
        if (presetDB == null)
            presetDB = DataManager.Instance.PresetDB;
    }

    // 프리셋 변경 시 호출
    public void ShowPresetCharactersByIndex(int presetIndex)
    {
        Debug.Log($"ShowPresetCharactersByIndex 호출: {presetIndex}");
        if (presetDB == null) { Debug.Log("presetDB가 null입니다."); return; }
        if (presetIndex < 0 || presetIndex >= presetDB.PresetData.Count) { Debug.Log("presetIndex 범위 오류"); return; }
        ShowPresetCharacters(presetDB.PresetData[presetIndex]);
    }

    public void ShowPresetCharacters(TeamPresetData presetData)
    {
        for (int i = 0; i < unitParent.childCount; i++)
            unitParent.GetChild(i).gameObject.SetActive(false);

        for (int i = walkingUnitParent.childCount - 1; i >= 0; i--)
            Destroy(walkingUnitParent.GetChild(i).gameObject);

        int unitCount = Mathf.Min(presetData.Statuses.Length, unitParent.childCount);
        int finishedCount = 0;

        Canvas canvas = unitParent.GetComponentInParent<Canvas>();
        Camera spumCamera = GameObject.Find("SPUMCamera").GetComponent<Camera>();

        for (int i = 0; i < unitCount; i++)
        {
            var status = presetData.Statuses[i];
            if (status == null || status.Data == null || status.Data.UnitPrefab == null)
                continue;

            var targetRect = unitParent.GetChild(i).GetComponent<RectTransform>();
            Vector3 targetPos;

            if (canvas != null && canvas.renderMode == RenderMode.ScreenSpaceOverlay)
            {
                targetPos = spumCamera.ScreenToWorldPoint(targetRect.position);
                targetPos.z = 0;
            }
            else
            {
                targetPos = targetRect.TransformPoint(targetRect.anchoredPosition);
                targetPos.z = 0;
            }

            var walkingUnit = Instantiate(status.Data.UnitPrefab, walkingUnitParent);
            Vector3 startPos = targetPos - new Vector3(unitOffScreenX, 0, 0);
            startPos.z = 0;
            walkingUnit.transform.position = startPos;
            Debug.Log($"걷는 프리팹 생성: {walkingUnit.name}, 시작 위치: {walkingUnit.transform.position}, 목표 위치: {targetPos}");

            walkingUnit.transform.DOMove(targetPos, walkDuration)
                .SetEase(Ease.OutCubic)
                .OnComplete(() =>
                {
                    Debug.Log($"걷는 프리팹 도착: {walkingUnit.name}, 위치: {walkingUnit.transform.position}");
                    finishedCount++;
                    if (finishedCount == unitCount)
                    {
                        Debug.Log("모든 걷는 프리팹 도착, 기존 유닛 활성화 및 걷는 프리팹 삭제");
                        for (int j = 0; j < unitCount; j++)
                            unitParent.GetChild(j).gameObject.SetActive(true);
                        for (int j = walkingUnitParent.childCount - 1; j >= 0; j--)
                            Destroy(walkingUnitParent.GetChild(j).gameObject);
                    } 
                });
        }
    }
    //public void ShowPresetCharacters(TeamPresetData presetData)
    //{
    //    // 1. 기존 유닛 오브젝트 모두 비활성화
    //    for (int i = 0; i < unitParent.childCount; i++)
    //        unitParent.GetChild(i).gameObject.SetActive(false);

    //    // 2. 기존 걷는 유닛 오브젝트 모두 삭제
    //    for (int i = walkingUnitParent.childCount - 1; i >= 0; i--)
    //        Destroy(walkingUnitParent.GetChild(i).gameObject);

    //    // 3. 프리셋에 맞게 걷는 유닛 오브젝트 생성 및 연출
    //    int unitCount = Mathf.Min(presetData.Statuses.Length, unitParent.childCount);
    //    int finishedCount = 0;

    //    for (int i = 0; i < unitCount; i++)
    //    {
    //        // 원래 유닛 자리 위치
    //        var targetRect = unitParent.GetChild(i).GetComponent<RectTransform>();
    //        Vector2 targetPos = targetRect.anchoredPosition;

    //        // 걷는 유닛 오브젝트 생성
    //        var walkingUnit = Instantiate(walkingUnitPrefab, walkingUnitParent);
    //        var walkingRect = walkingUnit.GetComponent<RectTransform>();
    //        walkingRect.anchoredPosition = targetPos - new Vector2(unitOffScreenX, 0);

    //        // 걷는 유닛 정보 세팅 (이미지 등)
    //        // 예시: walkingUnit.GetComponent<UnitUI>().SetStatus(presetData.Statuses[i]);

    //        // 걷기 애니메이션 트리거
    //        var animator = walkingUnit.GetComponent<Animator>();
    //        if (animator != null)
    //            animator.SetTrigger("Move");

    //        // 이동 연출
    //        walkingRect.DOAnchorPos(targetPos, walkDuration)
    //            .SetEase(Ease.OutCubic)
    //            .OnComplete(() =>
    //            {
    //                if (animator != null)
    //                    animator.SetTrigger("Idle");

    //                finishedCount++;
    //                // 모든 걷는 유닛이 도착하면 원래 유닛 활성화 & 걷는 유닛 삭제
    //                if (finishedCount == unitCount)
    //                {
    //                    for (int j = 0; j < unitCount; j++)
    //                        unitParent.GetChild(j).gameObject.SetActive(true);

    //                    for (int j = walkingUnitParent.childCount - 1; j >= 0; j--)
    //                        Destroy(walkingUnitParent.GetChild(j).gameObject);
    //                }
    //            });
    //    }
    //}
}



