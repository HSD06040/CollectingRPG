using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatBuff_SynergyEffect : SynergyEffect
{
    [SerializeField] protected TargetType _targetType;
    [SerializeField] protected BuffEffectData[] _buffEffects;

    public override void ApplyEffect(UnitBase[] units)
    {
        
    }

    public override void RemoveEffect(UnitBase[] units)
    {
        
    }
}
