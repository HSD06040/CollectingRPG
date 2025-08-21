using UnityEngine;

public class UnitBase : MonoBehaviour, IAttacker
{
    [field: SerializeField] public Transform Target { get; private set; }
    [field: SerializeField] public Animator Anim { get; private set; }
    [field: SerializeField] public Rigidbody2D Rb { get; private set; }
    [field: SerializeField] public UnitStatus Status { get; set; }
    [field: SerializeField] public Collider2D Col { get; private set; }

    public LayerMask TargetLayer { get; set; }
    public Vector2 TargetDir => GetTargetDirection();
    public Vector2Int CurrentSlot { get; set; }
    private Vector3 _localScale;
    private int _enemyLayer;

    [Header("Logic Components")]
    public UnitStatusController StatusController;
    [SerializeField] BaseFSM _fsm;

    #region LifeCycle
    protected virtual void Awake()
    {
        TargetLayer = gameObject.layer == LayerMask.NameToLayer("Player") ? LayerMask.GetMask("Enemy") : LayerMask.GetMask("Player");
        _enemyLayer = LayerMask.NameToLayer("Enemy");

        Anim ??= GetComponentInChildren<Animator>();
        Rb ??= GetComponent<Rigidbody2D>();
        Col ??= GetComponent<CapsuleCollider2D>();

        AddProviderComponents();
    }

    protected virtual void Start()
    {
        _fsm.Init(this);
    }

    private void OnDestroy()
    {
        RemoveProviderComponents();
    }
    #endregion

    public void Init()
    {  
        Col.enabled = true;

        StatusController.Init(Status);
    }

    #region Provider
    private void AddProviderComponents()
    {
        ComponentProvider.Add<UnitStatusController>(gameObject, StatusController);
    }

    private void RemoveProviderComponents()
    {
        ComponentProvider.Remove<UnitStatusController>(gameObject);
    }
    #endregion

    #region FSM
    public void Fight()
    {
        Init();
        _fsm.Fight();
    }

    public void Stanby()
    {
        _fsm.Stanby();
    }

    public void Attack()
    {
        Status.Data.AttackData.Attack(this);
    }

    public bool SkillCheck()
    {
        if (Status.Data.Skill == null) return false;

        if (StatusController.CurMana.Value >= Status.Data.Skill.ManaCost)
        {
            StatusController.CurMana.Value -= Status.Data.Skill.ManaCost;
            return true;
        }
        else
            return false;
    }

    public void FindTarget()
    {
        if (Target == null || ComponentProvider.Get<UnitStatusController>(Target.gameObject).IsDead)
            Target = Utils.GetClosestTargetNonAlloc(transform.position, StatusController.DetectionRange, TargetLayer);
    }

    public void FlipToTarget()
    {
        if (Target == null || StatusController.IsDead) return;

        _localScale = transform.localScale;

        if (Target == null)
        {
            if (TargetLayer.Contain(_enemyLayer))
                _localScale.x = -Mathf.Abs(_localScale.x);
            else
                _localScale.x = Mathf.Abs(_localScale.x);

            transform.localScale = _localScale;
            return;
        }

        if (transform.position.x > Target.position.x)
        {
            _localScale.x = Mathf.Abs(_localScale.x);
        }
        else
        {
            _localScale.x = -Mathf.Abs(_localScale.x);
        }

        transform.localScale = _localScale;
    }
    

    #region Bool
    /// <summary>
    /// 범위안에 들어와 있다면
    /// </summary>
    /// <returns></returns>
    public bool IsTargetInRange()
    {
        if (Target == null) return false;

        return Vector2.Distance(Target.position, transform.position) <= StatusController.AttackRange.Value;
    }

    /// <summary>
    /// 서로 보는 방향이 다르다면
    /// </summary>
    /// <returns></returns>
    public bool CanAttack()
    {
        if (Target == null) return false;

        if (!IsTargetInRange())
            return false;

        return Vector2.Dot(TargetDir, new Vector2(Target.GetFacingDir(), 0)) < 0;
    }
    #endregion

    #endregion

    #region Getters
    private Vector2 GetTargetDirection()
    {
        if (Target == null) return Vector2.zero;
        return (Target.position - transform.position).normalized;
    }

    public float GetAttackTime()
    {
        return 1 / StatusController.AttackSpeed.Value;
    }

    public Transform GetTarget()
    {
        return Target;
    }

    public UnitStatus GetUnitData()
    {
        return Status;
    }

    public Transform GetTransform()
    {
        return transform;
    }

    public UnitStatusController GetStatusController()
    {
        return StatusController;
    }
    #endregion

    #region Gizmos
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (Status == null) return;

        // 찾는 거리
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, StatusController.DetectionRange);

        // 공격 사거리
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, StatusController.AttackRange.Value);

        if (Status.Data.AttackData == null) return;
        // 공격 범위
        Gizmos.color = Color.red;
        Vector2 center = transform.position;
        if (Status.Data.AttackData is UnitMeleeAttack MeleeAttackData)
        {
            if (MeleeAttackData.SearchType == SearchType.Circle)
            {
                Vector2 offset = Status.Data.AttackData.AttackPointOffset;
                offset *= TargetDir;

                Gizmos.DrawWireSphere(center + offset, MeleeAttackData.SizeOrRadius);
            }
        }
        else if (Status.Data.AttackData is UnitRangedAttack RandAttackData)
        {
            Vector2 offset = Status.Data.AttackData.AttackPointOffset;
            offset.x *= transform.GetFacingDir();

            Gizmos.DrawWireSphere(center + offset, .1f);
        }
    }
#endif
    #endregion
}
