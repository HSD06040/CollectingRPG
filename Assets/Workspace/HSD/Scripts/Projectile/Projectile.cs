using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    protected int _pireceCount;
    protected float _attackPower;
    protected DamageType _damageType;
    protected LayerMask _targetLayer;
    protected Transform _target;
    protected UnitStatusController _status;
    protected float _speed;

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
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if(_targetLayer.Contain(collision.gameObject.layer))
        {            
            if(collision.TryGetComponent(out IDamageable damageable))
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
}
