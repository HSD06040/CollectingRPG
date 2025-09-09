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
        PanelMultiSlideController.IsBattleActive = false;

        // FightButtonS는 항상 활성화
        fightButtonS.SetActive(true);
        // FightButton만 활성화
        fightButton.SetActive(true);

        // 현재 위치 기준으로 복원
        Vector2 currentPlayerPos = waitingBattlePlayer.anchoredPosition;
        Vector2 currentEnemyPos = waitingBattleEnemy.anchoredPosition;

        Sequence endSequence = DOTween.Sequence();
        endSequence.Append(playerBatchUI.DOScale(playerBatchOriginScale, 0.3f).SetEase(Ease.OutCubic));
        endSequence.Join(enemyBatchUI.DOScale(enemyBatchOriginScale, 0.3f).SetEase(Ease.OutCubic));

        // X만 원래대로, Y는 현재값 유지
        Vector2 playerOriginPos = new Vector2(waitingBattlePlayerOriginPos.x, currentPlayerPos.y);
        Vector2 enemyOriginPos = new Vector2(waitingBattleEnemyOriginPos.x, currentEnemyPos.y);


        endSequence.Append(waitingBattlePlayer.DOAnchorPos(waitingBattlePlayerOriginPos, 0.3f).SetEase(Ease.OutCubic));
        endSequence.Join(waitingBattleEnemy.DOAnchorPos(waitingBattleEnemyOriginPos, 0.3f).SetEase(Ease.OutCubic));

        endSequence.OnComplete(() =>
        {
            waitingBattlePlayer.DOKill();
            waitingBattleEnemy.DOKill();
            waitingBattlePlayer.anchoredPosition = waitingBattlePlayerOriginPos;
            waitingBattleEnemy.anchoredPosition = waitingBattleEnemyOriginPos;

            // PlayerBatch, EnemyBatch 스케일/위치 복원
            playerBatchUI.localScale = playerBatchOriginScale;
            enemyBatchUI.localScale = enemyBatchOriginScale;
            playerBatchUI.anchoredPosition = playerBatchUI.anchoredPosition; // 필요시 원래 위치로
            enemyBatchUI.anchoredPosition = enemyBatchUI.anchoredPosition;   // 필요시 원래 위치로

            // FightButtonS 부모 복귀 후 위치 복원
            fightButtonS.transform.SetParent(fightButtonSOriginalParent, true);

            // FightButtonS의 RectTransform 위치를 원래대로 복원
            var fightButtonSRect = fightButtonS.GetComponent<RectTransform>();
            if (fightButtonSRect != null)
            {
                fightButtonSRect.anchoredPosition = Vector2.zero; // 또는 Inspector에서 확인한 원래 위치값
            }

            waitingBattleUI.SetActive(true);
            fightSceneUI.SetActive(false);
            skillUI.SetActive(false);
            enemyInfoMove.SetActive(true);
            summonSceneMovePlayer.SetActive(true);
            restMove.SetActive(true);
            summonSceneMoveMonster.SetActive(true);

            waitingBattlePlayer.gameObject.SetActive(true);
            waitingBattleEnemy.gameObject.SetActive(false);
            
        });
    }

    void OnBattleStart()
    {
        PanelMultiSlideController.IsBattleActive = true;

        waitingBattlePlayer.DOKill();
        waitingBattleEnemy.DOKill();

        // 반드시 활성화!
        waitingBattlePlayer.gameObject.SetActive(true);
        waitingBattleEnemy.gameObject.SetActive(true);

        // FightButtonS는 항상 활성화
        fightButtonS.SetActive(true);
        // FightButton만 비활성화
        fightButton.SetActive(false);

        // 현재 위치를 기준으로 연출 시작
        Vector2 currentPlayerPos = waitingBattlePlayer.anchoredPosition;
        Vector2 currentEnemyPos = waitingBattleEnemy.anchoredPosition;

        // 연출용 목표 위치 계산 (X만 변경, Y는 현재값 유지)
        float targetXPlayer = -300f; // 원하는 좌우 이동값
        float targetXEnemy = 300f;

        Vector2 playerTargetPos = new Vector2(targetXPlayer, currentPlayerPos.y);
        Vector2 enemyTargetPos = new Vector2(targetXEnemy, currentEnemyPos.y);

        // 1. WaitingBattleUI 비활성화, FightSceneUI/SkillUI 활성화
        waitingBattleUI.SetActive(false);
        // FightButtonS를 부모에서 분리하고 활성화
        fightButtonS.transform.SetParent(null, true);
        fightButtonS.SetActive(true);

        fightSceneUI.SetActive(true);
        skillUI.SetActive(true);
        enemyInfoMove.SetActive(false);
        summonSceneMovePlayer.SetActive(false);
        restMove.SetActive(false);
        summonSceneMoveMonster.SetActive(false);

        // 2. Enemy를 오른쪽 바깥으로 위치시킴 (예: 화면 기준 800px 오른쪽)
        float offScreenX = 800f; // 실제 해상도에 맞게 조정
        waitingBattleEnemy.anchoredPosition = new Vector2(offScreenX, currentPlayerPos.y);

        // 3. 줌 아웃 애니메이션
        Sequence battleSequence = DOTween.Sequence();
        battleSequence.Append(playerBatchUI.DOScale(playerBatchZoomScale, 0.3f).SetEase(Ease.InOutCubic));
        battleSequence.Join(enemyBatchUI.DOScale(enemyBatchZoomScale, 0.3f).SetEase(Ease.InOutCubic));

        // 4. 플레이어는 중앙으로, Enemy는 오른쪽 바깥에서 중앙으로 슬라이드
        battleSequence.Append(waitingBattlePlayer.DOAnchorPos(playerTargetPos, 0.4f).SetEase(Ease.OutCubic));
        battleSequence.Join(waitingBattleEnemy.DOAnchorPos(enemyTargetPos, 0.4f).SetEase(Ease.OutCubic));

        // 3초 후 자동 전투 종료
        StartCoroutine(EndBattleAfterDelay(3f));
    }
    //테스트용 배틀종료
    private IEnumerator EndBattleAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        OnBattleEnd();
    }
}
