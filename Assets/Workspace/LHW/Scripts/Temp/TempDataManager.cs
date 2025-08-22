using System.Collections.Generic;
using UnityEngine;

public class TempDataManager : MonoBehaviour
{
    public static TempDataManager Instance { get; private set; }

    #region Data

    private List<TeamPresetData> _presetData = new List<TeamPresetData>();
    public List<TeamPresetData> PresetData => _presetData;

    #endregion

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); 
        Init();
    }

    private void Init()
    {
        PresetDataInit();
    }

    private void PresetDataInit()
    {
        if (_presetData.Count == 0)
        {
            for (int i = 0; i < 2; i++)
            {
                _presetData.Add(new TeamPresetData(5));
            }
        }
    }

    public void CreatePreset(int size)
    {
        _presetData.Add(new TeamPresetData(size));
    }
}
