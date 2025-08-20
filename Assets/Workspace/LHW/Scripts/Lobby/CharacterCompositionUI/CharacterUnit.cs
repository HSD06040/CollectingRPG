using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 편성할 수 있는 캐릭터를 표시하는 유닛
/// </summary>
public class CharacterUnit : MonoBehaviour
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
    
    private void Awake()
    {
        _manager = GetComponentInParent<TeamOrganizeManager>();
        GetComponent<Button>().onClick.AddListener(TryAddCharacter);
    }

    private void Start()
    {
        UIUpdate();
    }

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

    private void TryAddCharacter()
    {
        _manager.AddCharacterData(_charData);
    }

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

    public void InputData(CharacterSO data)
    {
        _charData = data;
    }    
}