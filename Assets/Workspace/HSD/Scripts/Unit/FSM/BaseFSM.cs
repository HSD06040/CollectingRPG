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
    #endregion

    #region State
    public StateMachine StateMachine { get; private set; }
    public StanbyState StanbyState { get; private set; }
    public IdleState IdleState {  get; private set; }
    public MoveState MoveState { get; private set; }
    public AttackState AttackState { get; private set; }
    public SkillState SkillState { get; private set; }
    #endregion

    private Coroutine _fightRoutine;

    public virtual void Init(UnitBase owner)
    {
        Owner = owner;

        StateMachine = new StateMachine();

        StanbyState = new StanbyState(this, _idleHash);
        IdleState = new IdleState(this, _idleHash);
        MoveState = new MoveState(this, _moveHash);
        AttackState = new AttackState(this, _attackHash);
        SkillState = new SkillState(this, _skillHash);

        //Stanby(); //이거 추가 했는데 심지어 주석처리를 해뒀는데
    }

    public void Stanby()
    {
        if(_fightRoutine != null)
        {
            StopCoroutine(_fightRoutine);
            _fightRoutine = null;
        }

        StateMachine.ChangeState(StanbyState);
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
            Owner.FindTarget();
            Owner.FlipToTarget();
            StateMachine.Update();
            yield return null;
        }
    }

    private void AnimationFinished() => StateMachine._currentState.AnimationFinished();
}
