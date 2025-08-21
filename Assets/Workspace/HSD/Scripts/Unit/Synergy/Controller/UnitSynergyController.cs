using System;
using System.Collections.Generic;
using UnityEngine;

public class SynergyController : MonoBehaviour
{
    private readonly Dictionary<string, int> _synergyCountDic = new(60);

    [SerializeField] SynergyDatabase _database;

    public Action OnSynergyChanged;

    private void Awake()
    {
        _database.Init();
        OnSynergyChanged += TestDebug;
    }

    public void AddSynergy(Synergy unitSynergy, ClassType classSynergy)
    {
        if(!_synergyCountDic.ContainsKey(unitSynergy.ToString()))
        {
            _synergyCountDic.Add(unitSynergy.ToString(), 0);
        }
        _synergyCountDic[unitSynergy.ToString()]++;

        if (!_synergyCountDic.ContainsKey(classSynergy.ToString()))
        {
            _synergyCountDic.Add(classSynergy.ToString(), 0);
        }
        _synergyCountDic[classSynergy.ToString()]++;

        CheckSynergy(unitSynergy.ToString());
        CheckSynergy(classSynergy.ToString());

        OnSynergyChanged?.Invoke();
    }

    public void RemoveSynergy(Synergy unitSynergy, ClassType classSynergy)
    {
        _synergyCountDic[unitSynergy.ToString()]--;
        _synergyCountDic[classSynergy.ToString()]--;

        CheckSynergy(unitSynergy.ToString());
        CheckSynergy(classSynergy.ToString());

        OnSynergyChanged?.Invoke();
    }

    private void CheckSynergy(string synergyName)
    {
        if (string.IsNullOrEmpty(synergyName)) return;

        int count = _synergyCountDic.TryGetValue(synergyName, out var val) ? val : 0;
        SynergyData synergy = _database.GetSynergy(synergyName);

        if (synergy == null) return;

        synergy.Check(count);
    }

    private void TestDebug()
    {
        foreach (var synergy in _synergyCountDic)
        {
            Debug.Log($"Synergy: {synergy.Key}, Count: {synergy.Value}");
        }       
    }
}