using DG.Tweening;
using Michsky.UI.ModernUIPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class TeamPresetData
{
    public UnitStatus[] Statuses;

    public TeamPresetData(int size)
    {
        Statuses = new UnitStatus[size];
        for (int i = 0; i < size; i++)
        {
            Statuses[i] = new UnitStatus(null, 0);
        }
    }
}

public class TeamOrganizeManager : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private CollectedCharacterData _collectedCharacterData;
    
    [Header("Data")]
    // TempDataManager 반영 이전
    //[SerializeField] private List<TeamPresetData> _presetData = new List<TeamPresetData>();

    private UnitStatus _selectedUnit;
    private UnitStatus[] _currentPreset;
    private List<UnitStatus> _collectedUnits;

    [Header("UI")]
    [SerializeField] private TMP_Text _costInfoText;
    [SerializeField] private TMP_Text _totalOverallPowerText;
    [SerializeField] private TMP_Text _leaderEffectText;
    [SerializeField] private TMP_Text _characterCountText;
    [SerializeField] private ButtonManagerBasic[] _presetAddButton;
    
    [Header("Capacity")]
    [SerializeField] private int _totalCost = 10;
    public int TotalCost => _totalCost;

    public Action OnCharacterDataChanged;

    private int _currentCost = 0;
    public int CurrentCost => _currentCost;
    private int _currentOverallPower;

    private void Awake()
    {
        // TempDataManager 반영 이전
        /*
        if(_presetData.Count == 0)
        {
            for(int i = 0; i < 2; i++)
            {
                _presetData.Add(new TeamPresetData(5));
            }
        }
        _currentPreset = _presetData[0].Statuses;
        */

        
    }    
    private void Start()
    {
        if(TempDataManager.Instance != null) _currentPreset = TempDataManager.Instance.PresetData[0].Statuses;
        LoadData();
    }    

    private async void LoadData()
    {
        await Manager.Resources.LoadLabel("Stage");
    }

    #region Event

    private void OnEnable()
    {
        _collectedUnits = _collectedCharacterData.CollectedUnit;
        OnCharacterDataChanged += ShowCostInfo;
        OnCharacterDataChanged += ShowTotalOverallPowerInfo;
        OnCharacterDataChanged += ShowLeaderEffectInfo;
        OnCharacterDataChanged += ShowCharacterCountInfo;
        OnCharacterDataChanged += ShowButtonPreset;
        ShowCostInfo();
        ShowTotalOverallPowerInfo();
        ShowCharacterCountInfo();
        ShowButtonPreset();
    }

    private void OnDisable()
    {
        OnCharacterDataChanged -= ShowCostInfo;
        OnCharacterDataChanged -= ShowTotalOverallPowerInfo;
        OnCharacterDataChanged -= ShowLeaderEffectInfo;
        OnCharacterDataChanged -= ShowCharacterCountInfo;
        OnCharacterDataChanged -= ShowButtonPreset;
    }

    #endregion

    #region Read Data

    public UnitStatus GetCurrentPresetData(int index)
    {
        if (_currentPreset != null)
        {
            return _currentPreset[index];
        }

        return null;
    }

    #endregion

    #region Manual Selection

    public void AddPresetData(UnitStatus data)
    {
        _selectedUnit = data;

        for (int i = 0; i < 5; i++)
        {
            if (_currentPreset[i].Data != null && _currentPreset[i].Data.ID == data.Data.ID)
            {
                Debug.Log("이미 편성된 캐릭터입니다");
                return;
            }
        }

        if (_currentCost + data.Data.Cost > _totalCost)
        {
            Debug.Log($"코스트 상한치를 초과했습니다 {data.Data.Cost} {_currentCost}");
            return;
        }

        for(int i = 0; i < _currentPreset.Length; i++)
        {
            if (_currentPreset[i].Data == null)
            {
                _currentPreset[i].Data = _selectedUnit.Data;
                _currentPreset[i].Level = _selectedUnit.Level;
                _currentCost += _currentPreset[i].Data.Cost;
                _currentOverallPower += _currentPreset[i].CombatPower;
                Debug.Log("편성됨");
                break;
            }

            if(i == _currentPreset.Length - 1)
            {
                Debug.Log("편성 제한치를 초과했습니다.");
                return;
            }
        }

        OnCharacterDataChanged?.Invoke();
    }

    public void RemoveUnitData(int index)
    {
        if(_currentPreset[index].Data != null)
        {
            _currentCost -= _currentPreset[index].Data.Cost;
            _currentOverallPower -= _currentPreset[index].CombatPower;
            _currentPreset[index].Data = null;
            _currentPreset[index].Level = 0;

            OnCharacterDataChanged?.Invoke();
        }
    }

    #endregion

    #region AutoMatic Selection

    /// <summary>
    /// 동적 계획법 알고리즘을 이용한 캐릭터 자동편성
    /// </summary>
    public void AutoSelectCharacters()
    {
        int n = _collectedUnits.Count;
        int[,,] dp = new int[n + 1, _totalCost + 1, _currentPreset.Length + 1];
        bool[,,] take = new bool[n + 1, _totalCost + 1, _currentPreset.Length + 1];

        // DP 진행 - Bottom-Up 방식
        for (int i = 1; i <= n; i++)
        {
            int power = _collectedUnits[i - 1].CombatPower;
            int cost = _collectedUnits[i - 1].Data.Cost;

            for (int c = 0; c <= _totalCost; c++)
            {
                for (int k = 0; k <= _currentPreset.Length; k++)
                {
                    // 선택 안함
                    dp[i, c, k] = dp[i - 1, c, k];

                    // 선택 가능할 때
                    if (c >= cost && k >= 1)
                    {
                        int newPower = dp[i - 1, c - cost, k - 1] + power;
                        if (newPower > dp[i, c, k])
                        {
                            dp[i, c, k] = newPower;
                            take[i, c, k] = true;
                        }
                    }
                }
            }
        }

        // 최적 값 찾기
        // TODO : 같은 최적값이 여러 개일 경우 추가 판정 할지? 현재는 제일 먼저 찾은 값 기준으로 편성
        int bestPower = 0;
        int bestC = 0;
        int bestK = 0;
        for (int c = 0; c <= _totalCost; c++)
        {
            for (int k = 0; k <= _currentPreset.Length; k++)
            {
                if (dp[n, c, k] > bestPower)
                {
                    bestPower = dp[n, c, k];
                    bestC = c;
                    bestK = k;
                }
            }
        }

        // 선택한 캐릭터 역추적
        List<UnitStatus> bestTeam = new List<UnitStatus>();
        int ci = bestC;
        int ki = bestK;

        for (int i = n; i > 0; i--)
        {
            if (take[i, ci, ki])
            {
                bestTeam.Add(_collectedUnits[i - 1]);
                ci -= _collectedUnits[i - 1].Data.Cost;
                ki -= 1;
            }
        }

        // 전투력이 높은 순으로 정렬
        bestTeam.OrderByDescending(n => n);

        // 기존 편성 초기화
        Array.Clear(_currentPreset, 0, _currentPreset.Length);
        _currentCost = 0;

        // 최적 편성 적용
        for (int i = 0; i < bestTeam.Count; i++)
        {
            _currentPreset[i] = new UnitStatus(bestTeam[i].Data, bestTeam[i].Level);
            _currentCost += bestTeam[i].Data.Cost;
        }
        _currentOverallPower = bestPower;

        OnCharacterDataChanged?.Invoke();
    }

    #endregion

    #region UI Output

    private void ShowCostInfo()
    {
        _costInfoText.text = $"충성도 {_currentCost} / {_totalCost}";
    }

    private void ShowTotalOverallPowerInfo()
    {
        //_totalOverallPowerText.text = $"팀 전투력 {_currentOverallPower}";
    }

    
    private void ShowLeaderEffectInfo()
    {
        //if (_currentCharacterSOs[0] == null) _leaderEffectText.text = "LeaderEffect : None";
        //else _leaderEffectText.text = $"LeaderEffect : {_currentCharacterSOs[0].LeaderEffectDescription}";
    }

    private void ShowCharacterCountInfo()
    {
        _characterCountText.text = $"보유 영웅 {_collectedCharacterData.CollectedCharacterCount}/{_collectedCharacterData.CharacterCount}";
        Debug.Log($"캐릭터 수 : {_collectedCharacterData.CollectedCharacterCount}");
    }

    private void ShowButtonPreset()
    {
        if (TempDataManager.Instance == null) return;

        for(int i = 0; i < TempDataManager.Instance.PresetData.Count - 2; i++)
        {
            _presetAddButton[i].buttonText = $"{(i + 3)}";
            _presetAddButton[i].UpdateUI();
        }

        for(int i = 0; i < _presetAddButton.Length; i++)
        {
            if(i <= TempDataManager.Instance.PresetData.Count- 2)
            {
                _presetAddButton[i].GetComponent<Button>().interactable = true;
            }
            else
            {
                _presetAddButton[i].GetComponent<Button>().interactable = false;
            }
        }
    }

    #endregion

    #region Preset

    public void SelectCharacterPreset(int index)
    {
        // 리더 캐릭터가 배치되지 않았을 시 경고 팝업 띄우기
        if (_currentPreset[0].Data == null)
        {
            if (PopupManager.Instance != null)
            {
                PopupManager.instance.ShowPopup("리더 캐릭터를 추가해주세요.");
            }

            return;
        }

        // 해당 프리셋이 생성되지 않은 프리셋일 시 확장 가능한지 확인하고, 확장을 진행      
        // TempDataManager 반영 이전
        //if (_presetData.Count < index + 1)
        if(TempDataManager.Instance.PresetData.Count < index + 1)
        {
            if (PopupManager.Instance != null)
            {
                PopupManager.instance.ShowConfirmationPopup("프리셋을 추가하시겠습니까?\n500 골드 소모", () => CreatePreset(index), null);
            }
        }
        else
        {
            LoadPreset(index);
        }
    }

    private void CreatePreset(int index)
    {
        // TODO : 금액이 부족할 시에 조건 추가

        Debug.Log("Used 500 Gold");
        
        // TempDataManager 반영 이전
        //_presetData.Add(new TeamPresetData(5));
        TempDataManager.Instance.CreatePreset(5);

        // 이 부분은 UI 디자인 변경 시 변경 필요
        _presetAddButton[index - 2].buttonText = $"{(index + 1)}";
        _presetAddButton[index - 2].UpdateUI();

        LoadPreset(index);
    }

    private void LoadPreset(int index)
    {
        // TempDataManager 반영 이전
        //_currentPreset = _presetData[index].Statuses;
        _currentPreset = TempDataManager.Instance.PresetData[index].Statuses;
           
        _currentCost = 0;
        _currentOverallPower = 0;
        for (int i = 0; i < _currentPreset.Length; i++)
        {
            if (_currentPreset[i].Data != null)
            {
                _currentCost += _currentPreset[i].Data.Cost;
                _currentOverallPower += _currentPreset[i].CombatPower;
            }
        }

        OnCharacterDataChanged?.Invoke();
    }

    #endregion

    #region Synergy

    public Dictionary<ClassType, int> GetClassSynergyCount()
    {
        Dictionary<ClassType, int> classSynergyCount = new();

        foreach(var unit in _currentPreset)
        {
            if(unit.Data == null) continue;
            if(unit.Data == null) continue;

            ClassType classSynergy = unit.Data.ClassSynergy;

            if (!classSynergyCount.ContainsKey(classSynergy))
                classSynergyCount[classSynergy] = 0;

            classSynergyCount[classSynergy]++;
            Debug.Log($"{classSynergy.ToString()} : {classSynergyCount[classSynergy]}");
        }
        
        return classSynergyCount;
    }

    public Dictionary<Synergy, int> GetSynergyCount()
    {
        Dictionary<Synergy, int> synergyCounts = new();

        foreach (var unit in _currentPreset)
        {
            if (unit.Data == null) continue;
            if (unit.Data == null) continue;

            Synergy synergy = unit.Data.Synergy;

            if (!synergyCounts.ContainsKey(synergy))
                synergyCounts[synergy] = 0;

            synergyCounts[synergy]++;
        }

        return synergyCounts;
    }

    #endregion
}