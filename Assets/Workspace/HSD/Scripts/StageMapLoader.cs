using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageMapLoader : MonoBehaviour
{
    [SerializeField] Transform _mapContent;

    public void MapSetting()
    {
        Instantiate(Manager.Data.StageGameData.GetCurrentStage().Map, _mapContent);
    }
}
