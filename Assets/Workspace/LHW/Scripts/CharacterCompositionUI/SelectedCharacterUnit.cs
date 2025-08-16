using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectedCharacterUnit : MonoBehaviour
{
    [Header("Index")]
    [SerializeField] private int _index;

    [Header("UI")]
    [SerializeField] private TMP_Text _charText;
    [SerializeField] private TMP_Text _costText;
    [SerializeField] private TMP_Text _jobSynergyText;
    [SerializeField] private TMP_Text _roleSynergyText;
    [SerializeField] private TMP_Text _overallPowerText;

    private CharacterSO _charData;
    private TeamOrganizeManager _manager;

    private void Awake()
    {
        _manager = GetComponentInParent<TeamOrganizeManager>();
        GetComponent<Button>().onClick.AddListener(TryDeleteCharacter);
    }

    private void OnEnable()
    {
        _manager.OnCharacterDataChanged += UIUpdate;        
    }

    private void OnDisable()
    {
        _manager.OnCharacterDataChanged -= UIUpdate;
    }

    private void Start()
    {
        UIUpdate();
    }

    private void TryDeleteCharacter()
    {
        _manager.RemoveCharacterData(_index);
        _charData = null;
    }

    private void UIUpdate()
    {
        _charData = _manager.GetCurrentCharacterData(_index);

        if(_manager == null || _charData == null)
        {
            _charText.text = "";
            _costText.text = "";
            _jobSynergyText.text = "";
            _roleSynergyText.text = "";
            _overallPowerText.text = "";
        }
        else
        {
            _charText.text = $"{_charData.name}";
            _costText.text = $"{_charData.Cost}";
            _jobSynergyText.text = $"{_charData.CharacterSynergy.JobSynergy}";
            _roleSynergyText.text = $"{_charData.CharacterSynergy.RoleSynergy}";
            _overallPowerText.text = $"{_charData.OverallPower}";
        }
    }
}
