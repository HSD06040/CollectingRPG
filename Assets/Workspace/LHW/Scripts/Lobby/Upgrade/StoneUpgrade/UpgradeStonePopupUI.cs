using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UpgradeStonePopupUI : MonoBehaviour, IPointerDownHandler
{
    [Header("Reference")]
    [SerializeField] private GameObject _backgroundPanel;

    [Header("Stone Info")]
    [SerializeField] private Image _stoneIcon;
    [SerializeField] private TMP_Text _stoneLevelText;
    [SerializeField] private TMP_Text _stoneNameText;
    [SerializeField] private TMP_Text _stoneDescriptionText;

    [Header("StoneProbable")]
    [SerializeField] private TMP_Text[] _currentStoneProbleText;
    [SerializeField] private TMP_Text[] _nextStoneProbleText;

    [Header("PieceGauge")]
    [SerializeField] private TMP_Text _pieceText;
    [SerializeField] private Image _pieceGauge;

    [Header("Level Up Button UI")]
    [SerializeField] private TMP_Text _openPieceText;
    [SerializeField] private GameObject _levelUpUI;
    [SerializeField] private TMP_Text _goldText;

    [Header("Button")]
    [SerializeField] private Button _levelUpButton;

    private MagicStoneUpgradeUnit _currentMagicStoneUnit;

    public Action OnMagicStoneStatusChanged;

    private void Awake()
    {
        _levelUpButton.onClick.AddListener(LevelUp);
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        OnMagicStoneStatusChanged += UpdateUI;
        UpdateUI();
    }

    private void OnDisable()
    {
        OnMagicStoneStatusChanged -= UpdateUI;
    }

    public void GetCurrentMagicStoneUnit(MagicStoneUpgradeUnit unit)
    {
        _currentMagicStoneUnit = unit;
    }

    private void UpdateUI()
    {
        UpdateStoneInfo();
        UpdateStoneProbable();
        PieceGaugeUpdate();
        LevelUpButtonUIUpdate();
    }

    private void UpdateStoneInfo()
    {
        _stoneIcon.sprite = _currentMagicStoneUnit.Data.Icon;
        _stoneNameText.text = _currentMagicStoneUnit.Data.Name;
        _stoneLevelText.text = $"Lv.{_currentMagicStoneUnit.Data.UpgradeData.CurrentUpgradeData.UpgradeLevel}";
        _stoneDescriptionText.text = _currentMagicStoneUnit.Data.Description;
    }

    private void UpdateStoneProbable()
    {
        List<SubGradeProb> currentProbs = new List<SubGradeProb>();
        List<SubGradeProb> nextProbs = new List<SubGradeProb>();
        if (_currentMagicStoneUnit.Data.UpgradeData.CurrentUpgradeData.UpgradeLevel != 10)
        {
            currentProbs = _currentMagicStoneUnit.Data.UpgradeProbData.
            GetCurrentLevelProbData(_currentMagicStoneUnit.Data.UpgradeData.CurrentUpgradeData.UpgradeLevel);

            nextProbs = _currentMagicStoneUnit.Data.UpgradeProbData.
                GetCurrentLevelProbData(_currentMagicStoneUnit.Data.UpgradeData.CurrentUpgradeData.UpgradeLevel + 1);

            for (int i = 0; i < _currentStoneProbleText.Length; i++)
            {
                _currentStoneProbleText[i].text = $"{currentProbs[i].Probable.ToString()}%";
                _nextStoneProbleText[i].text = $"{nextProbs[i].Probable.ToString()}%";
            }
        }
        else
        {
            currentProbs = _currentMagicStoneUnit.Data.UpgradeProbData.
            GetCurrentLevelProbData(_currentMagicStoneUnit.Data.UpgradeData.CurrentUpgradeData.UpgradeLevel - 1);

            nextProbs = _currentMagicStoneUnit.Data.UpgradeProbData.
                GetCurrentLevelProbData(_currentMagicStoneUnit.Data.UpgradeData.CurrentUpgradeData.UpgradeLevel);

            for (int i = 0; i < _currentStoneProbleText.Length; i++)
            {
                _currentStoneProbleText[i].text = $"{currentProbs[i].Probable.ToString()}%";
                _nextStoneProbleText[i].text = $"{nextProbs[i].Probable.ToString()}%";
            }
        }
    }

    private void PieceGaugeUpdate()
    {        
        if(_currentMagicStoneUnit.Data.UpgradeData.CurrentUpgradeData.UpgradeLevel == 0)
        {
            _pieceGauge.fillAmount = (float)_currentMagicStoneUnit.Data.UpgradeData.CurrentUpgradeData.CurrentPieces / 5;
            _pieceText.text = $"{_currentMagicStoneUnit.Data.UpgradeData.CurrentUpgradeData.CurrentPieces}/5";
        }
        else if(_currentMagicStoneUnit.Data.UpgradeData.CurrentUpgradeData.UpgradeLevel < 10)
        {
            int requirePiece = _currentMagicStoneUnit.Data.UpgradeData.GetRequiredPiece();
            _pieceGauge.fillAmount = (float)_currentMagicStoneUnit.Data.UpgradeData.CurrentUpgradeData.CurrentPieces / requirePiece;
            _pieceText.text = $"{_currentMagicStoneUnit.Data.UpgradeData.CurrentUpgradeData.CurrentPieces}/{requirePiece}";
        }
        else
        {
            _pieceGauge.fillAmount = 1;
            _pieceText.text = "MAX";
        }
    }

    private void LevelUpButtonUIUpdate()
    {
        if (_currentMagicStoneUnit.Data.UpgradeData.CurrentUpgradeData.UpgradeLevel == 0)
        {
            _openPieceText.text = "해금하기";
            _openPieceText.gameObject.SetActive(true);
            _levelUpUI.gameObject.SetActive(false);
        }
        else if(_currentMagicStoneUnit.Data.UpgradeData.CurrentUpgradeData.UpgradeLevel < 10)
        {
            int requireGold = _currentMagicStoneUnit.Data.UpgradeData.GetRequiredGold();
            _goldText.text = requireGold.ToString();
            _openPieceText.gameObject.SetActive(false);
            _levelUpUI.gameObject.SetActive(true);
        }
        else
        {
            _openPieceText.text = "최대 레벨";
            _openPieceText.gameObject.SetActive(true);
            _levelUpUI.gameObject.SetActive(false);
        }
    }

    private async void LevelUp()
    {
        if (_currentMagicStoneUnit.Data.UpgradeData.CurrentUpgradeData.UpgradeLevel >= 10)
        {
            if (PopupManager.Instance != null)
            {
                PopupManager.instance.ShowPopup("이미 최대 레벨입니다.");
                return;
            }
        }

        bool success = await _currentMagicStoneUnit.Data.UpgradeData.LevelUpWithPieces();

        if (!success)
        {
            if (PopupManager.Instance != null)
            {
                PopupManager.instance.ShowPopup("조각이 부족합니다.");
                return;
            }
        }
        else
        {
            await DBManager.Instance.magicStoneDB.SaveMagicStoneUpgradeData(_currentMagicStoneUnit.Data);
            OnMagicStoneStatusChanged?.Invoke();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(eventData.pointerEnter.gameObject == _backgroundPanel)
        {
            gameObject.SetActive(false);
        }
    }
}
