using System.Collections;
using UnityEngine;

public class BaseFSM : MonoBehaviour
{
    public UnitBase Owner;    

    #region animHash
    private static readonly int _idleHash = Animator.StringToHash("Idle");
    private static readonly int _moveHash = Animator.StringToHash("Move");
    private static readonly int _attackHash = Animator.StringToHash("Attack");
    private static readonly int _skillHash = Animator.StringToHash("Skill");
    private static readonly int _deadHash = Animator.StringToHash("Dead");
    private static readonly int _stunHash = Animator.StringToHash("Stun");
    #endregion

    #region State
    public StateMachine StateMachine { get; private set; }
    public StandbyState StandbyState { get; private set; }
    public IdleState IdleState {  get; private set; }
    public MoveState MoveState { get; private set; }
    public DeadState DeadState { get; private set; }
    public AttackState AttackState { get; private set; }
    public StunState StunState { get; private set; }
    public SkillState SkillState { get; private set; }
    #endregion

    private Coroutine _fightRoutine;

    public virtual void Init(UnitBase owner)
    {
        Owner = owner;

        StateMachine ??= new StateMachine();

        StandbyState ??= new StandbyState(this, _idleHash);
        IdleState ??= new IdleState(this, _idleHash);
        MoveState ??= new MoveState(this, _moveHash);
        AttackState ??= new AttackState(this, _attackHash);
        SkillState ??= new SkillState(this, _skillHash);
        DeadState ??= new DeadState(this, _deadHash);
        StunState ??= new StunState(this, _stunHash);
    }

    public void Standby()
    {
        if(_fightRoutine != null)
        {
            StopCoroutine(_fightRoutine);
            _fightRoutine = null;
        }

        StateMachine.ChangeState(StandbyState);
        StateMachine.Update();
    }

    public void Fight()
    {
        _fightRoutine = StartCoroutine(FightRoutine());        
        StateMachine.ChangeState(MoveState);
    }

    private IEnumerator FightRoutine()
    {
        while (true)
        {
            Owner.FlipToTarget();
            Owner.FindTarget();
            StateMachine.Update();
            yield return null;
        }
    }

    public void ChangeStunState(bool isStun)
    {
        if (isStun)
        {
            StateMachine.ChangeState(StunState);
        }
        else
        {
            StateMachine.ChangeState(IdleState);
        }
    }

    private void Attack() => Owner.Attack();
    private void UseSkill() => Owner.UseSkill();
    private void AnimationFinished() => StateMachine._currentState.AnimationFinished();
}
