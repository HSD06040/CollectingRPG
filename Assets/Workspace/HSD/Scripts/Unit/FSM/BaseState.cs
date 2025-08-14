using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseState<T> where T : UnitBase
{
    protected BaseFSM<T> _fsm { get; private set; }
    protected Animator _anim;
    protected StateMachine<T> _stateMachine;
    private int _animHash;

    public BaseState(BaseFSM<T> fsm, int animHash)
    {
        _fsm = fsm;
        _animHash = animHash;
        _anim = _fsm.Owner._anim;
        _stateMachine = _fsm.StateMachine;
    }

    public virtual void Enter()
    {
        
    }

    public virtual void Update()
    {
        
    }

    public virtual void Exit()
    {
        _anim.SetBool(_animHash, false);
    }
}
