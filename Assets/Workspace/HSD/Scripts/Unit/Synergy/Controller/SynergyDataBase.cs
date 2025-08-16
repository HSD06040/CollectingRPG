using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SynergyDataBase
{
    private readonly Dictionary<string, SynergyData> _synergyEffectDic = new Dictionary<string, SynergyData>(60);

    [SerializeField] private UnitSynergyData[] _unitSynergyEffects;
    [SerializeField] private ClassSynergyData[] _classSynergyEffects;

    public void Init()
    {        
        foreach (var effect in _unitSynergyEffects)
        {
            if (!_synergyEffectDic.ContainsKey(effect.SynergyName))
                _synergyEffectDic.Add(effect.Synergy.ToString(), effect);
            else
                Debug.LogWarning($"Duplicate SynergyName: {effect.SynergyName}");
        }

        foreach (var effect in _classSynergyEffects)
        {
            if (!_synergyEffectDic.ContainsKey(effect.SynergyName))
                _synergyEffectDic.Add(effect.Synergy.ToString(), effect);
            else
                Debug.LogWarning($"Duplicate SynergyName: {effect.SynergyName}");
        }
    }

    public SynergyData GetSynergy(string synergyName)
    {
        return _synergyEffectDic.TryGetValue(synergyName, out var effect) ? effect : null;
    }
}
