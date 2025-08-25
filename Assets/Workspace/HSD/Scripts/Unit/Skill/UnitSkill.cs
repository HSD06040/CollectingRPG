using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public abstract class UnitSkill : ScriptableObject
{
    [Header("Default")]
    public Sprite Icon;
    [TextArea] public string Description;
    public int ManaCost;
    public event Action<UnitSkill> UseSkill;

    public virtual void Active(IAttacker attacker)
    {
        UseSkill?.Invoke(this);
    }
}