using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleStartButton : MonoBehaviour
{
    public Button startBtn;                        // 전투 시작 버튼
    public MultiPanelZoomController zoomCtrl;      // 방금 만든 컨트롤러

    void Start()
    {
        // 버튼 클릭 시 애니메이션 실행 연결
        startBtn.onClick.AddListener(OnStartBattle);
    }

    void OnStartBattle()
    {
        zoomCtrl.PlayBattleUIAnim();
    }
}
