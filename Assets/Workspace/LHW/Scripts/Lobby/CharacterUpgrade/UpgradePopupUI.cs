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

    private int _usingPiece = 10;

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
        if (CanLevelUp(out int piece))
        {
            _currentCharUnit.LevelUp();
            TempDataManager.Instance.RemovePiece(piece);
            OnCharacterStatusChanged?.Invoke();
        }
    }

    private bool CanLevelUp(out int piece)
    {
        int level = _currentCharUnit.Status.Level;

        if(TempDataManager.Instance.CharPiece >= level * 10)
        {
            piece = level * 10;
            // UI 표기용 임시
            _usingPiece = piece + 10;
            return true;
        }

        piece = 0;
        return false;
    }

    #endregion

    private void UpdateUI()
    {
        if (_currentCharUnit != null)
        {
            // UI 표기
        }
        if (TempDataManager.Instance != null)
        {
            _tempText.text = $"캐릭터 정보 표기 예정\n조각 수 : {TempDataManager.Instance.CharPiece}\n 소모 조각 수: {_usingPiece}";
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