using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitAttackData : ScriptableObject
{
    public DamageType DamageType;

    [Header("Serch")]
    public SearchType SearchType;
    public float SizeOrRadius;
    public Vector2 BoxSize;
    
    [Header("Offset")]
    public Vector2 Offset;

    public abstract void Attack(IAttacker attacker);
}