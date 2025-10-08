using Firebase.Database;
using System;
using System.Threading.Tasks;
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
    [SerializeField] private Image _cardImage;
    [SerializeField] private TMP_Text _charText;
    [SerializeField] private Image _characterImg;
    [SerializeField] private Image _roleSynergyImg;
    [SerializeField] private TMP_Text _levelText;
    [SerializeField] private Image _pieceGauge;

    [Header("Reference")]
    [SerializeField] private Sprite[] _gradeSprite;

    private UpgradeManager _manager;

    private bool _isCollected = true;
    public bool IsCollected => _isCollected;

    [SerializeField] private float _requiredPointerDownTime = 2f;
    private Coroutine _holdCoroutine;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(ShowPopUp);
        _manager = GetComponentInParent<UpgradeManager>();
    }

    private void Start()
    {
        UIUpdate();
    }

    private void OnEnable()
    {
        if (_manager != null)
        {
            _manager.CharPopUpUI.OnCharacterStatusChanged += UIUpdate;
            UIUpdate();
        }
    }

    private void OnDisable()
    {
        if (_manager != null) _manager.CharPopUpUI.OnCharacterStatusChanged -= UIUpdate;
    }

    #region Onclick

    private void ShowPopUp()
    {
        _manager.CharPopUpUI.GetCurrentCharacterUnitData(this);
        _manager.ShowCharacterPopUp();
    }

    #endregion

    #region UI Update

    private async void UIUpdate()
    {
        if(_status == null) return;

        _cardImage.sprite = _gradeSprite[(int)_status.Data.Grade];
        _charText.text = $"{_status.Data.Name}";
        _characterImg.sprite = _status.Data.Icon;
        if (Manager.Data.SynergyDB != null)
        {
            _roleSynergyImg.sprite = Manager.Data.SynergyDB.GetSynergy((int)_status.Data.ClassSynergy).ActiveIcon;
        }
        _levelText.text = $"Lv.{_status.Data.UpgradeData.CurrentUpgradeData.UpgradeLevel}";

        await GaugeUpdate();
    }

    private async Task GaugeUpdate()
    {
        int requirePiece = Status.Data.UpgradeData.GetRequiredPiece();
        if (_status.Data.UpgradeData.CurrentUpgradeData.CurrentPieces == 0)
        {
            _pieceGauge.fillAmount = 0;
        }
        else if(_status.Data.UpgradeData.CurrentUpgradeData.UpgradeLevel == 0)
        {
            _pieceGauge.fillAmount = (float)_status.Data.UpgradeData.CurrentUpgradeData.CurrentPieces / 10;
        }
        else
        {
            string uid = FirebaseManager.Auth.CurrentUser.UserId;
            var pieceRef = FirebaseManager.DataReference.Child("UserData").Child(uid).Child("MythStone");

            DataSnapshot snapshot = await pieceRef.GetValueAsync();

            int current = 0;

            if (snapshot.Exists && snapshot.Value != null)
            {
                current = Convert.ToInt32(snapshot.Value);
            }

            int currentPiece = _status.Data.UpgradeData.CurrentUpgradeData.CurrentPieces + current;
            _pieceGauge.fillAmount = (float)currentPiece / requirePiece;
        }
    }

    #endregion

    #region Data Input

    // 데이터 입력 관련 메소드
    public void InitUnitStatus(UnitData data)
    {
        _status = new UnitStatus(data, data.UpgradeData.CurrentUpgradeData.UpgradeLevel);
    }

    #endregion
}