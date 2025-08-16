using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SynergyLevelData
{
    public int SynergyNeedCount;
    public SynergyEffect SynergyEffect;
}

public abstract class SynergyData : ScriptableObject
{
    public string SynergyName;
    public string Description;
    public SynergyLevelData[] SynergyLevelData;
    private SynergyEffect _currentEffect;

    public void Check(int newCount)
    {
        SynergyEffect newEffect = null;

        foreach (var levelData in SynergyLevelData)
        {
            if(newCount >= levelData.SynergyNeedCount)
            {
                newEffect = levelData.SynergyEffect;
            }
            else
            {
                break;
            }
        }

        if (_currentEffect != newEffect)
        {
            _currentEffect?.RemoveEffect();
            newEffect?.ApplyEffect();
            _currentEffect = newEffect;
        }
    }
}
