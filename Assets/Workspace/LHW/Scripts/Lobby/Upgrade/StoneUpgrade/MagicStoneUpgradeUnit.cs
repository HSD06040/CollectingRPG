using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MagicStoneUpgradeUnit : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Image _stoneIcon;
    [SerializeField] private TMP_Text _levelText;
    [SerializeField] private TMP_Text _nameText;

    private UpgradeManager _manager;
    private MagicStoneData _data;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(ShowPopup);
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
            _manager.StonePopUpUI.OnMagicStoneStatusChanged += UIUpdate;
            UIUpdate();
        }
    }

    private void OnDisable()
    {
        if (_manager != null) _manager.StonePopUpUI.OnMagicStoneStatusChanged -= UIUpdate;
    }

    private void ShowPopup()
    {
        _manager.ShowStonePopUp();
    }

    public void InitMagicStoneStatus(MagicStoneData data)
    {
        _data = data;
    }

    private void UIUpdate()
    {
        if(_data == null) return;

        _stoneIcon.sprite = _data.Icon;
        _levelText.text = $"LV.{_data.UpgradeData.CurrentUpgradeData.UpgradeLevel}";
        _nameText.text = _data.Name;
    }
}