using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 편성할 수 있는 캐릭터를 표시하는 유닛
/// </summary>
public class CharacterUnit : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
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
        _manager = GetComponentInParent<TeamOrganizeManager>();
        GetComponent<Button>().onClick.AddListener(TryAddCharacter);
    }

    private void Start()
    {
        UIUpdate();
    }

    private void DataInit()
    {
        _status = new UnitStatus(_unitData, 1);
    }

    #region Event

    private void OnEnable()
    {
        if (_manager != null) _manager.OnCharacterDataChanged += UIUpdate;

        if (_isCollected)
        {
            GetComponent<Button>().interactable = true;
        }
        else
        {
            GetComponent<Button>().interactable = false;
        }
    }

    private void OnDisable()
    {
        if (_manager != null) _manager.OnCharacterDataChanged -= UIUpdate;
    }

    #endregion

    #region Onclick - Character Add

    private void TryAddCharacter()
    {
        if (_manager != null) _manager.AddPresetData(_status);
    }

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
        if (Manager.Data.SynergyDB != null)
        {
            _jobSynergyImg.sprite = Manager.Data.SynergyDB.GetSynergy((int)_status.Data.Synergy).Icon;
            _roleSynergyImg.sprite = Manager.Data.SynergyDB.GetSynergy((int)_status.Data.ClassSynergy).Icon;
        }
        _overallPowerText.text = $"{_status.CombatPower}";
        _levelText.text = $"Lv.{_status.Level}";
    }

    #endregion

    #region Data Input

    // 데이터 입력 관련 메소드

    #endregion
}