using System;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class MapPlayerTracker : MonoBehaviour
{
    [Header("Selection Settings")]
    [SerializeField] private bool _lockAfterSelecting = false;
    [SerializeField] private float _enterNodeDelay = 1f;
    
    [Header("References")]
    [SerializeField] private MapManager _mapManager;
    [SerializeField] private MapView _view;

    public static MapPlayerTracker Instance;

    public bool Locked { get; set; }

    private void Awake()
    {
        Instance = this;
    }

    public void SelectNode(MapNode mapNode)
    {
        if (Locked) return;

        Debug.Log("Selected node: " + mapNode.Node.point);

        if (_mapManager.CurrentMap.path.Count == 0)
        {
            // 첫 번째 노드 선택 - 1층(y=0) 노드만 선택 가능
            if (mapNode.Node.point.y == 0)
                SendPlayerToNode(mapNode);
            else
                PlayWarningThatNodeCannotBeAccessed();
        }
        else
        {
            Vector2Int currentPoint = _mapManager.CurrentMap.path[_mapManager.CurrentMap.path.Count - 1];
            Node currentNode = _mapManager.CurrentMap.GetNode(currentPoint);

            if (currentNode != null && currentNode.outgoing.Any(point => point.Equals(mapNode.Node.point)))
                SendPlayerToNode(mapNode);
            else
                PlayWarningThatNodeCannotBeAccessed();
        }
    }

    private void SendPlayerToNode(MapNode mapNode)
    {
        Locked = _lockAfterSelecting;
        _mapManager.CurrentMap.path.Add(mapNode.Node.point);
        _mapManager.SaveMap();
        _view.SetAttainableNodes();
        _view.SetLineColors();
        mapNode.ShowSwirlAnimation();

        DOTween.Sequence().AppendInterval(_enterNodeDelay).OnComplete(() => EnterNode(mapNode));
    }

    private static void EnterNode(MapNode mapNode)
    {
        
        // UIManager를 통한 패널 표시
        switch (mapNode.Node.nodeType)
        {
            case NodeType.MinorEnemy:
                Debug.Log("일반 전투 시작");
                UIManager.Instance?.ShowBattleTransition();
                break;
                
            case NodeType.EliteEnemy:
                Debug.Log("엘리트 전투 시작");
                UIManager.Instance?.ShowBattleTransition();
                break;
                
            case NodeType.Store:
                Debug.Log("상점 발견");
                UIManager.Instance?.ShowStorePanel();
                break;
                
            case NodeType.Boss:
                Debug.Log("보스 전투 시작");
                UIManager.Instance?.ShowBattleTransition();
                break;
                
            case NodeType.Event:
                Debug.Log("Stage Dialog 이벤트 발생");
                UIManager.Instance?.ShowEventPanel();
                break;
                
            default:
                Debug.LogWarning($"처리되지 않은 노드 타입: {mapNode.Node.nodeType}");
                break;
        }
    }

    private void PlayWarningThatNodeCannotBeAccessed()
    {
        Debug.Log("선택할 수 없는 노드입니다");
        // TODO: UI 경고 효과 추가
        // 예: 빨간색 깜빡임, 사운드 효과 등
    }

    // 패널에서 호출할 수 있는 공개 메서드들
    public void OnBattleComplete()
    {
        Debug.Log("전투 완료");
        UIManager.Instance?.ClosePanelAndContinue();
    }

    public void OnStoreExit()
    {
        Debug.Log("상점 나가기");
        UIManager.Instance?.ClosePanelAndContinue();
    }

    public void OnEventComplete()
    {
        Debug.Log("Stage Dialog 이벤트 완료");
        UIManager.Instance?.ClosePanelAndContinue();
    }
}