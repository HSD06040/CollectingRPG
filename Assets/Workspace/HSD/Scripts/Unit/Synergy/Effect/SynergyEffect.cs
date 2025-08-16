using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SynergyEffect : ScriptableObject
{
    [TextArea]
    public string Description;

    public abstract void ApplyEffect();
    public abstract void RemoveEffect();
}
