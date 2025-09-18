using Cysharp.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class Projectile : MonoBehaviour
{
    [SerializeField] protected float _lifeTime = 5f; // 발사체의 생명 시간
    [SerializeField] protected bool _isPirece = true;
    protected int _pireceCount;
    protected float _attackPower;
    protected DamageType _damageType;
    protected LayerMask _targetLayer;
    protected Transform _target;
    protected UnitStatusController _status;
    protected float _speed;
    protected float _distance;
    protected Vector2 _targetDir => GetTargetDir();
    protected Vector2 _dir;

    private void Awake()
    {
        ComponentProvider.Add(gameObject, this);
    }

    private void OnDestroy()
    {
        ComponentProvider.Remove<Projectile>(gameObject);
    }

    public virtual void Init(Transform target, UnitStatusController status, float attackPower, DamageType damageType,
        LayerMask targetLayer, float speed, float distance = 0)
    {        
        _status = status;
        _target = target;
        _targetLayer = targetLayer;
        _damageType = damageType;
        _pireceCount = status.AttackCount.Value;
        _attackPower = attackPower;
        _speed = speed;
        _distance = distance;

        MoveAsync().Forget();
    }
    private async UniTask MoveAsync()
    {
        await UniTask.Yield();
        await MoveAndDestroyAsync(_lifeTime);
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (_targetLayer.Contain(collision.gameObject.layer))
        {
            _status.CalculateDamage(_attackPower, _damageType, ComponentProvider.Get<UnitBase>(collision.gameObject).StatusController);

            if(!_isPirece)
            {
                _pireceCount--;

                if (_pireceCount <= 0)
                {
                    ProjectileDestroy();
                }
            }            
        }
    }

    protected virtual async UniTask MoveAndDestroyAsync(float duration)
    {
        await UniTask.Yield();
    }

    protected Vector2 GetTargetDir()
    {
        if (_target == null) 
            return new Vector2(_status.transform.GetFacingDir(), 0);

        return (_target.position - transform.position).normalized;
    }

    protected void ProjectileDestroy()
    {
        Manager.Resources.Destroy(gameObject);
    }
}
