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

    [Header("Capacity")]
    [SerializeField] private int _totalCost = 10;

    public Action OnCharacterDataChanged;

    private int _currentCost;
    private int _currentOverallPower;
    private CharacterSO _selectedCharacterSO;

    [SerializeField] private CharacterSO[] _currentCharacterSOs;
    private List<CharacterSO> _collectedCharData;

    private void Awake()
    {
        for (int i = 0; i < 3; i++)
        {
            _selectedCharacters.Add(new SelectedCharacters(5));
        }
        _currentCharacterSOs = _selectedCharacters[0].CharLists;
    }

    #region Event

    private void OnEnable()
    {
        OnCharacterDataChanged += ShowCostInfo;
        OnCharacterDataChanged += ShowTotalOverallPowerInfo;
        ShowCostInfo();
        ShowTotalOverallPowerInfo();
        _collectedCharData = _collectedCharacterData.CollectedCharData;
    }

    private void OnDisable()
    {
        OnCharacterDataChanged -= ShowCostInfo;
        OnCharacterDataChanged -= ShowTotalOverallPowerInfo;
    }

    #endregion

    #region

    public CharacterSO GetCurrentCharacterData(int index)
    {
        return _currentCharacterSOs[index];
    }

    #endregion

    #region Manual Selection

    public void AddCharacterData(CharacterSO data)
    {
        _selectedCharacterSO = data;

        if (_currentCharacterSOs.Contains(data))
        {
            Debug.Log("�̹� ���� ĳ�����Դϴ�");
            return;
        }

        if (_currentCost + data.Cost > _totalCost)
        {
            Debug.Log("�ڽ�Ʈ ����ġ�� �ʰ��߽��ϴ�");
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
                Debug.Log("�� ����ġ�� �ʰ��߽��ϴ�.");
                return;
            }
        }
        OnCharacterDataChanged?.Invoke();
    }

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
    /// ���� ��ȹ�� �˰����� �̿��� ĳ���� �ڵ���
    /// </summary>
    public void AutoSelectCharacters()
    {
        int n = _collectedCharData.Count;
        int[,,] dp = new int[n + 1, _totalCost + 1, _currentCharacterSOs.Length + 1];
        bool[,,] take = new bool[n + 1, _totalCost + 1, _currentCharacterSOs.Length + 1];

        // DP ���� - Bottom-Up ���
        for (int i = 1; i <= n; i++)
        {
            int power = _collectedCharData[i - 1].OverallPower;
            int cost = _collectedCharData[i - 1].Cost;

            for (int c = 0; c <= _totalCost; c++)
            {
                for (int k = 0; k <= _currentCharacterSOs.Length; k++)
                {
                    // ���� ����
                    dp[i, c, k] = dp[i - 1, c, k];

                    // ���� ������ ��
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

        // ���� �� ã��
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

        // ������
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

        // �������� ���� ������ ����
        bestTeam.OrderByDescending(n => n);

        // ���� �� �ʱ�ȭ
        Array.Clear(_currentCharacterSOs, 0, _currentCharacterSOs.Length);
        _currentCost = 0;

        // ���� �� ����
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
        Debug.Log("����");
    }

    #endregion

    #region Preset

    public void SelectCharacterPreset(int index)
    {
        _currentCharacterSOs = _selectedCharacters[index].CharLists;
        _currentCost = 0;
        _currentOverallPower = 0;
        for(int i = 0; i <  _currentCharacterSOs.Length; i++)
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