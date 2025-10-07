using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StageGameData
{
    private int _currentRegion;
    private int _currentStage;
    public Property<int> CurrentFloor = new();

    #region Stage
    public void SetStage(int region, int stage)
    {
        _currentRegion = region;
        _currentStage = stage;
    }
    public void GetStage(out int region, out int stage)
    {
        region = _currentRegion;
        stage = _currentStage;
    }
    #endregion
}
