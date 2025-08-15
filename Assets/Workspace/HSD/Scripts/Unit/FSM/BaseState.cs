using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseState
{
    protected BaseFSM _fsm { get; private set; }
    protected Animator _anim;
    protected Rigidbody2D _rb;
    protected Transform _target => _fsm.Owner.Target;
    protected StateMachine _stateMachine;
    protected UnitData _data;
    protected UnitBase _owner;
    private int _animHash;

    public BaseState(BaseFSM fsm, int animHash)
    {
        _fsm = fsm;
        _animHash = animHash;
        _anim = _fsm.Owner.Anim;
        _stateMachine = _fsm.StateMachine;        
        _data = _fsm.Owner.Data;
        _rb = _fsm.Owner.Rb;
        _owner = _fsm.Owner;
    }

    public virtual void Enter()
    {
        _anim.SetBool(_animHash, true);
    }

    public virtual void Update()
    {
        
    }

    public virtual void Exit()
    {
        _anim.SetBool(_animHash, false);
    }
}
