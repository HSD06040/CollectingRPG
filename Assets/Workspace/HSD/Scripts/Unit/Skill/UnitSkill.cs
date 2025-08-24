using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public abstract class UnitSkill : ScriptableObject
{
    [Header("Default")]
    public Sprite Icon;
    public string SkillName;
    [TextArea] public string Description;
    public int MaxCount;
    public float Power;
    public int ManaCost;
    public event Action<UnitSkill> UseSkill;

    public virtual void Active(IAttacker attacker)
    {
        UseSkill?.Invoke(this);
    }
}