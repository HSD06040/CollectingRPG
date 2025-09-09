using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MapSelectAnimationController : MonoBehaviour, IDragHandler, IEndDragHandler
{
    [SerializeField] private Scrollbar _scrollBar;

    // Init
    const int SIZE = 7;
    private float[] _pos = new float[SIZE];
    private float _distance;

    // 드래그 중 지정 변수
    private float _targetPos;
    private float _scrollSpeed;

    private int _targetIndex;

    // 드래그 중 여부
    private bool _isDrag;

    private Coroutine _dragCoroutine;

    private void Start()
    {
        _distance = 1f / (SIZE - 1);
        for (int i = 0; i < SIZE; i++) _pos[i] = _distance * i;
    }

    private void Update()
    {
        if (!_isDrag)
        {
            _scrollBar.value = Mathf.Lerp(_scrollBar.value, _targetPos, Time.deltaTime * 5f);
            if(Mathf.Abs(_scrollBar.value - _targetPos) < 0.01)
                _scrollBar.value = _targetPos;
        }
    }

    #region Drag Event

    public void OnDrag(PointerEventData eventData) => _isDrag = true;

    /// <summary>
    /// 드래그 종료 시점의 속도를 저장하여,
    /// 해당 속도가 일정 수준 이하로 떨어졌을 때 드래그를 멈추고 맵을 지정함.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnEndDrag(PointerEventData eventData)
    {
        _scrollSpeed = eventData.delta.x;
        _dragCoroutine = StartCoroutine(DragCoroutine());
    }

    /// <summary>
    /// 현재 스크롤바의 위치를 기준으로 가장 가까운 위치를 반환
    /// </summary>
    /// <returns></returns>
    private float SetPos()
    {
        for (int i = 0; i < SIZE; i++)
        {
            if (_scrollBar.value < _pos[i] + _distance * 0.5f && _scrollBar.value > _pos[i] - _distance * 0.5f)
            {
                _targetIndex = i;
                return _pos[i];
            }
        }
        return 0;
    }

    /// <summary>
    /// 드래그가 종료되었을 때, 해당 드래그의 속도에 따라
    /// 타겟의 위치를 지정
    /// </summary>
    /// <returns></returns>
    private IEnumerator DragCoroutine()
    {
        while (_scrollSpeed > 20)
        {
            _scrollSpeed -= Time.deltaTime * 250;
            yield return null;
        }

        _targetPos = SetPos();
        _isDrag = false;
        _dragCoroutine = null;
    }

    #endregion
}