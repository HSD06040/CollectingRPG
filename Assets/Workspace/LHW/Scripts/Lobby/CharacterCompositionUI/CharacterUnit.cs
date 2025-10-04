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
    [SerializeField] private Image _cardImage;
    [SerializeField] private Image _characterImg;
    [SerializeField] private Image _roleSynergyImg;
    [SerializeField] private TMP_Text _levelText;
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private Image _selectedImage;

    [Header("Reference")]
    [SerializeField] private Sprite[] _gradeSprite;


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

    public void Setup(UnitData data)
    {
        _unitData = data;
        _status = new UnitStatus(data);

        UIUpdate();
    }    

    #region Event

    private void OnEnable()
    {
        if (_manager != null)
        {
            _manager.OnCharacterDataChanged += UIUpdate;

            if(Status != null && Status.Data != null)
                UIUpdate();
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
        _cardImage.sprite = _gradeSprite[(int)_status.Data.Grade];
        _characterImg.sprite = _status.Data.Icon;
        if (Manager.Data.SynergyDB != null)
        {
            _roleSynergyImg.sprite = Manager.Data.SynergyDB.GetSynergy((int)_status.Data.ClassSynergy).Icon;
        }
        _levelText.text = $"Lv.{_status.Level}";
        _nameText.text = _status.Data.Name;

        SelectedUpdate();
    }

    private void SelectedUpdate()
    {
        // 캐릭터 편성 칸이 5칸에서 다른 크기로 변경될 가능성이 적다는 전제로
        // 직접 5를 집어넣음, 유동성을 주고 싶다면 _currentUnit의 길이를 직접 가져올 것
        for(int i = 0; i < 5; i++)
        {
            UnitStatus data = _manager.GetCurrentPresetData(i);

            if (data != null && data.Data != null && data.Data.ID == _status.Data.ID)
            {
                _selectedImage.gameObject.SetActive(true);
                Debug.Log("실행됨");
                break;
            }
            
            if(i == 4) _selectedImage.gameObject.SetActive(false);
        }        
    }

    #endregion

    #region Data Input

    // 데이터 입력 관련 메소드

    #endregion
}