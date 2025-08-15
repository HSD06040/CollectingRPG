using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitBase : MonoBehaviour
{
    [field: SerializeField] public Transform Target { get; private set; }
    [field: SerializeField] public Animator Anim { get; private set; }
    [field: SerializeField] public Rigidbody2D Rb { get; private set; }
    [field : SerializeField] public UnitData Data { get; private set; }

    public LayerMask TargetLayer;
    public Vector2 TargetDir => GetTargetDirection();
    public int FacingDir => transform.localScale.x > 0 ? -1 : 1;
    private Vector3 _localScale;

    [SerializeField] BaseFSM _fsm;
    [SerializeField] protected UnitStatusController _unitStatusController;

    protected virtual void Awake()
    {        
        _unitStatusController.Init(Data);
    }

    protected virtual void Start()
    {
        _fsm.Init(this);
    }

    public bool SkillCheck()
    {
        if (_unitStatusController.CurMana.Value >= Data.UnitSkill.NeedMana)
        {
            _unitStatusController.CurMana.Value -= Data.UnitSkill.NeedMana;
            return true;
        }
        else 
            return false;
    }

    public void FindTarget()
    {
        if (Target != null) return;
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

    private Vector2 GetTargetDirection()
    {
        if (Target == null) return Vector2.zero;
        return (Target.position - transform.position).normalized;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, 10);
    }
}
