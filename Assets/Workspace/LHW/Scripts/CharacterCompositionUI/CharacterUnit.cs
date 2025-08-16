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
    [SerializeField] private TMP_Text _costText;
    [SerializeField] private TMP_Text _jobSynergyText;
    [SerializeField] private TMP_Text _roleSynergyText;
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
        _costText.text = $"{_charData.Cost}";
        _jobSynergyText.text = $"{_charData.CharacterSynergy.JobSynergy}";
        _roleSynergyText.text = $"{_charData.CharacterSynergy.RoleSynergy}";
        _overallPowerText.text = $"{_charData.OverallPower}";
    }

    public void InputData(CharacterSO data)
    {
        _charData = data;
    }    
}