using TMPro.EditorUtilities;
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
    [SerializeField] private GameObject _crownImage;

    private CharacterSO _charData;
    private TeamOrganizeManager _manager;

    [SerializeField] private UnitStatus _status;

    private GameObject _obj;

    private void Awake()
    {
        _manager = GetComponentInParent<TeamOrganizeManager>();
        GetComponent<Button>().onClick.AddListener(TryDeleteCharacter);
        _status = new UnitStatus(null, 0);
    }

    private void Start()
    {
        UIUpdate();
    }

    private void OnEnable()
    {
        _manager.OnCharacterDataChanged += UIUpdate;
    }

    private void OnDisable()
    {
        _manager.OnCharacterDataChanged -= UIUpdate;
    }

    private void TryDeleteCharacter()
    {
        _manager.RemoveUnitData(_index);
        _status = _manager.GetCurrentPresetData(_index);
        Debug.Log("편성해제함");
    }

    private void UIUpdate()
    {
        if (_manager != null)
        {
            _status = _manager.GetCurrentPresetData(_index);
        }

        if (_manager == null || _status == null || _status.Data == null)
        {
            if (_obj != null)
            {
                Manager.Pool.Release(_obj);
                _obj = null;
            }

            if(_index == 0)
            {
                _crownImage.SetActive(false);
            }

            if (_manager.CurrentCost == _manager.TotalCost)
            {
                //_charImage.color = Color.black;
                //_charImage.sprite = _xImage;
            }
            else
            {
                //_charImage.color = Color.clear;
                //_charImage.sprite = null;
            }
        }
        else
        {
            if (_status.Data != null)
            {
                if(_index == 0)
                {
                    _crownImage.SetActive(true);
                }

                if (_obj == null)
                {
                    _obj = Manager.Pool.Get(_status.Data.UnitPrefab, new Vector3(-0.8f + (_index * 1.4f), 0, 0), Quaternion.identity);
                }
            }
        }
    }
}