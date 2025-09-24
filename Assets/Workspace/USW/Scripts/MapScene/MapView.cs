using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapView : MonoBehaviour
{
    [Header("Map Settings")]
    [SerializeField] private GameObject _nodePrefab;
    [SerializeField] private Transform _mapContainer;
    
    [Header("Camera Settings")]
    [SerializeField] private Camera _mapCamera;
    [SerializeField] private float _layerSpacing = 4f;
    [SerializeField] private float _nodeSpacing = 3f;
    
    [Header("Colors")]
    [SerializeField] private Color _lockedColor = Color.gray;
    [SerializeField] private Color _attainableColor = Color.white;
    [SerializeField] private Color _visitedColor = Color.green;

    public static MapView Instance { get; private set; }
    
    private Map _currentMap;
    private List<MapNode> _mapNodes = new List<MapNode>();
    private Vector2Int _currentPlayerPosition = new Vector2Int(-1, -1);

    private void Awake()
    {
        Instance = this;
        if (_mapCamera == null) _mapCamera = Camera.main;
    }

    public void ShowMap(Map map)
    {
        _currentMap = map;
        ClearMap();
        CreateNodes();
        SetupNodeStates();
        CenterCameraOnCurrentPosition();
        
        Debug.Log($"맵 표시 완료: {map.nodes.Count}개 노드");
    }

    private void ClearMap()
    {
        foreach (var node in _mapNodes)
        {
            if (node != null && node.gameObject != null)
                Destroy(node.gameObject);
        }
        _mapNodes.Clear();
    }

    private void CreateNodes()
    {
        if (_currentMap == null || _nodePrefab == null) return;

        foreach (var node in _currentMap.nodes)
        {
            GameObject nodeObj = Instantiate(_nodePrefab, _mapContainer);
            MapNode mapNode = nodeObj.GetComponent<MapNode>();
            
            if (mapNode == null)
                mapNode = nodeObj.AddComponent<MapNode>();
                
            // 노드 위치 계산 (중앙 정렬)
            Vector3 worldPos = CalculateNodeWorldPosition(node);
            nodeObj.transform.position = worldPos;
            
            // 노드 설정
            mapNode.Initialize(node, GetNodeBlueprint(node.nodeType));
            _mapNodes.Add(mapNode);
        }
    }

    private Vector3 CalculateNodeWorldPosition(Node node)
    {
        // 해당 층의 노드 개수 구하기
        var layerNodes = _currentMap.nodes.Where(n => n.point.y == node.point.y).ToList();
        int layerNodeCount = layerNodes.Count;
        
        // X 위치: 해당 층에서 중앙 정렬
        float xOffset = (layerNodeCount - 1) * _nodeSpacing * 0.5f;
        float x = node.point.x * _nodeSpacing - xOffset;
        
        // Y 위치: 층별 간격
        float y = node.point.y * _layerSpacing;
        
        return new Vector3(x, y, 0);
    }

    private NodeBlueprint GetNodeBlueprint(NodeType nodeType)
    {
        // TODO: MapConfig에서 Blueprint 가져오기
        // 임시로 null 반환
        return null;
    }

    public void SetAttainableNodes()
    {
        if (_currentMap == null) return;

        // 모든 노드를 먼저 잠금 상태로
        foreach (var mapNode in _mapNodes)
        {
            mapNode.SetState(NodeState.Locked);
        }

        if (_currentMap.path.Count == 0)
        {
            // 게임 시작: 0층 모든 노드 활성화
            foreach (var mapNode in _mapNodes.Where(n => n.Node.point.y == 0))
            {
                mapNode.SetState(NodeState.Attainable);
            }
        }
        else
        {
            // 경로상 노드들을 방문됨으로 표시
            foreach (var pathPoint in _currentMap.path)
            {
                var visitedNode = _mapNodes.FirstOrDefault(n => n.Node.point.Equals(pathPoint));
                if (visitedNode != null)
                    visitedNode.SetState(NodeState.Visited);
            }

            // 현재 위치에서 갈 수 있는 노드들 활성화
            Vector2Int currentPos = _currentMap.path.Last();
            _currentPlayerPosition = currentPos;
            
            Node currentNode = _currentMap.GetNode(currentPos);
            if (currentNode != null)
            {
                foreach (var outgoingPoint in currentNode.outgoing)
                {
                    var attainableNode = _mapNodes.FirstOrDefault(n => n.Node.point.Equals(outgoingPoint));
                    if (attainableNode != null)
                        attainableNode.SetState(NodeState.Attainable);
                }
            }
        }

        // 카메라를 현재 위치로 이동
        CenterCameraOnCurrentPosition();
    }

    private void CenterCameraOnCurrentPosition()
    {
        if (_mapCamera == null) return;

        Vector3 targetPosition = _mapCamera.transform.position;

        if (_currentMap.path.Count > 0)
        {
            // 현재 플레이어 위치로 카메라 이동
            Vector2Int currentPos = _currentMap.path.Last();
            targetPosition.y = currentPos.y * _layerSpacing;
        }
        else
        {
            // 게임 시작시 0층으로
            targetPosition.y = 0;
        }

        // 맵 경계 체크
        float mapHeight = (_currentMap.nodes.Max(n => n.point.y)) * _layerSpacing;
        float cameraHeight = _mapCamera.orthographicSize * 2;
        
        // 카메라가 맵 범위를 벗어나지 않도록 제한
        targetPosition.y = Mathf.Clamp(targetPosition.y, 
            cameraHeight * 0.5f, 
            mapHeight - cameraHeight * 0.5f);

        _mapCamera.transform.position = targetPosition;
        
        Debug.Log($"카메라 위치 조정: {targetPosition}");
    }

    public void SetLineColors()
    {
        // TODO: 라인 렌더링 시스템 구현
        Debug.Log("라인 색상 설정 (TODO)");
    }

    private void SetupNodeStates()
    {
        SetAttainableNodes();
    }
}