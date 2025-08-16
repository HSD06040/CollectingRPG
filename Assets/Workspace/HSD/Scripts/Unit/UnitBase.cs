using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class UnitBase : MonoBehaviour, IAttacker
{    
    [field: SerializeField] public Transform Target { get; private set; }
    [field: SerializeField] public Animator Anim { get; private set; }
    [field: SerializeField] public Rigidbody2D Rb { get; private set; }
    [field : SerializeField] public UnitData Data { get; private set; }

    public LayerMask TargetLayer { get; set; }
    public Vector2 TargetDir => GetTargetDirection();

    private Vector3 _localScale;

    [Header("Logic Components")]
    public UnitStatusController StatusController;
    [SerializeField] BaseFSM _fsm;

    protected virtual void Awake()
    {        
        TargetLayer = gameObject.layer == LayerMask.NameToLayer("Player") ? LayerMask.GetMask("Enemy") : LayerMask.GetMask("Player");
        StatusController.Init(Data);
    }

    protected virtual void Start()
    {
        _fsm.Init(this);
    }

    public void Attack()
    {
        Data.AttackData.Attack(this);
    }

    public bool SkillCheck()
    {
        return false; 
        if (StatusController.CurMana.Value >= Data.Skill.NeedMana)
        {
            StatusController.CurMana.Value -= Data.Skill.NeedMana;
            return true;
        }
        else 
            return false;
    }

    public void FindTarget()
    {
        if (Target != null) return;
        Debug.Log("타겟 찾기!");
        Target = Utils.GetClosestTargetNonAlloc(transform.position, 10, TargetLayer);
    }

    public void FlipToTarget()
    {
        _localScale = transform.localScale;

        if (Target == null)
        {
            _localScale.x = -Mathf.Abs(_localScale.x);
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

    /// <summary>
    /// 범위안에 들어와 있다면
    /// </summary>
    /// <returns></returns>
    public bool IsTargetInRange()
    {
        if(Target == null) return false;

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

    private Vector2 GetTargetDirection()
    {
        if (Target == null) return Vector2.zero;
        return (Target.position - transform.position).normalized;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (Data == null) return;

        // 찾는 거리
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, 10);

        // 공격 사거리
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, StatusController.AttackRange.Value);

        if (Data.AttackData == null) return;
        // 공격 범위
        Gizmos.color = Color.red;
        if (Data.AttackData.SearchType == SearchType.Circle)
        {
            Vector2 center = transform.position;
            Vector2 offset = Data.AttackData.Offset;
            offset.x *= transform.GetFacingDir();

            Gizmos.DrawWireSphere(center + offset, Data.AttackData.SizeOrRadius);
        }
    }
#endif

    public float GetAttackTime()
    {
        return 1 / StatusController.AttackSpeed.Value;
    }

    public Transform GetTarget()
    {
        return Target;
    }

    public UnitData GetUnitData()
    {
        return Data;
    }

    public Transform GetTransform()
    {
        return transform;
    }

    public UnitStatusController GetStatusController()
    {
        return StatusController;
    }
}
