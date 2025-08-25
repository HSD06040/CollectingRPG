using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SynergyDatabase", menuName = "Data/Database/Synergy")]
public class SynergyDatabase : ScriptableObject
{
    public readonly Dictionary<string, SynergyData> _synergyDataDic = new Dictionary<string, SynergyData>(60);

    [SerializeField] UnitSynergyData[] _unitSynergyDatas;
    [SerializeField] ClassSynergyData[] _classSynergyDatas;

    public void Init()
    {        
        if(_unitSynergyDatas.Length == 0 || _classSynergyDatas.Length == 0)
        {
            Debug.Log("시너지 데이터가 설정되지 않았습니다.");
            return;
        }
        foreach (var synergyData in _unitSynergyDatas)
        {
            if (!_synergyDataDic.ContainsKey(synergyData.Synergy.ToString()))
                _synergyDataDic.Add(synergyData.Synergy.ToString(), synergyData);
            else
                Debug.LogWarning($"Duplicate SynergyName: {synergyData.Synergy.ToString()}");
        }

        foreach (var synergyData in _classSynergyDatas)
        {
            if (!_synergyDataDic.ContainsKey(synergyData.Synergy.ToString()))
                _synergyDataDic.Add(synergyData.Synergy.ToString(), synergyData);
            else
                Debug.LogWarning($"Duplicate SynergyName: {synergyData.Synergy.ToString()}");
        }

        foreach (var synergyData in _synergyDataDic.Values)
        {
            synergyData.Init();
        }
    }

    public SynergyData GetSynergy(string synergyName)
    {
        return _synergyDataDic.TryGetValue(synergyName, out var effect) ? effect : null;
    }
}
