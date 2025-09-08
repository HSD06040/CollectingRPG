using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UpgradePopupUI : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private GameObject _backgroundPanel;
    [SerializeField] private Button _levelUpButton;
    [SerializeField] private TMP_Text _pieceText;
    [SerializeField] private Image _pieceGauge;


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

    private void LevelUp()
    {
        //if(TempDataManager.Instance.UpgradeData.UpgradeLevel >= 10)
        if(_currentCharUnit.Status.Data.UpgradeData.CurrentUpgradeData.UpgradeLevel >= 10)
        {
            if(PopupManager.Instance != null)
            {
                PopupManager.instance.ShowPopup("이미 최대 레벨입니다.");
                return;
            }
        }
        _currentCharUnit.Status.Data.UpgradeData.LevelUp();
        //TempDataManager.Instance.LevelUp();

        OnCharacterStatusChanged?.Invoke();
    }

    #endregion

    private void UpdateUI()
    {
        if (_currentCharUnit != null && _currentCharUnit.Status.Data.UpgradeData != null && _currentCharUnit.Status.Data.LevelUpData != null)
        {
            // UI 표기
            PieceGaugeUpdate();
        }
    }

    private void PieceGaugeUpdate()
    {
        int requirePiece = _currentCharUnit.Status.Data.UpgradeData.GetRequiredPiece();
        if (_currentCharUnit.Status.Data.UpgradeData.CurrentUpgradeData.CurrentPieces == 0)
        {
            _pieceGauge.fillAmount = 0;
        }
        else
        {
            _pieceGauge.fillAmount = (float)_currentCharUnit.Status.Data.UpgradeData.CurrentUpgradeData.CurrentPieces / requirePiece;
        }
        _pieceText.text = $"{_currentCharUnit.Status.Data.UpgradeData.CurrentUpgradeData.CurrentPieces}/{requirePiece}";
    }

    #region CloseUI

    public void OnPointerClick(PointerEventData eventData)
    {
        GameObject clickedObject = eventData.pointerCurrentRaycast.gameObject;
        if(clickedObject == _backgroundPanel)
        {
            gameObject.SetActive(false);
        }
    }

    #endregion
}