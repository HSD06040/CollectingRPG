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
    public float Power = 1;
    public int ManaCost;
    

    public virtual void Active(IAttacker attacker)
    {
        
    }
}