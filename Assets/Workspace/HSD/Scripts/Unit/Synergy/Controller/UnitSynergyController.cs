using System;
using System.Collections.Generic;
using UnityEngine;

public class SynergyController : MonoBehaviour
{
    private readonly Dictionary<string, int> _synergyCountDic = new(60);

    public static SynergyDatabase SynergyDB;    // 추후 싱글톤 적재예정

    public event Action<string, int> OnSynergyChanged;

    public void Init()
    {
        SynergyDB = Resources.Load<SynergyDatabase>("Database/SynergyDatabase");
        SynergyDB.Init();
    }

    public void AddSynergy(Synergy unitSynergy, ClassType classSynergy)
    {
        string unitSynergyName = unitSynergy.ToString();
        string classSynergyName = classSynergy.ToString();

        if (!_synergyCountDic.ContainsKey(unitSynergyName))
        {
            _synergyCountDic.Add(unitSynergyName, 0);
        }
        _synergyCountDic[unitSynergyName]++;

        //

        if (!_synergyCountDic.ContainsKey(classSynergyName))
        {
            _synergyCountDic.Add(classSynergyName, 0);
        }
        _synergyCountDic[classSynergyName]++;

        CheckSynergy(unitSynergyName);
        CheckSynergy(classSynergyName);

        OnSynergyChanged?.Invoke(unitSynergyName, _synergyCountDic[unitSynergyName]);
        OnSynergyChanged?.Invoke(classSynergyName, _synergyCountDic[classSynergyName]);
    }

    public void RemoveSynergy(Synergy unitSynergy, ClassType classSynergy)
    {
        string unitSynergyName = unitSynergy.ToString();
        string classSynergyName = classSynergy.ToString();

        _synergyCountDic[unitSynergyName]--;
        _synergyCountDic[classSynergyName]--;

        CheckSynergy(unitSynergyName);
        CheckSynergy(classSynergyName);

        OnSynergyChanged?.Invoke(unitSynergyName, _synergyCountDic[unitSynergyName]);
        OnSynergyChanged?.Invoke(classSynergyName, _synergyCountDic[classSynergyName]);
    }

    private void CheckSynergy(string synergyName)
    {
        if (string.IsNullOrEmpty(synergyName)) return;

        int count = _synergyCountDic.TryGetValue(synergyName, out var val) ? val : 0;
        SynergyData synergy = SynergyDB.GetSynergy(synergyName);

        if (synergy == null) return;

        synergy.Check(count);
    }
}