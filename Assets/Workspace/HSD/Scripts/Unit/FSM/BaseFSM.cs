using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseFSM<T> : MonoBehaviour where T : UnitBase
{
    public T Owner;    

    #region animHash
    private static readonly int _idleHash = Animator.StringToHash("Idle");
    private static readonly int _attackHash = Animator.StringToHash("Attack");
    private static readonly int _skillHash = Animator.StringToHash("Skill");
    #endregion

    #region State
    public StateMachine<T> StateMachine { get; private set; }

    public IdleState<T> IdleState {  get; private set; }
    public AttackState<T> AttackState { get; private set; }
    public SkillState<T> SkillState { get; private set; }
    #endregion

    public virtual void Init(T owner)
    {
        Owner = owner;

        StateMachine = new StateMachine<T>();

        IdleState = new IdleState<T>(this, _idleHash);
        AttackState = new AttackState<T>(this, _attackHash);
        SkillState = new SkillState<T>(this, _skillHash);
    }
}
