using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpgradeCollectedCharacterData : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private CharacterUpgradeUnit[] _charUnits;

    [Header("UI")]
    [SerializeField] private TMP_Text _currentObtainedChacterText;

    [SerializeField] private List<UnitStatus> _collectedUnit = new List<UnitStatus>();
    public List<UnitStatus> CollectedUnit => _collectedUnit;

    private int _characterCount;
    public int CharacterCount => _characterCount;

    private int _collectedCharacterCount;
    public int CollectedCharacterCount => _collectedCharacterCount;

    private void Start()
    {
        for (int i = 0; i < _charUnits.Length; i++)
        {
            // 획득여부 체크는 나중에 추가
            //if(_charUnits[i].IsCollected)
            //{
            //_collectedCharData.Add(_charUnits[i].CharData);

            _collectedUnit.Add(_charUnits[i].Status);
            //}
        }
        _characterCount = _charUnits.Length;
        _collectedCharacterCount = _collectedUnit.Count;
    }

    private void OnEnable()
    {
        _currentObtainedChacterText.text = $"보유 영웅 {_collectedCharacterCount}/{_characterCount}";
    }
}
