using System.Collections.Generic;
using UnityEngine;

public class SynergyController : MonoBehaviour
{
    private readonly Dictionary<string, int> _synergyCountDic = new(60);

    [SerializeField] SynergyDataBase _database;

    private void Awake()
    {
        _database.Init();
    }

    public void AddSynergy(Synergy unitSynergy, ClassSynergy classSynergy)
    {
        _synergyCountDic[unitSynergy.ToString()]++;
        _synergyCountDic[classSynergy.ToString()]++;

        CheckSynergy(unitSynergy.ToString());
        CheckSynergy(classSynergy.ToString());
    }

    public void RemoveSynergy(Synergy unitSynergy, ClassSynergy classSynergy)
    {
        _synergyCountDic[unitSynergy.ToString()]++;
        _synergyCountDic[classSynergy.ToString()]++;

        CheckSynergy(unitSynergy.ToString());
        CheckSynergy(classSynergy.ToString());
    }

    private void CheckSynergy(string synergyName)
    {
        if (string.IsNullOrEmpty(synergyName)) return;

        int count = _synergyCountDic.TryGetValue(synergyName, out var val) ? val : 0;
        SynergyData synergy = _database.GetSynergy(synergyName);

        if (synergy == null) return;

        synergy.Check(count);
    }
}