using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    protected float _lifeTime = 5f; // 발사체의 생명 시간
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

        MoveAndDestroyAsync(_lifeTime).Forget(); // 발사체 이동 및 파괴 비동기 작업 시작
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (_targetLayer.Contain(collision.gameObject.layer))
        {
            if (collision.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(Utils.CalculateBaseDamage(_status, _attackPower, _damageType), _damageType);

                _pireceCount--;

                if (_pireceCount <= 0)
                {
                    Destroy(gameObject);
                }
            }
        }
    }

    protected virtual async UniTaskVoid MoveAndDestroyAsync(float duration)
    {
    }
}
