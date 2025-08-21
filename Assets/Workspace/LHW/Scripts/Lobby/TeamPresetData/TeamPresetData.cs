using System;
using UnityEngine;

[Serializable]
public class TeamPresetData
{
    public UnitStatus[] Statuses;

    public TeamPresetData(int size)
    {
        Statuses = new UnitStatus[size];
        for(int i = 0; i < size; i++)
        {
            Statuses[i] = new UnitStatus(null, 0);
        }
    }
}
