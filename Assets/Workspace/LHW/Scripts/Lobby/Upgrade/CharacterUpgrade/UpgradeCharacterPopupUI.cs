using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UpgradeCharacterPopupUI : MonoBehaviour, IPointerDownHandler
{
    [Header("Reference")]
    [SerializeField] private GameObject _backgroundPanel;
    [SerializeField] private Sprite[] _gradeIconSprites;

    [Header("Character Profile")]
    [SerializeField] private Image _gradeIcon;
    [SerializeField] private TMP_Text _gradeText;
    [SerializeField] private TMP_Text _characterLevelText;
    [SerializeField] private Image _characterImage;
    [SerializeField] private TMP_Text _pieceText;
    [SerializeField] private Image _pieceGauge;
    [SerializeField] private TMP_Text _characterNameText;
    [SerializeField] private TMP_Text _characterCostText;
    [SerializeField] private Image _synergyImage;
    [SerializeField] private Image _classImage;

    [Header("CombatPower UI")]
    [SerializeField] private TMP_Text _combatPowerText;

    [Header("Character Status")]
    [SerializeField] private TMP_Text[] _statuses;

    [Header("Character Skill Info")]
    [SerializeField] private Image _skillIcon;
    [SerializeField] private TMP_Text _skillNameText;
    [SerializeField] private TMP_Text _skillDescriptionText;
    [SerializeField] private TMP_Text _skillManaConsume;

    [Header("Character Upgrade Info")]
    [SerializeField] private TMP_Text[] _characterUpgradeLevelText;
    [SerializeField] private TMP_Text[] _characterUpgradeDescriptionText;
    [SerializeField] private GameObject[] _upgradeStatusDisablePanel;

    [Header("Level Up Button UI")]
    [SerializeField] private TMP_Text _openPieceText;
    [SerializeField] private GameObject _goldObject;
    [SerializeField] private TMP_Text _goldText;

    [Header("Button")]
    [SerializeField] private Button _levelUpButton;

    private CharacterUpgradeUnit _currentCharUnit;

    public Action OnCharacterStatusChanged;

    private void Awake()
    {
        _levelUpButton.onClick.AddListener(LevelUp);
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

        bool success = await _currentCharUnit.Status.Data.UpgradeData.LevelUpWithPiecesOnly();

        if (!success && _currentCharUnit.Status.Data.UpgradeData.CurrentUpgradeData.UpgradeLevel >= 1)
        {
            bool canUseMyth = await _currentCharUnit.Status.Data.UpgradeData.CanLevelUpWithMythStone();

            if (!canUseMyth)
            {
                if (PopupManager.Instance != null)
                {
                    PopupManager.instance.ShowPopup("조각이 부족합니다.");
                    return;
                }
            }

            if (PopupManager.Instance != null)
            {
                int currentMythstone = await _currentCharUnit.Status.Data.UpgradeData.GetCurrentMythStoneCount();
                int usedMythstone = _currentCharUnit.Status.Data.UpgradeData.GetRequiredMythStoneForNextLevel(out int requiredPiece);

                PopupManager.instance.ShowConfirmationPopup(
                    $"조각이 부족합니다. 신화석을 사용해서 레벨업 하시겠습니까?\n보유 마법석:{currentMythstone}\n 사용 마법석:{usedMythstone}->부족 조각 수:{requiredPiece}",
                    async () =>
                    {
                        await _currentCharUnit.Status.Data.UpgradeData.LevelUpWithMythStone();
                        await DBManager.Instance.charDB.SaveCharacterUpgradeData(_currentCharUnit.Status.Data);
                        OnCharacterStatusChanged?.Invoke();

                    },
                    () =>
                    {
                        PopupManager.instance.ShowPopup("레벨업이 취소되었습니다.");
                    }
                );
            }
        }
        else
        {
            await DBManager.Instance.charDB.SaveCharacterUpgradeData(_currentCharUnit.Status.Data);
            OnCharacterStatusChanged?.Invoke();
        }
    }

    #endregion

    #region Update UI

    private void UpdateUI()
    {
        if (_currentCharUnit != null && _currentCharUnit.Status.Data.UpgradeData != null && _currentCharUnit.Status.Data.LevelUpData != null)
        {
            CharacterProfileUpdate();
            CombatPowerUpdate();
            CharacterStatusUpdate();
            LevelUpButtonUpdate();
            SkillUIUpdate();
            StatusUpgradeUIUpdate();
        }
    }

    #region CharacterProfileUIUpdate

    private void CharacterProfileUpdate()
    {
        _gradeIcon.sprite = _gradeIconSprites[(int)_currentCharUnit.Status.Data.Grade];
        _gradeText.text = _currentCharUnit.Status.Data.Grade.ToString();
        _characterLevelText.text = $"Lv.{_currentCharUnit.Status.Data.UpgradeData.CurrentUpgradeData.UpgradeLevel.ToString()}";
        GaugeUpdate();
        _characterNameText.text = _currentCharUnit.Status.Data.Name;
        _characterImage.sprite = _currentCharUnit.Status.Data.Icon;
        _characterCostText.text = _currentCharUnit.Status.Data.Cost.ToString();
        if (Manager.Data.SynergyDB != null)
        {
            _synergyImage.sprite = Manager.Data.SynergyDB.GetSynergy((int)_currentCharUnit.Status.Data.Synergy).ActiveIcon;
            _classImage.sprite = Manager.Data.SynergyDB.GetSynergy((int)_currentCharUnit.Status.Data.ClassSynergy).ActiveIcon;
        }
    }

    private void GaugeUpdate()
    {
        if (_currentCharUnit.Status.Data.UpgradeData.CurrentUpgradeData.UpgradeLevel == 0)
        {
            _pieceGauge.fillAmount = (float)_currentCharUnit.Status.Data.UpgradeData.CurrentUpgradeData.CurrentPieces / 10;
            _pieceText.text = $"{_currentCharUnit.Status.Data.UpgradeData.CurrentUpgradeData.CurrentPieces}/10";
        }
        else
        {
            int requirePiece = _currentCharUnit.Status.Data.UpgradeData.GetRequiredPiece();

            _pieceGauge.fillAmount = (float)_currentCharUnit.Status.Data.UpgradeData.CurrentUpgradeData.CurrentPieces / requirePiece;
            _pieceText.text = $"{_currentCharUnit.Status.Data.UpgradeData.CurrentUpgradeData.CurrentPieces}/{requirePiece}";
        }
    }

    #endregion

    #region CombatPowerUpdate

    private void CombatPowerUpdate()
    {
        _combatPowerText.text = _currentCharUnit.Status.CombatPower.ToString();
    }

    #endregion

    #region CharacterStatusUpdate

    private void CharacterStatusUpdate()
    {
        int level = _currentCharUnit.Status.Data.UpgradeData.CurrentUpgradeData.UpgradeLevel;
        _statuses[0].text = _currentCharUnit.Status.Data.UpgradeStats[0].MaxHealth.ToString();
        _statuses[1].text = _currentCharUnit.Status.Data.UpgradeStats[0].MaxMana.ToString();
        _statuses[2].text = _currentCharUnit.Status.Data.UpgradeStats[0].PhysicalDamage.ToString();
        _statuses[3].text = _currentCharUnit.Status.Data.UpgradeStats[0].MagicDamage.ToString();
        _statuses[4].text = _currentCharUnit.Status.Data.UpgradeStats[0].PhysicalDefense.ToString();
        _statuses[5].text = _currentCharUnit.Status.Data.UpgradeStats[0].MagicDefense.ToString();
        _statuses[6].text = _currentCharUnit.Status.Data.UpgradeStats[0].AttackSpeed.ToString();
        _statuses[7].text = _currentCharUnit.Status.Data.UpgradeStats[0].CritChance.ToString();
    }

    #endregion

    #region SkillUIUpdate

    private void SkillUIUpdate()
    {
        UnitStatus status = _currentCharUnit.Status;

        _skillIcon.sprite = status.Data.Skill.Icon;
        _skillNameText.text = status.Data.Skill.SkillName;
        _skillDescriptionText.text = GetDescription(status.Data.Skill, status);
        _skillManaConsume.text = $"소모 마나 : {status.Data.Skill.ManaCost}";
    }

    private string GetDescription(UnitSkill skill, UnitStatus status)
    {
        return skill.Description.Replace("{value}", skill.GetCalculateValueString(status));
    }

    #endregion

    #region StatusUIUpdate

    private void StatusUpgradeUIUpdate()
    {
        for (int i = 0; i < _characterUpgradeLevelText.Length; i++)
        {
            _characterUpgradeLevelText[i].text = $"LV.{2 * (i + 1)}";

            List<StatusGrowth> statusGrowths = _currentCharUnit.Status.Data.UpgradeStatData.GetCurrentStatusData(_currentCharUnit.Status.Data.Grade, 2 * (i + 1));

            StringBuilder sb = new StringBuilder();
            for (int j = 0; j < statusGrowths.Count; j++)
            {
                sb.Append($"{StatTypeTranslate(statusGrowths[j].Type)} {statusGrowths[j].Value} ");
                if (j != statusGrowths.Count - 1 && j % 2 == 1) sb.Append("\n");
            }

            sb.Append("증가");

            _characterUpgradeDescriptionText[i].text = sb.ToString();
        }

        ActiveUpgradeStatusInfo();
    }

    private void ActiveUpgradeStatusInfo()
    {
        int currentUpgradeLevel = _currentCharUnit.Status.Data.UpgradeData.CurrentUpgradeData.UpgradeLevel / 2;

        for (int i = 0; i < currentUpgradeLevel; i++)
        {
            _upgradeStatusDisablePanel[i].SetActive(false);
        }
        for (int i = currentUpgradeLevel; i < _upgradeStatusDisablePanel.Length; i++)
        {
            _upgradeStatusDisablePanel[i].SetActive(true);
        }
    }

    private string StatTypeTranslate(StatType type)
    {
        string text = "";

        switch (type)
        {
            case StatType.MaxHealth: text = "최대체력"; break;
            case StatType.MaxMana: text = "최대마나"; break;
            case StatType.ManaGain: text = "마나 회복속도"; break;
            case StatType.AttackSpeed: text = "공격속도"; break;
            case StatType.MoveSpeed: text = "공격속도"; break;
            case StatType.PhysicalDamage: text = "물리공격력"; break;
            case StatType.MagicDamage: text = "마법공격력"; break;
            case StatType.CritChance: text = "크리티컬확률"; break;
            case StatType.CritDamage: text = "크리티컬 데미지"; break;
            case StatType.PhysicalDefense: text = "물리방어력"; break;
            case StatType.MagicDefense: text = "마법방어력"; break;
            case StatType.Shield: text = "쉴드"; break;
            case StatType.AttackRange: text = "공격범위"; break;
            case StatType.AttackCount: text = "공격횟수"; break;
            case StatType.CurHp: text = "현재체력"; break;
            case StatType.CurMana: text = "현재마나"; break;
        }
        return text;
    }

    #endregion

    #region LevelUpButtonUpdate

    private void LevelUpButtonUpdate()
    {
        if (_currentCharUnit.Status.Data.UpgradeData.CurrentUpgradeData.UpgradeLevel == 0)
        {
            _openPieceText.gameObject.SetActive(true);
            _goldObject.SetActive(false);
            _openPieceText.text = "해금하기";
        }
        else if (_currentCharUnit.Status.Data.UpgradeData.CurrentUpgradeData.UpgradeLevel == 0)
        {
            _openPieceText.gameObject.SetActive(true);
            _goldObject.SetActive(false);
            _openPieceText.text = "최대레벨";
        }
        else
        {
            _openPieceText.gameObject.SetActive(false);
            _goldObject.SetActive(true);
            int requireGold = _currentCharUnit.Status.Data.UpgradeData.GetRequiredGold();
            _goldText.text = requireGold.ToString();
        }
    }

    #endregion

    #endregion

    #region CloseUI

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.pointerEnter.gameObject == _backgroundPanel)
        {
            gameObject.SetActive(false);
        }
    }

    #endregion
}