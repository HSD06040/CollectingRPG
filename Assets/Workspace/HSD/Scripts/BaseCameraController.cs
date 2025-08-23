using UnityEngine;
using Cysharp.Threading.Tasks;

public class BaseCameraController : MonoBehaviour
{
    [SerializeField] Transform _stanby;
    [SerializeField] Transform _battle;
    [SerializeField] float _sensitivity = 2f;
    [SerializeField] float _smoothSpeed = 5f;
    [SerializeField] float _upwardLimit = 10f;
    [SerializeField] float _downwardLimit = -10f;
    [SerializeField] float timer = 1f;

    private bool _isBattle;
    private float _y;
    private bool _isMoving; // Move() 실행 중 여부 체크

    public void OnDrag(float deltaY)
    {
        if (_isMoving) return; // Move 중이면 드래그 무시

        // deltaY를 누적
        _y += deltaY * _sensitivity;

        if (!_isBattle)
            _y = Mathf.Clamp(_y, _downwardLimit, 0f);
        else
            _y = Mathf.Clamp(_y, 0f, _upwardLimit);

        // 드래그 시 즉시 반영 (부드럽게)
        var targetPos = new Vector3(transform.position.x, _y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * _smoothSpeed);
    }

    public void OnDragEnd()
    {
        if (_isMoving) return; // 이미 Move 중이면 무시

        if (!_isBattle)
        {
            if (_y < _downwardLimit / 2)
            {
                Move(_battle, timer).Forget();
                _isBattle = true;
            }
            else
            {
                Move(_stanby, timer).Forget();
                _isBattle = false;
            }
        }
        else
        {
            if (_y > _upwardLimit / 2)
            {
                Move(_stanby, timer).Forget();
                _isBattle = false;
            }
            else
            {
                Move(_battle, timer).Forget();
                _isBattle = true;
            }
        }
    }

    public bool IsBattleMode() => _isBattle;

    private async UniTask Move(Transform target, float duration)
    {
        _isMoving = true;

        float elapsed = 0f;
        Vector3 start = transform.position;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            transform.position = Vector3.Lerp(start, target.position, elapsed / duration);
            await UniTask.Yield(PlayerLoopTiming.Update);
        }

        transform.position = target.position;
        _y = target.position.y; // 드래그 기준값 리셋
        _isMoving = false;
    }
}
