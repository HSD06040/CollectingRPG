using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitSkill : ScriptableObject
{
    public int NeedMana;
    public DamageType DamageType;

    public abstract void Active();   
}
