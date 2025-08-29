using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StatEffect", menuName = "Data/Synergy/Effect/Stat")]
public class Stat_SynergyEffect : SynergyEffect
{
    [SerializeField] protected SynergyStatModifier[] _statModifiers;

    public override void ApplyEffect(UnitBase[] units)
    {

        
    }

    public override void RemoveEffect(UnitBase[] units)
    {
        
    }
}
