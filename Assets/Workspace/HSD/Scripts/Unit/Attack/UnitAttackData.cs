using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitAttackData : ScriptableObject
{
    public DamageType DamageType;
    public float AttackPower;    
    
    [Header("Offset")]
    public Vector2 AttackPointOffset;

    public virtual void Attack(IAttacker attacker)
    {
        attacker.GetStatusController().OnAttack?.Invoke();
    }
}