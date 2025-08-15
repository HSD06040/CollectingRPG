using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMeleeAttack : UnitAttackData
{
   

    public override void Attack(IAttacker attacker)
    {        
        float damage = DamageType == DamageType.Magic ? attacker.GetUnitData().MagicDamage.Value : attacker.GetUnitData().PhysicalDamage.Value;
        damage *= attacker.AttackPower;

        foreach (GameObject obj in Utils.GetTargetsNonAlloc(attacker.GetTransform(), SearchType.Circle, SizeOrRadius, BoxSize))
        {

        }
        
    }
}
