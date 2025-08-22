using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public abstract class UnitSkill : ScriptableObject
{
    public int ManaCost;    

    public abstract void Active(IAttacker attacker);     
}