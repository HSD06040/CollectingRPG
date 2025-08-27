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
    public Sprite Icon;
    public string SynergyName;
    [TextArea]
    public string Description;
    public SynergyLevelData[] SynergyLevelData;

    private SynergyEffect _currentEffect;
    public int CurrentUpgradeIdx;

    public void Init()
    {
        _currentEffect = null;
        CurrentUpgradeIdx = -1;
    }

    public void Check(int newCount)
    {
        SynergyEffect newEffect = null;

        CurrentUpgradeIdx = -1;

        for (int i = 0; i < SynergyLevelData.Length; i++)
        {
            if (newCount >= SynergyLevelData[i].SynergyNeedCount)
            {
                newEffect = SynergyLevelData[i].SynergyEffect;
                CurrentUpgradeIdx = i;
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
