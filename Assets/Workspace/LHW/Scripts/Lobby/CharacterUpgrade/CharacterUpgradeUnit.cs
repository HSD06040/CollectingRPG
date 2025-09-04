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
    [SerializeField] private GameObject _backgroundPanel;
    //[SerializeField] private TMP_Text _charText;
    [SerializeField] private Image _characterImg;
    [SerializeField] private Image _costImg;
    [SerializeField] private Image _jobSynergyImg;
    [SerializeField] private Image _roleSynergyImg;
    //[SerializeField] private TMP_Text _overallPowerText;
    [SerializeField] private TMP_Text _levelText;
    [SerializeField] private Image _outlineImage;

    [Header("GaugeUI")]
    [SerializeField] private Image _pieceGauge;
    [SerializeField] private TMP_Text _pieceNum;

    [Header("Reference")]
    [SerializeField] private Sprite[] costSprites;
    [SerializeField] private TempUpgradeUnitData _upgradeData;
    public TempUpgradeUnitData UpgradeData => _upgradeData;

    private UpgradeManager _manager;

    private bool _isCollected = true;
    public bool IsCollected => _isCollected;

    [SerializeField] private float _requiredPointerDownTime = 2f;
    private Coroutine _holdCoroutine;

    private void Awake()
    {
        DataInit();
        GetComponent<Button>().onClick.AddListener(ShowPopUp);
        _manager = GetComponentInParent<UpgradeManager>();
    }

    private void Start()
    {
        UIUpdate();
    }

    private void DataInit()
    {
        _status = new UnitStatus(_unitData, 1);
    }

    private void OnEnable()
    {
        _manager.PopUpUI.OnCharacterStatusChanged += UIUpdate;
    }

    private void OnDisable()
    {
        _manager.PopUpUI.OnCharacterStatusChanged -= UIUpdate;
    }

    #region Onclick

    private void ShowPopUp()
    {
        if (_upgradeData.IsCollected)
        {
            _manager.PopUpUI.GetCurrentCharacterUnitData(this);
            _manager.ShowPopUp();
        }
        else
        {
            if (PopupManager.Instance != null)
            {
                PopupManager.instance.ShowPopup("획득하지 않은 캐릭터입니다.");
            }
        }
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
        if (_upgradeData.IsCollected)
        {
            _backgroundPanel.SetActive(false);
        }
        else
        {
            _backgroundPanel.SetActive(true);
        }

        //_charText.text = $"{_status.Data.Name}";
        _characterImg.sprite = _status.Data.Icon;
        _costImg.sprite = costSprites[_status.Data.Cost - 1];
        if (Manager.Data != null)
        {
            _jobSynergyImg.sprite = Manager.Data.SynergyDB.GetSynergy((int)_status.Data.Synergy).Icon;
            _roleSynergyImg.sprite = Manager.Data.SynergyDB.GetSynergy((int)_status.Data.Synergy).Icon;
        }
        //_overallPowerText.text = $"{_status.CombatPower}";
        _levelText.text = $"Lv.{_status.Level}";

        PieceGaugeUpdate();
        OutlineUpdate();
    }

    private void PieceGaugeUpdate()
    {
        int requirePiece = _upgradeData.GetRequiredPiece();
        if (_upgradeData.CurrentPieces == 0)
        {
            _pieceGauge.fillAmount = 0;
        }
        else
        {
            _pieceGauge.fillAmount = (float)(_upgradeData.CurrentPieces / requirePiece);
        }
        _pieceNum.text = $"{_upgradeData.CurrentPieces}/{requirePiece}";
    }

    private void OutlineUpdate()
    {
        if (_status.Level >= 3)
        {
            _outlineImage.gameObject.SetActive(true);
            if (_status.Level >= 9)
            {
                _outlineImage.color = Color.red;


            }
            else if (_status.Level >= 6)
            {
                _outlineImage.color = Color.yellow;
            }
            else
            {
                _outlineImage.color = Color.blue;

            }
        }
        else
        {
            _outlineImage.gameObject.SetActive(false);
        }
    }

    #endregion

    public void LevelUp()
    {
        if (_status.Level >= 12) return;
        _status.Level++;
    }

    #region Data Input

    // 데이터 입력 관련 메소드

    #endregion
}
