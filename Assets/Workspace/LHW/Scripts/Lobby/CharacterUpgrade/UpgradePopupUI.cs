using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UpgradePopupUI : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private GameObject _backgroundPanel;
    [SerializeField] private Button _levelUpButton;
    [SerializeField] private TMP_Text _tempText;

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
        if(_currentCharUnit.UpgradeData.UpgradeLevel >= 10)
        {
            if(PopupManager.Instance != null)
            {
                PopupManager.instance.ShowPopup("이미 최대 레벨입니다.");
                return;
            }
        }
        _currentCharUnit.UpgradeData.LevelUp();
        //TempDataManager.Instance.LevelUp();

        OnCharacterStatusChanged?.Invoke();
    }

    #endregion

    private void UpdateUI()
    {
        if (_currentCharUnit != null)
        {
            // UI 표기
            _tempText.text = $"캐릭터 정보 표기 예정\n조각 수 : {_currentCharUnit.UpgradeData.CurrentPieces.ToString()}\n 소모 조각 수: {_currentCharUnit.UpgradeData.GetRequiredPiece().ToString()}";
        }
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