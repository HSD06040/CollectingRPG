using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState<T> : BaseState<T> where T : UnitBase
{
    public IdleState(BaseFSM<T> fsm, int animHash) : base(fsm, animHash)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
    }
}

public class AttackState<T> : BaseState<T> where T : UnitBase
{
    public AttackState(BaseFSM<T> fsm, int animHash) : base(fsm, animHash)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
    }
}

public class SkillState<T> : BaseState<T> where T : UnitBase
{
    AnimatorStateInfo stateInfo;

    public SkillState(BaseFSM<T> fsm, int animHash) : base(fsm, animHash)
    {
    }

    public override void Enter()
    {
        base.Enter();
        stateInfo = _anim.GetCurrentAnimatorStateInfo(0);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (stateInfo.normalizedTime >= 1f && !stateInfo.loop)
        {
            _stateMachine.ChangeState(_fsm.IdleState);
        }
    }
}
