using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

    private bool _isCollected = true;
    public bool IsCollected => _isCollected;

    private TeamOrganizeManager _manager;
    
    private void Awake()
    {
        _manager = GetComponentInParent<TeamOrganizeManager>();
        GetComponent<Button>().onClick.AddListener(TryAddCharacter);
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

        UIUpdate();
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
        _overallPowerText.text = $"{_charData.OverallPower}";
    }

    public void InputData(CharacterSO data)
    {
        _charData = data;
    }    
}