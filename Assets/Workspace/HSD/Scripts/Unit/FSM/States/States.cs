using Cysharp.Threading.Tasks;
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
        attackTimer = _fsm.Owner.Data.GetAttackTime(); // 공격 시간 체크
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if(_fsm.Owner.SkillCheck()) // 스킬 사용 가능 여부 체크
        {
            _stateMachine.ChangeState(_fsm.SkillState);
        }
        else if(CanAttack()) // 공격 가능 체크
        {
            _stateMachine.ChangeState(_fsm.AttackState);
        }
        else if (!_owner.IsTargetInRange()) // 멀어진거 체크
        {
            _stateMachine.ChangeState(_fsm.MoveState);
        }
    }

    private bool CanAttack()
    {        
        if (!_owner.CanAttack())
            return false;

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

        if(_owner.IsTargetInRange())
        {
            _stateMachine.ChangeState(_fsm.IdleState);
            return;
        }

        if(_target == null)
        {
            _owner.transform.Translate(new Vector2(_owner.transform.GetFacingDir() * _data.MoveSpeed.Value * Time.deltaTime, 0), Space.World);
            //_rb.velocity = Vector2.right * _data.MoveSpeed.Value * Time.deltaTime;
        }
        else
        {
            _owner.transform.Translate(_owner.TargetDir * _data.MoveSpeed.Value * Time.deltaTime, Space.World);
            //_rb.velocity = _owner.TargetDir * _data.MoveSpeed.Value * Time.deltaTime;
        }
    }
}

public class AnimationFinishedState : BaseState
{
    public AnimationFinishedState(BaseFSM fsm, int animHash) : base(fsm, animHash)
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

        if (_isAnimFinished)
            _stateMachine.ChangeState(_fsm.IdleState);
    }
}

public class AttackState : AnimationFinishedState
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

public class SkillState : AnimationFinishedState
{
    public SkillState(BaseFSM fsm, int animHash) : base(fsm, animHash)
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
