using Michsky.UI.ModernUIPack;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

[Serializable]
public class SelectedCharacters
{
    public CharacterSO[] CharLists;

    public SelectedCharacters(int size)
    {
        CharLists = new CharacterSO[size];
    }
}

public class TeamOrganizeManager : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private CollectedCharacterData _collectedCharacterData;

    [Header("Data")]
    [SerializeField] private List<SelectedCharacters> _selectedCharacters = new List<SelectedCharacters>();

    [Header("UI")]
    [SerializeField] private TMP_Text _costInfoText;
    [SerializeField] private TMP_Text _totalOverallPowerText;
    [SerializeField] private TMP_Text _leaderEffectText;
    [SerializeField] private TMP_Text _characterCountText;
    [SerializeField] private GameObject _popUpUI;
    [SerializeField] private TMP_Text _popUpText;
    [SerializeField] private ButtonManagerBasic[] _presetAddButton;

    [Header("Capacity")]
    [SerializeField] private int _totalCost = 10;
    public int TotalCost => _totalCost;

    public Action OnCharacterDataChanged;

    private int _currentCost;
    public int CurrentCost => _currentCost;
    private int _currentOverallPower;
    private CharacterSO _selectedCharacterSO;

    [SerializeField] private CharacterSO[] _currentCharacterSOs;
    private List<CharacterSO> _collectedCharData;

    private void Awake()
    {
        // 최초 생성 - 2칸
        if (_selectedCharacters.Count == 0)
        {
            for (int i = 0; i < 2; i++)
            {
                _selectedCharacters.Add(new SelectedCharacters(5));
            }
        }
        _currentCharacterSOs = _selectedCharacters[0].CharLists;
    }

    private void Start()
    {
        ShowCostInfo();
        ShowTotalOverallPowerInfo();
        ShowLeaderEffectInfo();
        ShowCharacterCountInfo();
    }

    #region Event

    private void OnEnable()
    {
        _collectedCharData = _collectedCharacterData.CollectedCharData;
        OnCharacterDataChanged += ShowCostInfo;
        OnCharacterDataChanged += ShowTotalOverallPowerInfo;
        OnCharacterDataChanged += ShowLeaderEffectInfo;
        OnCharacterDataChanged += ShowCharacterCountInfo;
    }

    private void OnDisable()
    {
        OnCharacterDataChanged -= ShowCostInfo;
        OnCharacterDataChanged -= ShowTotalOverallPowerInfo;
        OnCharacterDataChanged -= ShowLeaderEffectInfo;
        OnCharacterDataChanged -= ShowCharacterCountInfo;
    }

    #endregion

    #region Read Data

    public CharacterSO GetCurrentCharacterData(int index)
    {
        return _currentCharacterSOs[index];
    }

    #endregion

    #region Manual Selection

    /// <summary>
    /// 캐릭터를 수동으로 추가함
    /// </summary>
    /// <param name="data"></param>
    public void AddCharacterData(CharacterSO data)
    {
        _selectedCharacterSO = data;

        if (_currentCharacterSOs.Contains(data))
        {
            Debug.Log("이미 편성된 캐릭터입니다");
            return;
        }

        if (_currentCost + data.Cost > _totalCost)
        {
            Debug.Log("코스트 상한치를 초과했습니다");
            return;
        }

        for (int i = 0; i < _currentCharacterSOs.Length; i++)
        {
            if (_currentCharacterSOs[i] == null)
            {
                _currentCharacterSOs[i] = _selectedCharacterSO;
                _currentCost += _selectedCharacterSO.Cost;
                _currentOverallPower += _selectedCharacterSO.OverallPower;
                break;
            }

            if (i == _currentCharacterSOs.Length - 1)
            {
                Debug.Log("편성 제한치를 초과했습니다.");
                return;
            }
        }
        OnCharacterDataChanged?.Invoke();
    }

    /// <summary>
    /// 캐릭터를 수동으로 해제함
    /// </summary>
    /// <param name="index"></param>
    public void RemoveCharacterData(int index)
    {
        if (_currentCharacterSOs[index] != null)
        {
            _currentCost -= _currentCharacterSOs[index].Cost;
            _currentOverallPower -= _currentCharacterSOs[index].OverallPower;
            _currentCharacterSOs[index] = null;
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
        int n = _collectedCharData.Count;
        int[,,] dp = new int[n + 1, _totalCost + 1, _currentCharacterSOs.Length + 1];
        bool[,,] take = new bool[n + 1, _totalCost + 1, _currentCharacterSOs.Length + 1];

        // DP 진행 - Bottom-Up 방식
        for (int i = 1; i <= n; i++)
        {
            int power = _collectedCharData[i - 1].OverallPower;
            int cost = _collectedCharData[i - 1].Cost;

            for (int c = 0; c <= _totalCost; c++)
            {
                for (int k = 0; k <= _currentCharacterSOs.Length; k++)
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
            for (int k = 0; k <= _currentCharacterSOs.Length; k++)
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
        List<CharacterSO> bestTeam = new List<CharacterSO>();
        int ci = bestC;
        int ki = bestK;

        for (int i = n; i > 0; i--)
        {
            if (take[i, ci, ki])
            {
                bestTeam.Add(_collectedCharData[i - 1]);
                ci -= _collectedCharData[i - 1].Cost;
                ki -= 1;
            }
        }

        // 전투력이 높은 순으로 정렬
        bestTeam.OrderByDescending(n => n);

        // 기존 편성 초기화
        Array.Clear(_currentCharacterSOs, 0, _currentCharacterSOs.Length);
        _currentCost = 0;

        // 최적 편성 적용
        for (int i = 0; i < bestTeam.Count; i++)
        {
            _currentCharacterSOs[i] = bestTeam[i];
            _currentCost += bestTeam[i].Cost;
        }
        _currentOverallPower = bestPower;

        OnCharacterDataChanged?.Invoke();
    }

    #endregion

    #region UI Output

    private void ShowCostInfo()
    {
        _costInfoText.text = $"{_currentCost} / {_totalCost}";
    }

    private void ShowTotalOverallPowerInfo()
    {
        _totalOverallPowerText.text = $"OverallPower : {_currentOverallPower}";
    }

    private void ShowLeaderEffectInfo()
    {
        if (_currentCharacterSOs[0] == null) _leaderEffectText.text = "LeaderEffect : None";
        else _leaderEffectText.text = $"LeaderEffect : {_currentCharacterSOs[0].LeaderEffectDescription}";
    }

    private void ShowCharacterCountInfo()
    {
        _characterCountText.text = $"Character {_collectedCharacterData.CollectedCharacterCount}/{_collectedCharacterData.CharacterCount}";
    }

    #endregion

    #region Preset

    public void SelectCharacterPreset(int index)
    {
        // 리더 캐릭터가 배치되지 않았을 시 경고 팝업 띄우기
        if (_currentCharacterSOs[0] == null)
        {
            if (PopupManager.Instance != null)
            {
                PopupManager.instance.ShowPopup("Leader is not added.\nPlease add.");
            }

            return;
        }

        // 해당 프리셋이 생성되지 않은 프리셋일 시 확장 가능한지 확인하고, 확장을 진행
        if (_selectedCharacters.Count < index + 1)
        {
            if (PopupManager.Instance != null)
            {
                PopupManager.instance.ShowConfirmationPopup("Add Preset?\nConsumes 500 Gold.", () => CreatePreset(index), null);
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
        _selectedCharacters.Add(new SelectedCharacters(5));
        // 이 부분은 UI 디자인 변경 시 변경 필요
        _presetAddButton[index - 2].buttonText = $"{(index + 1)}";
        _presetAddButton[index - 2].UpdateUI();

        LoadPreset(index);
    }

    private void LoadPreset(int index)
    {
        _currentCharacterSOs = _selectedCharacters[index].CharLists;
        _currentCost = 0;
        _currentOverallPower = 0;
        for (int i = 0; i < _currentCharacterSOs.Length; i++)
        {
            if (_currentCharacterSOs[i] != null)
            {
                _currentCost += _currentCharacterSOs[i].Cost;
                _currentOverallPower += _currentCharacterSOs[i].OverallPower;
            }
        }

        OnCharacterDataChanged?.Invoke();
    }

    #endregion
}