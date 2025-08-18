using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectedCharacterUnit : MonoBehaviour
{
    [Header("Index")]
    [SerializeField] private int _index;

    [Header("UI")]
    [SerializeField] Image _charImage;

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
            _charImage.color = Color.clear;
            _charImage.sprite = null;
        }
        else
        {
            _charImage.color = Color.white;
            _charImage.sprite = _charData.CharacterImage;
        }
    }
}
