using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class SplashProjectile : Projectile
{
    [Header("Stat")]
    [SerializeField] StatEffectModifier _statEffectModifier;
    [SerializeField] float _rotSpeed;
    private float _radius;

    protected override void Awake()
    {
        ComponentProvider.Add(gameObject, this);
    }
    protected override void OnDestroy()
    {
        ComponentProvider.Remove<SplashProjectile>(gameObject);
    }

    public void Init(StatEffectModifier statEffectModifier, float radius,
        Transform target, UnitStatusController status, float attackPower, DamageType damageType, LayerMask targetLayer, float speed, 
        GameObject effect, float distance = 0)
    {
        _statEffectModifier = statEffectModifier;
        _radius = radius;

        base.Init(target, status, attackPower, damageType, targetLayer, speed, effect, distance);
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform == _target)
        {
            foreach (var target in Physics2D.OverlapCircleAll(transform.position, _radius))
            {
                ComponentProvider.Get<UnitBase>(target.gameObject).
                    StatusController.AddStat(_statEffectModifier.StatType, _statEffectModifier.Value, name);
            }
        }
    }

    protected override async UniTask MoveAndDestroyAsync(float duration)
    {
        await transform.DOMove(_target.position, 5 / _speed).SetEase(Ease.Linear).AsyncWaitForCompletion();

        ProjectileDestroy();
    }
}
