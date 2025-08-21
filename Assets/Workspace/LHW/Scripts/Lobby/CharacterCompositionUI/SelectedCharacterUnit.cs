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

        if (_manager == null || _charData == null)
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
            if (_manager.CurrentCost == _manager.TotalCost && _charData == null)
            {
                _charImage.color = Color.black;
                _charImage.sprite = _xImage;
            }

            _charImage.color = Color.white;
            _charImage.sprite = _charData.CharacterImage;
        }
    }
}