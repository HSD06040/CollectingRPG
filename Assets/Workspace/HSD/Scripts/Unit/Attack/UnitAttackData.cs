using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitAttackData : ScriptableObject
{
    public DamageType DamageType;
    public float AttackPower;

    [Header("Effect")]
    public string EffectAddress;

    public virtual void Attack(IAttacker attacker)
    {
        attacker.GetStatusController().OnAttack?.Invoke();
    }
}