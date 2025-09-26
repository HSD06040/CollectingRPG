using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SlidePresetManager : MonoBehaviour
{
    [Header("원래 편성 유닛 부모")]
    public Transform unitParent;

    [Header("유닛이 왼쪽에서 시작할 오프셋")]
    public float unitOffScreenX = 4f;

    [Header("유닛이 자리로 이동하는 시간(초)")]
    public float walkDuration = 0.7f;

    [Header("프리셋 데이터베이스")]
    public PresetDatabase presetDB;

    [Header("월드 유닛을 UI에 표시할 RawImage")]
    public RawImage worldUnitRawImage; // Canvas에 추가한 RawImage

    [Header("월드 유닛을 렌더링할 카메라")]
    public Camera spumCamera; // SPUMCamera

    [Header("월드 유닛을 렌더링할 RenderTexture")]
    public RenderTexture spumRenderTexture; // SPUM_UITexture

    private void Awake()
    {
        if (presetDB == null)
            presetDB = DataManager.Instance.PresetDB;
        // SPUMCamera에 RenderTexture 연결
        if (spumCamera != null && spumRenderTexture != null)
            spumCamera.targetTexture = spumRenderTexture;

        // RawImage에 RenderTexture 연결
        if (worldUnitRawImage != null && spumRenderTexture != null)
            worldUnitRawImage.texture = spumRenderTexture;
    }

    // 프리셋 변경 시 호출
    public void ShowPresetCharactersByIndex(int presetIndex)
    {
        if (presetDB == null) return;
        if (presetIndex < 0 || presetIndex >= presetDB.PresetData.Count) return;

        TeamPresetData presetData = presetDB.PresetData[presetIndex];
        ShowPresetCharacters(presetData);
    }

    public void ShowPresetCharacters(TeamPresetData presetData)
    {
        // 월드에 있는 TestUnit 오브젝트 모두 찾기
        var testUnits = GameObject.FindObjectsOfType<GameObject>();
        int finishedCount = 0;

        foreach (var obj in testUnits)
        {
            if (!obj.name.StartsWith("TestUnit"))
                continue;
             
            // 목표 위치
            Vector3 targetPos = obj.transform.position;
            if (Mathf.Abs(targetPos.x) > 20 || Mathf.Abs(targetPos.y) > 20)
                targetPos = new Vector3(0, 0, 0);

            // 시작 위치(왼쪽 오프셋)
            Vector3 startPos = targetPos - new Vector3(unitOffScreenX, 0, 0);

            // 시작 위치로 이동
            obj.transform.position = startPos;
            obj.SetActive(true);

            // 걷는 연출
            obj.transform.DOMove(targetPos, walkDuration)
                .SetEase(Ease.OutCubic)
                .OnComplete(() =>
                {
                    finishedCount++;
                    // 필요시 모든 연출이 끝난 후 추가 처리
                });
        }
    }
}



