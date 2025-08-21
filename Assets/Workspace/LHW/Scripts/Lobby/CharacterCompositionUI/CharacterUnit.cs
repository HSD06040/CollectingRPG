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
    [SerializeField] private CharacterSO _charData;
    public CharacterSO CharData => _charData;

    [Header("UI")]
    [SerializeField] private TMP_Text _charText;
    [SerializeField] private Image _characterImg;
    [SerializeField] private Image _costImg;
    [SerializeField] private Image _jobSynergyImg;
    [SerializeField] private Image _roleSynergyImg;
    [SerializeField] private TMP_Text _overallPowerText;
    [SerializeField] private TMP_Text _levelText;

    private bool _isCollected = true;
    public bool IsCollected => _isCollected;

    private TeamOrganizeManager _manager;
    
    [SerializeField] private float _requiredPointerDownTime = 2f;
    private Coroutine _holdCoroutine;
    
    private void Awake()
    {
        _manager = GetComponentInParent<TeamOrganizeManager>();
        GetComponent<Button>().onClick.AddListener(TryAddCharacter);
    }

    private void Start()
    {
        UIUpdate();
    }

    #region Event

    private void OnEnable()
    {
        _manager.OnCharacterDataChanged += UIUpdate;

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
        _manager.OnCharacterDataChanged -= UIUpdate;
    }

    #endregion

    #region Onclick - Character Add

    private void TryAddCharacter()
    {
        _manager.AddCharacterData(_charData);
    }

    #endregion

    #region OnPointerDown - Character Info UI PopUp

    public void OnPointerDown(PointerEventData eventData)
    {
        _holdCoroutine = StartCoroutine(HoldRoutine());
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if(_holdCoroutine != null)
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
        _charText.text = $"{_charData.name}";
        _characterImg.sprite = _charData.CharacterImage;
        _costImg.sprite = _charData.CostImg;
        _jobSynergyImg.sprite = _charData.CharacterSynergy.JobSynergy;
        _roleSynergyImg.sprite = _charData.CharacterSynergy.RoleSynergy;
        _overallPowerText.text = $"Damage {_charData.OverallPower}";
        _levelText.text = $"Lv{_charData.Level}";
    }

    #endregion

    #region Data Input

    public void InputData(CharacterSO data)
    {
        _charData = data;
    }    

    #endregion
}