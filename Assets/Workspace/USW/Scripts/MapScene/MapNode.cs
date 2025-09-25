using UnityEngine;

public class MapNode : MonoBehaviour
{
    [Header("Visual Components")]
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Color _lockedColor = Color.gray;
    [SerializeField] private Color _attainableColor = Color.white;
    [SerializeField] private Color _visitedColor = Color.green;

    public Node Node { get; private set; }
    public NodeBlueprint Blueprint { get; private set; }
    
    private NodeState _currentState;

    private void Awake()
    {
        if (_spriteRenderer == null)
            _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetUp(Node node, NodeBlueprint blueprint)
    {
        Initialize(node, blueprint);
    }

    public void Initialize(Node node, NodeBlueprint blueprint)
    {
        Node = node;
        Blueprint = blueprint;
        
        // 노드 타입에 따른 스프라이트 설정
        if (blueprint != null && _spriteRenderer != null)
        {
            _spriteRenderer.sprite = blueprint.sprite;
        }
        
        // 보스 노드는 크기 1.5배
        if (node.nodeType == NodeType.Boss)
        {
            transform.localScale = Vector3.one * 1.5f;
        }
        
        Debug.Log($"노드 초기화: {node.nodeType} at {node.point}");
    }

    public void SetState(NodeState state)
    {
        _currentState = state;
        
        if (_spriteRenderer == null) return;

        switch (state)
        {
            case NodeState.Locked:
                _spriteRenderer.color = _lockedColor;
                break;
            case NodeState.Attainable:
                _spriteRenderer.color = _attainableColor;
                break;
            case NodeState.Visited:
                _spriteRenderer.color = _visitedColor;
                break;
        }
    }

    private void OnMouseDown()
    {
        if (_currentState == NodeState.Attainable)
        {
            // 노드 클릭 이벤트
            if (MapPlayerTracker.Instance != null)
            {
                MapPlayerTracker.Instance.SelectNode(this);
            }
        }
        else
        {
            Debug.Log($"선택할 수 없는 노드: {_currentState}");
        }
    }

    public void ShowSwirlAnimation()
    {
        // TODO: 소용돌이 애니메이션
        Debug.Log($"소용돌이 애니메이션: {Node.nodeType}");
    }
}

// 현재 노드 상태.
public enum NodeState
{
    Locked,
    Attainable,
    Visited
}