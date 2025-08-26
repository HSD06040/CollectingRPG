using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfiniteScroll : MonoBehaviour
{
    [Header("Scroll Settings")]
    [SerializeField] ScrollRect scroll;
    [SerializeField] RectTransform content;
    [SerializeField] GameObject slotPrefab; //Content에 들어갈 슬롯 프리팹 (버튼, 텍스트 등)
    [SerializeField] int viewCnt = 10;   // 화면에 보이는 슬롯 개수
    [SerializeField] float slotH = 100f; // 슬롯 높이

    int dataCnt = 100; // 총 데이터 개수 (임시)
    int topIdx = 0;    // 현재 최상단 인덱스
    List<RectTransform> slots = new List<RectTransform>();

    void Start()
    {
        // 컨텐츠 높이 설정
        content.sizeDelta = new Vector2(content.sizeDelta.x, dataCnt * slotH);

        // 슬롯 미리 생성
        for (int i = 0; i < viewCnt + 2; i++)
        {
            var obj = Instantiate(slotPrefab, content);
            var rt = obj.GetComponent<RectTransform>();
            slots.Add(rt);
        }
    }

    void Update()
    {
        float scrollY = content.anchoredPosition.y;
        int newTop = Mathf.FloorToInt(scrollY / slotH);

        if (newTop != topIdx)
        {
            topIdx = newTop;
            UpdateSlots();
        }
    }

    void UpdateSlots() //스크롤 위치에 따라 슬롯 재배치 + 데이터 갱신
    {
        for (int i = 0; i < slots.Count; i++)
        {
            int idx = topIdx + i;
            if (idx < 0 || idx >= dataCnt)
            {
                slots[i].gameObject.SetActive(false);
                continue;
            }

            slots[i].gameObject.SetActive(true);
            slots[i].anchoredPosition = new Vector2(0, -idx * slotH);

            // 슬롯 안 텍스트 갱신 (예시)
            var txt = slots[i].GetComponentInChildren<Text>();
            if (txt != null) txt.text = "아이템 " + idx;
        }
    }
}
