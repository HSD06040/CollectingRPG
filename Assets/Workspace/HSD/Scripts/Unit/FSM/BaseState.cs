using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseState<T> where T : UnitBase
{
    protected BaseFSM<T> _fsm { get; private set; }
    protected Animator _anim => _fsm.Owner._anim;
    private int _animHash;

    public BaseState(BaseFSM<T> fsm, int animHash)
    {
        _fsm = fsm;
        _animHash = animHash;
    }

    public virtual void Enter()
    {
        _anim.SetBool(_animHash, true);
    }

    public virtual void Update()
    {
        _anim.GetCurrentAnimatorClipInfo(0);
    }

    public virtual void Exit()
    {
        _anim.SetBool(_animHash, false);
    }
}
