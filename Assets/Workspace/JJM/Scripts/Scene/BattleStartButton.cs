using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleStartButton : MonoBehaviour
{
    public RectTransform playerBatchUI;
    public RectTransform enemyBatchUI;
    public RectTransform waitingBattlePlayer;
    public RectTransform waitingBattleEnemy;

    public GameObject waitingBattleUI;
    public GameObject fightSceneUI;
    public GameObject skillUI;
    public GameObject enemyInfoMove;
    public GameObject summonSceneMovePlayer;
    public GameObject restMove;
    public GameObject summonSceneMoveMonster;


    // 줌 애니메이션용
    private Vector3 playerBatchOriginScale;
    private Vector3 enemyBatchOriginScale;
    private Vector2 waitingBattlePlayerOriginPos;
    private Vector2 waitingBattleEnemyOriginPos;

    public Button battleStartButton;

    public GameObject fightButtonS;   // 항상 활성화
    public GameObject fightButton;    // 활성/비활성 전환용

    //전투 연출용 위치 저장
    private Vector2 waitingBattlePlayerBattlePos;
    private Vector2 waitingBattleEnemyBattlePos;
    private Vector3 playerBatchZoomScale;
    private Vector3 enemyBatchZoomScale;
    private Transform fightButtonSOriginalParent;

    public float offScreenX = 3000f; // 화면 밖 X값 (Inspector에서 조정 가능)
    public float batchSlideDuration = 3f;  // 슬라이드 시간 (Inspector에서 조정 가능)

    public EnemyRestSlideController enemyRestSlideController;


    void Awake()
    {
        // 원래 값 저장
        playerBatchOriginScale = playerBatchUI.localScale;
        enemyBatchOriginScale = enemyBatchUI.localScale;
        waitingBattlePlayerOriginPos = waitingBattlePlayer.anchoredPosition;
        waitingBattleEnemyOriginPos = waitingBattleEnemy.anchoredPosition;
        fightButtonSOriginalParent = fightButtonS.transform.parent;
        waitingBattlePlayerOriginPos = new Vector2(0f, 0f); 

        // 배틀 연출용 스케일 (작게)
        playerBatchZoomScale = playerBatchOriginScale * 0.5f;
        enemyBatchZoomScale = enemyBatchOriginScale * 0.5f;

        // 배틀 연출용 위치 (중앙 기준, 좌우로 이동)
        float gridOffsetX = 300f; // 사진2 기준, 실제 UI에 맞게 조정

        

        // y값을 동일하게 맞춤 (예: waitingBattlePlayerOriginPos.y 사용)
        float battleY = 0f;     // 화면 중앙에 가깝게 조정

        waitingBattlePlayerBattlePos = new Vector2(-gridOffsetX, battleY);
        waitingBattleEnemyBattlePos = new Vector2(gridOffsetX, battleY);

        battleStartButton.onClick.AddListener(OnBattleStart);
    }

    // 전투 종료 시 호출
    public void OnBattleEnd()
    {
        PanelMultiSlideController.IsBattleActive = false; // 전투 상태 비활성화

        // FightButtonS는 항상 활성화
        fightButtonS.SetActive(true);
        // FightButton만 활성화
        fightButton.SetActive(true);

        // 배치칸 스케일/위치 복원
        playerBatchUI.DOKill();
        enemyBatchUI.DOKill();
        playerBatchUI.localScale = playerBatchOriginScale; // 플레이어 배치 스케일 복원
        enemyBatchUI.localScale = enemyBatchOriginScale;   // 적 배치 스케일 복원
        playerBatchUI.anchoredPosition = Vector2.zero;     // 플레이어 배치 위치 복원 (중앙)
        enemyBatchUI.anchoredPosition = Vector2.zero;      // 적 배치 위치 복원 (중앙)

        // 대기 UI, 전투 UI, 스킬 UI 등 복원
        waitingBattleUI.SetActive(true);       // 대기 UI 활성화
        fightSceneUI.SetActive(false);         // 전투 UI 비활성화
        skillUI.SetActive(false);              // 스킬 UI 비활성화
        enemyInfoMove.SetActive(true);         // 적 정보 UI 활성화
        summonSceneMovePlayer.SetActive(true); // 소환 UI 활성화
        restMove.SetActive(true);              // 휴식 UI 활성화
        summonSceneMoveMonster.SetActive(true);// 몬스터 소환 UI 활성화

        // FightButtonS 부모 복귀 후 위치 복원
        fightButtonS.transform.SetParent(fightButtonSOriginalParent, true);
        var fightButtonSRect = fightButtonS.GetComponent<RectTransform>();
        if (fightButtonSRect != null)
        {
            fightButtonSRect.anchoredPosition = Vector2.zero; // 원래 위치값
        }

        // 플레이어/적 배치 오브젝트 활성화 상태 복원
        waitingBattlePlayer.gameObject.SetActive(true);
        waitingBattleEnemy.gameObject.SetActive(false);
    }

    public void OnBattleStart()
    {
        // 항상 플레이어 위치에서 시작
        if (enemyRestSlideController != null)
            enemyRestSlideController.RestMoveToPlayer();

        PanelMultiSlideController.IsBattleActive = true; // 전투 상태 활성화

        waitingBattlePlayer.DOKill(); // 플레이어 배치 트윈 중지
        waitingBattleEnemy.DOKill();  // 적 배치 트윈 중지

        waitingBattlePlayer.gameObject.SetActive(true); // 플레이어 배치 활성화
        waitingBattleEnemy.gameObject.SetActive(true);  // 적 배치 활성화

        fightButtonS.SetActive(true); // S버튼 항상 활성화
        fightButton.SetActive(false); // 일반 버튼 비활성화

        waitingBattleUI.SetActive(false); // 대기 UI 비활성화
        fightButtonS.transform.SetParent(null, true);   // S버튼 부모 분리
        fightButtonS.SetActive(true);                   // S버튼 활성화

        fightSceneUI.SetActive(true);   // 전투 UI 활성화
        skillUI.SetActive(true);        // 스킬 UI 활성화
        enemyInfoMove.SetActive(false); // 적 정보 UI 비활성화
        summonSceneMovePlayer.SetActive(false); // 소환 UI 비활성화
        restMove.SetActive(false);      // 휴식 UI 비활성화
        summonSceneMoveMonster.SetActive(false); // 몬스터 소환 UI 비활성화

        // 줌 애니메이션 시퀀스 생성
        Sequence zoomSeq = DOTween.Sequence();
        zoomSeq.Append(playerBatchUI.DOScale(playerBatchZoomScale, 0.3f).SetEase(Ease.InOutCubic)); // 플레이어 배치 줌
        zoomSeq.Join(enemyBatchUI.DOScale(enemyBatchZoomScale, 0.3f).SetEase(Ease.InOutCubic));     // 적 배치 줌

        // 줌 끝나면 슬라이드 연출 시작
        zoomSeq.OnComplete(() =>
        {
            float startY = playerBatchUI.anchoredPosition.y; // 현재 Y값 저장
            float leftOutX = -offScreenX; // 왼쪽 화면 밖 X값
            float rightOutX = offScreenX; // 오른쪽 화면 밖 X값
            float centerX = waitingBattlePlayerOriginPos.x; // 플레이어의 원래 X 위치
            float enemyTargetX = waitingBattleEnemyOriginPos.x; // 적의 원래 X 위치


            // 플레이어 배치: 중앙에서 왼쪽 화면 밖으로 이동
            playerBatchUI.anchoredPosition = new Vector2(centerX, startY); // 중앙 위치로 세팅
                                                                           // 적 배치: 오른쪽 화면 밖에서 중앙으로 이동
            enemyBatchUI.anchoredPosition = new Vector2(rightOutX, startY); // 오른쪽 밖 위치로 세팅

            // 슬라이드 애니메이션 시퀀스 생성
            Sequence batchSlideSeq = DOTween.Sequence();
            batchSlideSeq.Append(playerBatchUI.DOAnchorPos(new Vector2(leftOutX, startY), batchSlideDuration).SetEase(Ease.InOutCubic)); // 플레이어 배치 왼쪽 밖으로 슬라이드
            batchSlideSeq.Join(enemyBatchUI.DOAnchorPos(new Vector2(enemyTargetX, startY), batchSlideDuration).SetEase(Ease.InOutCubic));     // 적 배치 중앙으로 슬라이드

            // 슬라이드 끝나면 전투 종료 처리
            batchSlideSeq.OnComplete(() =>
            {
                OnBattleEnd(); // 전투 종료 함수 호출
            });
        });
        //// 3초 후 자동 전투 종료
        //StartCoroutine(EndBattleAfterDelay(3f));
    }
    ////테스트용 배틀종료
    //private IEnumerator EndBattleAfterDelay(float delay)
    //{
    //    yield return new WaitForSeconds(delay);
    //    OnBattleEnd();
    //}
    // EnemyRestSlideController.cs
    
}
