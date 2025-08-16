using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SynergyDatabase", menuName = "Data/Synergy/Database")]
public class SynergyDatabase : ScriptableObject
{
    private readonly Dictionary<string, SynergyData> _synergyEffectDic = new Dictionary<string, SynergyData>(60);

    [SerializeField] private UnitSynergyData[] _unitSynergyDatas;
    [SerializeField] private ClassSynergyData[] _classSynergyDatas;

    public void Init()
    {        
        foreach (var effect in _unitSynergyDatas)
        {
            if (!_synergyEffectDic.ContainsKey(effect.SynergyName))
                _synergyEffectDic.Add(effect.Synergy.ToString(), effect);
            else
                Debug.LogWarning($"Duplicate SynergyName: {effect.SynergyName}");
        }

        foreach (var effect in _classSynergyDatas)
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
