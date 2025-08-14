using System.Collections;
using System.Collections.Generic;
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
    public IdleState IdleState {  get; private set; }
    public AttackState AttackState { get; private set; }
    public SkillState SkillState { get; private set; }
    #endregion

    public virtual void Init(UnitBase owner)
    {
        Owner = owner;

        StateMachine = new StateMachine();

        IdleState = new IdleState(this, _idleHash);
        AttackState = new AttackState(this, _attackHash);
        SkillState = new SkillState(this, _skillHash);
    }
}
