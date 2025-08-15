using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StanbyState : BaseState
{
    public StanbyState(BaseFSM fsm, int animHash) : base(fsm, animHash)
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

public class IdleState : BaseState
{
    private float attackTimer;

    public IdleState(BaseFSM fsm, int animHash) : base(fsm, animHash)
    {
    }

    public override void Enter()
    {
        base.Enter();
        attackTimer = _fsm.Owner.Data.GetAttackTime();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if(_fsm.Owner.SkillCheck())
        {
            _stateMachine.ChangeState(_fsm.SkillState);
        }
        else if(CanAttack())
        {
            _stateMachine.ChangeState(_fsm.AttackState);
        }
    }

    private bool CanAttack()
    {
        attackTimer -= Time.deltaTime;
        return attackTimer <= 0;        
    }
}

public class MoveState : BaseState
{
    public MoveState(BaseFSM fsm, int animHash) : base(fsm, animHash)
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

        if(_target == null)
        {
            _owner.transform.Translate(new Vector2(_owner.FacingDir * _data.MoveSpeed.Value * Time.deltaTime, 0), Space.World);
            //_rb.velocity = Vector2.right * _data.MoveSpeed.Value * Time.deltaTime;
        }
        else
        {
            Debug.Log(_owner.TargetDir);
            _owner.transform.Translate(_owner.TargetDir * _data.MoveSpeed.Value * Time.deltaTime, Space.World);
            //_rb.velocity = _owner.TargetDir * _data.MoveSpeed.Value * Time.deltaTime;
        }
    }
}

public class AttackState : BaseState
{
    public AttackState(BaseFSM fsm, int animHash) : base(fsm, animHash)
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

public class SkillState : BaseState
{
    AnimatorStateInfo stateInfo;

    public SkillState(BaseFSM fsm, int animHash) : base(fsm, animHash)
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
