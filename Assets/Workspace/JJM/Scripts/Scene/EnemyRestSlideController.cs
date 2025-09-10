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

    private float topY = 33f; // 원하는 Y값(예: 0이 가장 위, Canvas 기준에 따라 다름)


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

        // Y값을 강제로 위쪽(topY)으로 설정
        waitingBattlePlayer.anchoredPosition = new Vector2(playerOriginPos.x, topY);
        waitingBattleEnemy.anchoredPosition = new Vector2(offScreenX, topY);


        Sequence seq = DOTween.Sequence();
        seq.Append(waitingBattleEnemy.DOAnchorPos(new Vector2(enemyOriginPos.x, topY), slideDuration).SetEase(Ease.OutCubic));
        seq.Join(waitingBattlePlayer.DOAnchorPos(new Vector2(-offScreenX, topY), slideDuration).SetEase(Ease.OutCubic));
    }

    // RestMove 버튼 클릭 시
    private void OnRestMove()
    {
        waitingBattlePlayer.DOKill();
        waitingBattleEnemy.DOKill();

        waitingBattlePlayer.gameObject.SetActive(true);
        waitingBattleEnemy.gameObject.SetActive(true);

        // Y값을 강제로 위쪽(topY)으로 설정
        waitingBattlePlayer.anchoredPosition = new Vector2(-offScreenX, topY);
        waitingBattleEnemy.anchoredPosition = new Vector2(enemyOriginPos.x, topY);

        Sequence seq = DOTween.Sequence();
        seq.Append(waitingBattleEnemy.DOAnchorPos(new Vector2(offScreenX, topY), slideDuration).SetEase(Ease.OutCubic));
        seq.Join(waitingBattlePlayer.DOAnchorPos(new Vector2(playerOriginPos.x, topY), slideDuration).SetEase(Ease.OutCubic));
    }
    public void RestMoveToPlayer()
    {
        OnRestMove();
    }
}
