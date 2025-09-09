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

    // 중앙 위치(Inspector에서 직접 할당하거나 Awake에서 저장)
    private Vector2 playerOriginPos;
    private Vector2 enemyOriginPos;

    // 화면 밖 위치(해상도에 맞게 조정)
    public float offScreenX = 800f;
    public float slideDuration = 0.4f;

    void Awake()
    {
        playerOriginPos = waitingBattlePlayer.anchoredPosition;
        enemyOriginPos = waitingBattleEnemy.anchoredPosition;

        enemyInfoMoveButton.onClick.AddListener(OnEnemyInfoMove);
        restMoveButton.onClick.AddListener(OnRestMove);
    }

    // EnemyInfoMove 버튼 클릭 시
    private void OnEnemyInfoMove()
    {
        waitingBattlePlayer.DOKill();
        waitingBattleEnemy.DOKill();

        waitingBattlePlayer.gameObject.SetActive(true);
        waitingBattleEnemy.gameObject.SetActive(true);

        // Enemy: 오른쪽 밖에서 중앙으로, Player: 중앙에서 왼쪽 밖으로
        waitingBattleEnemy.anchoredPosition = new Vector2(offScreenX, enemyOriginPos.y);
        waitingBattlePlayer.anchoredPosition = playerOriginPos;

        Sequence seq = DOTween.Sequence();
        seq.Append(waitingBattleEnemy.DOAnchorPos(enemyOriginPos, slideDuration).SetEase(Ease.OutCubic));
        seq.Join(waitingBattlePlayer.DOAnchorPos(new Vector2(-offScreenX, playerOriginPos.y), slideDuration).SetEase(Ease.OutCubic));
    }

    // RestMove 버튼 클릭 시
    private void OnRestMove()
    {
        waitingBattlePlayer.DOKill();
        waitingBattleEnemy.DOKill();

        waitingBattlePlayer.gameObject.SetActive(true);
        waitingBattleEnemy.gameObject.SetActive(true);

        // Enemy: 중앙에서 오른쪽 밖으로, Player: 왼쪽 밖에서 중앙으로
        waitingBattleEnemy.anchoredPosition = enemyOriginPos;
        waitingBattlePlayer.anchoredPosition = new Vector2(-offScreenX, playerOriginPos.y);

        Sequence seq = DOTween.Sequence();
        seq.Append(waitingBattleEnemy.DOAnchorPos(new Vector2(offScreenX, enemyOriginPos.y), slideDuration).SetEase(Ease.OutCubic));
        seq.Join(waitingBattlePlayer.DOAnchorPos(playerOriginPos, slideDuration).SetEase(Ease.OutCubic));
    }
}
