using System.Collections.Generic;
using UnityEngine;

public class TempDataManager : MonoBehaviour
{
    #region Singleton

    public static TempDataManager Instance { get; private set; }

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

    #endregion

    #region Data

    private int _selectedPresetIndex = 0;
    public int SelectedPresetIndex => _selectedPresetIndex;

    private List<TeamPresetData> _presetData = new List<TeamPresetData>();
    public List<TeamPresetData> PresetData => _presetData;

    [SerializeField] private TempUpgradeUnitData _upgradeUnitData;
    public TempUpgradeUnitData UpgradeData => _upgradeUnitData;

    #endregion    

    private void Init()
    {
        PresetDataInit();
    }

    #region Preset

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

    public TeamPresetData ReadCurrentSelectedPreset()
    {
        if (_selectedPresetIndex == -1) return null;

        return _presetData[_selectedPresetIndex];
    }

    public void SelectPresetIndex(int index)
    {
        _selectedPresetIndex = index;
    }

    #endregion

    public void AddPiece(int amount)
    {        
        _upgradeUnitData.AddPiece(amount);
    }

    public void LevelUp()
    {
        _upgradeUnitData.LevelUp();
    }
}