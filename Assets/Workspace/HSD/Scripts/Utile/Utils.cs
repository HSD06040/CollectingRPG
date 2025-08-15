using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public static bool Contain(this LayerMask layerMask, int layer)
    {
        return ((1 << layer) & layerMask) != 0;
    }

    public static int DamageCaluator(int damage, int defense, DamageType damageType)
    {
        float totalDefense = defense / defense + 100;

        return Mathf.RoundToInt(damage * totalDefense);
    }

    public static int TotalDamageCaluator(this UnitData data, float multiply, DamageType damageType)
    {
        int damage = damageType == DamageType.Physical ? data.PhysicalDamage.Value : data.MagicDamage.Value;
        float total = damage * multiply;

        if(data.CritChance.Value > Random.Range(0f, 100f))
        {
            total *= data.CritDamage.Value / 100;
        }

        return Mathf.RoundToInt(total);
    }
}
