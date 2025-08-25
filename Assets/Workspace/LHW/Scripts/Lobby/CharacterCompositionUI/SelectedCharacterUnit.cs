using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 편성된 캐릭터를 확인하기 위한 유닛
/// </summary>
public class SelectedCharacterUnit : MonoBehaviour
{
    [Header("Referenece")]
    [SerializeField] private Sprite _xImage;

    [Header("Index")]
    [SerializeField] private int _index;

    [Header("UI")]
    [SerializeField] private Image _charImage;

    private CharacterSO _charData;
    private TeamOrganizeManager _manager;

    [SerializeField] private UnitStatus _status;

    private void Awake()
    {
        _manager = GetComponentInParent<TeamOrganizeManager>();
        GetComponent<Button>().onClick.AddListener(TryDeleteCharacter);
        _status = new UnitStatus(null, 0);
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
        _manager.RemoveUnitData(_index);
        _status = _manager.GetCurrentPresetData(_index);
        Debug.Log("편성해제함");
    }

    private void UIUpdate()
    {
        _status = _manager.GetCurrentPresetData(_index);

        if (_manager == null || _status == null || _status.Data == null)
        {
            if (_manager.CurrentCost == _manager.TotalCost)
            {
                _charImage.color = Color.black;
                _charImage.sprite = _xImage;
            }
            else
            {
                _charImage.color = Color.clear;
                _charImage.sprite = null;
            }
        }
        else
        {
            if (_status.Data != null)
            {
                _charImage.color = Color.white;
                _charImage.sprite = _status.Data.Icon;
            }
        }
    }
}