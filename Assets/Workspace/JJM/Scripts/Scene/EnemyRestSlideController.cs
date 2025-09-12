using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class EnemyRestSlideController : MonoBehaviour
{
    public RectTransform waitingBattlePlayer;
    public RectTransform waitingBattleEnemy;
    public Button enemyInfoMoveButton;
    public Button restMoveButton;

    // 플레이어/적 패널의 원래 위치 (중앙 기준)
    // 중앙 위치(Inspector에서 직접 할당하거나 Awake에서 저장)
    private Vector2 playerOriginPos;
    private Vector2 enemyOriginPos;

    // 화면 밖 X 위치 (애니메이션 이동 거리, 해상도에 따라 조정)
    public float offScreenX = 800f;
    // 슬라이드 애니메이션 지속 시간(초)
    public float slideDuration = 0.4f;

    private float topY = 33f; // 패널이 슬라이드될 Y 위치 (UI 기준, 상단에 고정)

    /// <summary>
    /// 컴포넌트 초기화 및 버튼 이벤트 연결
    /// </summary>
    void Awake()
    {
        // 패널의 원래 위치 저장
        playerOriginPos = waitingBattlePlayer.anchoredPosition;
        enemyOriginPos = waitingBattleEnemy.anchoredPosition;

        // 버튼 클릭 이벤트 등록
        enemyInfoMoveButton.onClick.AddListener(OnEnemyInfoMove);
        restMoveButton.onClick.AddListener(OnRestMove);
    }


    /// <summary>
    ///  EnemyInfoMove 버튼 클릭 시
    /// - 플레이어 패널은 왼쪽 화면 밖으로, 적 패널은 중앙으로 슬라이드
    /// </summary>
    private void OnEnemyInfoMove()
    {
        // 기존 트윈 중지
        waitingBattlePlayer.DOKill();
        waitingBattleEnemy.DOKill();

        // 패널 활성화
        waitingBattlePlayer.gameObject.SetActive(true);
        waitingBattleEnemy.gameObject.SetActive(true);

        // 패널 위치를 Y(topY)로 강제 설정
        waitingBattlePlayer.anchoredPosition = new Vector2(playerOriginPos.x, topY);
        waitingBattleEnemy.anchoredPosition = new Vector2(offScreenX, topY);

        // 슬라이드 애니메이션 시퀀스
        Sequence seq = DOTween.Sequence();
        // 적 패널: 오른쪽 밖에서 중앙으로 이동
        seq.Append(waitingBattleEnemy.DOAnchorPos(new Vector2(enemyOriginPos.x, topY), slideDuration).SetEase(Ease.OutCubic));
        // 플레이어 패널: 중앙에서 왼쪽 밖으로 이동
        seq.Join(waitingBattlePlayer.DOAnchorPos(new Vector2(-offScreenX, topY), slideDuration).SetEase(Ease.OutCubic));
    }

    
    /// <summary>
    /// RestMove 버튼 클릭 시
    /// - 플레이어 패널은 중앙으로, 적 패널은 오른쪽 화면 밖으로 슬라이드
    /// </summary>
    private void OnRestMove()
    {
        // 기존 트윈 중지
        waitingBattlePlayer.DOKill();
        waitingBattleEnemy.DOKill();

        // 패널 활성화
        waitingBattlePlayer.gameObject.SetActive(true);
        waitingBattleEnemy.gameObject.SetActive(true);

        // 패널 위치를 Y(topY)로 강제 설정
        waitingBattlePlayer.anchoredPosition = new Vector2(-offScreenX, topY);
        waitingBattleEnemy.anchoredPosition = new Vector2(enemyOriginPos.x, topY);

        // 슬라이드 애니메이션 시퀀스
        Sequence seq = DOTween.Sequence();
        // 적 패널: 중앙에서 오른쪽 밖으로 이동
        seq.Append(waitingBattleEnemy.DOAnchorPos(new Vector2(offScreenX, topY), slideDuration).SetEase(Ease.OutCubic));
        // 플레이어 패널: 왼쪽 밖에서 중앙으로 이동
        seq.Join(waitingBattlePlayer.DOAnchorPos(new Vector2(playerOriginPos.x, topY), slideDuration).SetEase(Ease.OutCubic));
    }
    /// <summary>
    /// 외부에서 플레이어 대기 위치로 슬라이드할 때 호출
    /// </summary>
    public void RestMoveToPlayer()
    {
        OnRestMove();
    }
}
