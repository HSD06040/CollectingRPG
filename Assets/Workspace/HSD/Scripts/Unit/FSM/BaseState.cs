using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseState
{
    protected BaseFSM _fsm { get; private set; }
    protected Animator _anim;
    protected StateMachine _stateMachine;
    private int _animHash;

    public BaseState(BaseFSM fsm, int animHash)
    {
        _fsm = fsm;
        _animHash = animHash;
        _anim = _fsm.Owner.Anim;
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
