using Cysharp.Threading.Tasks;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] protected float _lifeTime = 5f; // 발사체의 생명 시간
    protected int _pireceCount;
    protected float _attackPower;
    protected DamageType _damageType;
    protected LayerMask _targetLayer;
    protected Transform _target;
    protected UnitStatusController _status;
    protected float _speed;
    protected Vector2 _direction;

    private void Awake()
    {
        ComponentProvider.Add(gameObject, this);
    }

    private void OnDestroy()
    {
        ComponentProvider.Remove<Projectile>(gameObject);
    }

    public virtual void Init(Transform target, UnitStatusController status, float attackPower, DamageType damageType, LayerMask targetLayer, float speed)
    {        
        _status = status;
        _target = target;
        _targetLayer = targetLayer;
        _damageType = damageType;
        _pireceCount = status.AttackCount.Value;
        _attackPower = attackPower;
        _speed = speed;

        if(_target == null)
            _target = Physics2D.OverlapCircle(transform.position, status.AttackRange.Value, targetLayer)?.transform;

        MoveAndDestroyAsync(_lifeTime).Forget(); // 발사체 이동 및 파괴 비동기 작업 시작
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (_targetLayer.Contain(collision.gameObject.layer))
        {
            _status.CalculateDamage(_attackPower, _damageType, ComponentProvider.Get<UnitStatusController>(collision.gameObject));

            _pireceCount--;

            if (_pireceCount <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    protected virtual async UniTask MoveAndDestroyAsync(float duration)
    {
        await UniTask.Delay(1);
    }
}
