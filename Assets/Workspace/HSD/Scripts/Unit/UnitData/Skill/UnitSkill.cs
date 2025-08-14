using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitSkill : ScriptableObject
{
    public int NeedMana;
    public DamageType DamageType;
    public float BaseCoolTime;
    private float coolDown;

    public abstract void Active();

    public bool CoolTimeCheckActive()
    {
        if (coolDown <= 0)
            return true;

        coolDown -= Time.deltaTime;
        return coolDown <= 0;
    }
}
