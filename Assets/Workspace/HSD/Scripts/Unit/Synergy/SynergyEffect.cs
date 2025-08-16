using System.Collections.Generic;
using UnityEngine;

public abstract class SynergyEffect : ScriptableObject
{
    public string SynergyName;
    public string Description;

    public abstract void ApplyEffect();

    public abstract void RemoveEffect();
}
