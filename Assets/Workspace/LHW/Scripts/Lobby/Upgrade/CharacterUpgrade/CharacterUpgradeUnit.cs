using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterUpgradeUnit : MonoBehaviour
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
    [SerializeField] private ImageSO _costSprites;

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
        if (_manager != null)
        {
            _manager.PopUpUI.OnCharacterStatusChanged += UIUpdate;
            UIUpdate();
        }
    }

    private void OnDisable()
    {
        if(_manager != null) _manager.PopUpUI.OnCharacterStatusChanged -= UIUpdate;
    }

    #region Onclick

    private void ShowPopUp()
    {
        if (_status.Data.UpgradeData.CurrentUpgradeData.UpgradeLevel == 0)
        {
            if (PopupManager.Instance != null)
            {
                PopupManager.instance.ShowPopup("획득하지 않은 캐릭터입니다.");
            }
        }
        else
        {
            _manager.PopUpUI.GetCurrentCharacterUnitData(this);
            _manager.ShowCharacterPopUp();
        }
    }

    #endregion

    #region UI Update

    private void UIUpdate()
    {
        if (_status.Data.UpgradeData.CurrentUpgradeData.UpgradeLevel == 0)
        {
            _backgroundPanel.SetActive(true);
        }
        else
        {
            _backgroundPanel.SetActive(false);
        }

        //_charText.text = $"{_status.Data.Name}";
        _characterImg.sprite = _status.Data.Icon;
        _costImg.sprite = _costSprites.CostSprites[_status.Data.Cost - 1];
        if (Manager.Data.SynergyDB != null)
        {
            _jobSynergyImg.sprite = Manager.Data.SynergyDB.GetSynergy((int)_status.Data.Synergy).Icon;
            _roleSynergyImg.sprite = Manager.Data.SynergyDB.GetSynergy((int)_status.Data.ClassSynergy).Icon;
        }
        //_overallPowerText.text = $"{_status.CombatPower}";
        _levelText.text = $"Lv.{_status.Data.UpgradeData.CurrentUpgradeData.UpgradeLevel}";

        PieceGaugeUpdate();
        OutlineUpdate();
    }

    private void PieceGaugeUpdate()
    {
        int requirePiece = _status.Data.UpgradeData.GetRequiredPiece();
        if (_status.Data.UpgradeData.CurrentUpgradeData.CurrentPieces == 0)
        {
            _pieceGauge.fillAmount = 0;
        }
        else
        {
            _pieceGauge.fillAmount = (float)_status.Data.UpgradeData.CurrentUpgradeData.CurrentPieces / requirePiece;
        }
        _pieceNum.text = $"{_status.Data.UpgradeData.CurrentUpgradeData.CurrentPieces}/{requirePiece}";
    }

    private void OutlineUpdate()
    {
        if (_status.Data.UpgradeData.CurrentUpgradeData.UpgradeLevel >= 4)
        {
            _outlineImage.gameObject.SetActive(true);
            if (_status.Data.UpgradeData.CurrentUpgradeData.UpgradeLevel == 10)
            {
                _outlineImage.color = Color.red;
            }
            else if (_status.Data.UpgradeData.CurrentUpgradeData.UpgradeLevel >= 8)
            {
                _outlineImage.color = Color.yellow;
            }
            else if(_status.Data.UpgradeData.CurrentUpgradeData.UpgradeLevel >= 6)
            {
                _outlineImage.color = Color.blue;
            }
            else
            {
                _outlineImage.color = Color.cyan;
            }
        }
        else
        {
            _outlineImage.gameObject.SetActive(false);
        }
    }

    #endregion

    #region Data Input

    // 데이터 입력 관련 메소드

    #endregion
}
