using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeCharacterPopupUI : MonoBehaviour
{
    [Header("Character Profile")]
    [SerializeField] private TMP_Text _gradeText;
    [SerializeField] private TMP_Text _characterNameText;
    [SerializeField] private TMP_Text _characterLevelText;
    [SerializeField] private Image _characterImage;
    [SerializeField] private Image _costImage;
    [SerializeField] private Image _synergyImage;
    [SerializeField] private Image _classImage;

    [SerializeField] private ImageSO _costImages;

    [Header("Character Status")]
    [SerializeField] private TMP_Text[] _statuses;

    [Header("Character Upgrade Info")]
    [SerializeField] private Image _skillIcon;
    [SerializeField] private TMP_Text _skillNameText;
    [SerializeField] private TMP_Text _skillDescriptionText;

    [Header("Level Up Button UI")]
    [SerializeField] private TMP_Text _pieceText;
    [SerializeField] private Image _pieceGauge;
    [SerializeField] private TMP_Text _openPieceText;
    [SerializeField] private Image _openPieceGauge;

    [Header("Button")]
    [SerializeField] private Button _levelUpButton;
    [SerializeField] private Button _openButton;
    [SerializeField] private Button _closeButton;

    private CharacterUpgradeUnit _currentCharUnit;

    public Action OnCharacterStatusChanged;

    private void Awake()
    {
        _closeButton.onClick.AddListener(CloseUI);
        _levelUpButton.onClick.AddListener(LevelUp);
        _openButton.onClick.AddListener(LevelUp);
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        OnCharacterStatusChanged += UpdateUI;
        UpdateUI();
    }

    private void OnDisable()
    {
        OnCharacterStatusChanged -= UpdateUI;
    }

    #region Read Data

    public void GetCurrentCharacterUnitData(CharacterUpgradeUnit charUnit)
    {
        _currentCharUnit = charUnit;
    }

    #endregion

    #region Level Up

    private async void LevelUp()
    {
        if (_currentCharUnit.Status.Data.UpgradeData.CurrentUpgradeData.UpgradeLevel >= 10)
        {
            if (PopupManager.Instance != null)
            {
                PopupManager.instance.ShowPopup("이미 최대 레벨입니다.");
                return;
            }
        }
        _currentCharUnit.Status.Data.UpgradeData.LevelUp();

        await DBManager.Instance.charDB.SaveCharacterUpgradeData(_currentCharUnit.Status.Data);

        OnCharacterStatusChanged?.Invoke();
    }

    #endregion

    #region Update UI

    private void UpdateUI()
    {
        if (_currentCharUnit != null && _currentCharUnit.Status.Data.UpgradeData != null && _currentCharUnit.Status.Data.LevelUpData != null)
        {
            // UI 표기
            CharacterProfileUpdate();
            CharacterStatusUpdate();
            LevelUpButtonUpdate();
            SkillUIUpdate();
        }
    }

    private void CharacterProfileUpdate()
    {
        _gradeText.text = _currentCharUnit.Status.Data.Grade.ToString();
        _characterNameText.text = _currentCharUnit.Status.Data.Name;
        _characterLevelText.text = $"LV.{_currentCharUnit.Status.Data.UpgradeData.CurrentUpgradeData.UpgradeLevel.ToString()}";
        _characterImage.sprite = _currentCharUnit.Status.Data.Icon;
        _costImage.sprite = _costImages.CostSprites[_currentCharUnit.Status.Data.Cost - 1];
        if (Manager.Data.SynergyDB != null)
        {
            _synergyImage.sprite = Manager.Data.SynergyDB.GetSynergy((int)_currentCharUnit.Status.Data.Synergy).Icon;
            _classImage.sprite = Manager.Data.SynergyDB.GetSynergy((int)_currentCharUnit.Status.Data.ClassSynergy).Icon;
        }
    }

    private void CharacterStatusUpdate()
    {
        int level = _currentCharUnit.Status.Data.UpgradeData.CurrentUpgradeData.UpgradeLevel;
        _statuses[0].text = _currentCharUnit.Status.Data.UpgradeStats[0].MaxHealth.ToString();
        _statuses[1].text = _currentCharUnit.Status.Data.UpgradeStats[0].MaxMana.ToString();
        _statuses[2].text = _currentCharUnit.Status.Data.UpgradeStats[0].PhysicalDamage.ToString();
        _statuses[3].text = _currentCharUnit.Status.Data.UpgradeStats[0].MagicDamage.ToString();
        _statuses[4].text = _currentCharUnit.Status.Data.UpgradeStats[0].PhysicalDefense.ToString();
        _statuses[5].text = _currentCharUnit.Status.Data.UpgradeStats[0].MagicDefense.ToString();
        _statuses[6].text = _currentCharUnit.Status.Data.UpgradeStats[0].AttackRange.ToString();
        _statuses[7].text = _currentCharUnit.Status.Data.UpgradeStats[0].AttackSpeed.ToString();
        _statuses[8].text = _currentCharUnit.Status.Data.UpgradeStats[0].CritChance.ToString();
        _statuses[9].text = _currentCharUnit.Status.CombatPower.ToString();
    }

    private void LevelUpButtonUpdate()
    {
        if (_currentCharUnit.Status.Data.UpgradeData.CurrentUpgradeData.UpgradeLevel == 0)
        {
            _openButton.gameObject.SetActive(true);
            _levelUpButton.gameObject.SetActive(false);
        }
        else
        {
            _openButton.gameObject.SetActive(false);
            _levelUpButton.gameObject.SetActive(true);
        }

        int requirePiece = _currentCharUnit.Status.Data.UpgradeData.GetRequiredPiece();
        if (_currentCharUnit.Status.Data.UpgradeData.CurrentUpgradeData.CurrentPieces == 0)
        {
            _pieceGauge.fillAmount = 0;
            _openPieceGauge.fillAmount = 0;
        }
        else
        {
            _pieceGauge.fillAmount = (float)_currentCharUnit.Status.Data.UpgradeData.CurrentUpgradeData.CurrentPieces / requirePiece;
            _openPieceGauge.fillAmount = (float)_currentCharUnit.Status.Data.UpgradeData.CurrentUpgradeData.CurrentPieces / requirePiece;
        }
        _pieceText.text = $"{_currentCharUnit.Status.Data.UpgradeData.CurrentUpgradeData.CurrentPieces}/{requirePiece}";
        _openPieceText.text = $"{_currentCharUnit.Status.Data.UpgradeData.CurrentUpgradeData.CurrentPieces}/{requirePiece}";
    }

    private void SkillUIUpdate()
    {
        _skillIcon.sprite = _currentCharUnit.Status.Data.Skill.Icon;
        _skillNameText.text = _currentCharUnit.Status.Data.Skill.SkillName;
        _skillDescriptionText.text = _currentCharUnit.Status.Data.Skill.Description;
    }

    #endregion

    #region CloseUI

    private void CloseUI()
    {
        gameObject.SetActive(false);
    }

    #endregion
}