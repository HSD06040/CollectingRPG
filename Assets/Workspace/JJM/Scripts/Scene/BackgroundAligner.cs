using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundAligner : MonoBehaviour
{
    public string batchPanelName = "BatchPanel";      // 배치칸 UI 오브젝트 이름
    public string backgroundPanelName = "BackgroundPanel"; // 배경 하얀 타일 오브젝트 이름

    void Start()
    {
        // 서로 다른 씬에서 오브젝트 찾기
        var batchObj = GameObject.Find(batchPanelName);
        var backgroundObj = GameObject.Find(backgroundPanelName);

        if (batchObj != null && backgroundObj != null)
        {
            var batchRect = batchObj.GetComponent<RectTransform>();
            var backgroundRect = backgroundObj.GetComponent<RectTransform>();

            // 배치칸의 위치와 크기를 배경에 적용
            backgroundRect.position = batchRect.position;
            backgroundRect.sizeDelta = batchRect.sizeDelta;
        }
        else
        {
            Debug.LogError("배치칸 또는 배경 오브젝트를 찾을 수 없습니다.");
        }
    }
}
