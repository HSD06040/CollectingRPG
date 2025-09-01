using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterUpgradeUnit : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [Header("Data Input")]
    private UnitStatus _status;
    public UnitStatus Status => _status;
    [SerializeField] UnitData _unitData;

    [Header("UI")]
    [SerializeField] private TMP_Text _charText;
    [SerializeField] private Image _characterImg;
    [SerializeField] private Image _costImg;
    [SerializeField] private Image _jobSynergyImg;
    [SerializeField] private Image _roleSynergyImg;
    [SerializeField] private TMP_Text _overallPowerText;
    [SerializeField] private TMP_Text _levelText;

    [Header("Reference")]
    [SerializeField] Sprite[] costSprites;


    private bool _isCollected = true;
    public bool IsCollected => _isCollected;

    private TeamOrganizeManager _manager;

    [SerializeField] private float _requiredPointerDownTime = 2f;
    private Coroutine _holdCoroutine;

    private void Awake()
    {
        DataInit();
    }

    private void Start()
    {
        UIUpdate();
    }

    private void DataInit()
    {
        _status = new UnitStatus(_unitData, 1);
    }

    #region Onclick

    // 업그레이드 UI 띄우기

    #endregion

    #region OnPointerDown - Character Info UI PopUp

    public void OnPointerDown(PointerEventData eventData)
    {
        _holdCoroutine = StartCoroutine(HoldRoutine());
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (_holdCoroutine != null)
        {
            StopCoroutine(_holdCoroutine);
            _holdCoroutine = null;
        }
    }

    private IEnumerator HoldRoutine()
    {
        yield return new WaitForSeconds(_requiredPointerDownTime);

        // UI 활성화
        Debug.Log("UI 활성화");
    }

    #endregion

    #region UI Update

    private void UIUpdate()
    {
        _charText.text = $"{_status.Data.Name}";
        _characterImg.sprite = _status.Data.Icon;
        _costImg.sprite = costSprites[_status.Data.Cost - 1];
        _jobSynergyImg.sprite = SynergyController.SynergyDB.GetSynergy((int)_status.Data.Synergy).Icon;
        _roleSynergyImg.sprite = SynergyController.SynergyDB.GetSynergy((int)_status.Data.Synergy).Icon;
        _overallPowerText.text = $"{_status.CombatPower}";
        _levelText.text = $"Lv.{_status.Level}";
    }

    #endregion

    #region Data Input

    // 데이터 입력 관련 메소드

    #endregion
}
