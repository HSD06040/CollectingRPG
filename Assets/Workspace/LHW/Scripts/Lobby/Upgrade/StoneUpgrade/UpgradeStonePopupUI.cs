using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeStonePopupUI : MonoBehaviour
{
    [Header("Stone Info")]
    [SerializeField] private Image _stoneIcon;
    [SerializeField] private TMP_Text _stoneLevelText;
    [SerializeField] private TMP_Text _stoneNameText;
    [SerializeField] private TMP_Text _stoneDescriptionText;
    [SerializeField] private TMP_Text _stoneProbableAddText;
    [SerializeField] private TMP_Text[] _stoneProbleText;

    [Header("Level Up Button UI")]
    [SerializeField] private TMP_Text _pieceText;
    [SerializeField] private Image _pieceGauge;
    [SerializeField] private TMP_Text _goldText;
    [SerializeField] private TMP_Text _openPieceText;
    [SerializeField] private Image _openPieceGauge;

    [Header("Button")]
    [SerializeField] private Button _levelUpButton;
    [SerializeField] private Button _openButton;
    [SerializeField] private Button _closeButton;

    private MagicStoneUpgradeUnit _currentMagicStoneUnit;

    public Action OnMagicStoneStatusChanged;

    private void Awake()
    {
        _closeButton.onClick.AddListener(CloseUI);
        _levelUpButton.onClick.AddListener(LevelUp);
        _openButton.onClick.AddListener(LevelUp);
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
        LevelUpButtonUpdate();
    }

    private void UpdateStoneInfo()
    {
        _stoneIcon.sprite = _currentMagicStoneUnit.Data.Icon;
        _stoneLevelText.text = $"LV.{_currentMagicStoneUnit.Data.UpgradeData.CurrentUpgradeData.UpgradeLevel}";
        _stoneNameText.text = _currentMagicStoneUnit.Data.Name;
        _stoneDescriptionText.text = _currentMagicStoneUnit.Data.Description;
    }

    private void UpdateStoneProbable()
    {
        // 마법석 현재 적용 확률 표시
    }

    private void LevelUpButtonUpdate()
    {
        if (_currentMagicStoneUnit.Data.UpgradeData.CurrentUpgradeData.UpgradeLevel == 0)
        {
            _openButton.gameObject.SetActive(true);
            _levelUpButton.gameObject.SetActive(false);
        }
        else
        {
            _openButton.gameObject.SetActive(false);
            _levelUpButton.gameObject.SetActive(true);
        }

        int requirePiece = _currentMagicStoneUnit.Data.UpgradeData.GetRequiredPiece();
        int requireGold = _currentMagicStoneUnit.Data.UpgradeData.GetRequiredGold();
        if (_currentMagicStoneUnit.Data.UpgradeData.CurrentUpgradeData.CurrentPieces == 0)
        {
            _pieceGauge.fillAmount = 0;
            _openPieceGauge.fillAmount = 0;
        }
        else
        {
            _pieceGauge.fillAmount = (float)_currentMagicStoneUnit.Data.UpgradeData.CurrentUpgradeData.CurrentPieces / requirePiece;
            _openPieceGauge.fillAmount = (float)_currentMagicStoneUnit.Data.UpgradeData.CurrentUpgradeData.CurrentPieces / 5;
        }
        _pieceText.text = $"{_currentMagicStoneUnit.Data.UpgradeData.CurrentUpgradeData.CurrentPieces}/{requirePiece}";
        _goldText.text = requireGold.ToString();
        _openPieceText.text = $"{_currentMagicStoneUnit.Data.UpgradeData.CurrentUpgradeData.CurrentPieces}/5";
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


    private void CloseUI()
    {
        gameObject.SetActive(false);
    }
}
