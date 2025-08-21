using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 획득한 캐릭터의 개수 및 목록을 반환하기 위한 데이터 저장
/// </summary>
public class CollectedCharacterData : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private CharacterUnit[] _characters;
    [SerializeField] private List<CharacterSO> _collectedCharData = new List<CharacterSO>();
    public List<CharacterSO> CollectedCharData => _collectedCharData;

    private int _characterCount;
    public int CharacterCount => _characterCount;

    private int _collectedCharacterCount;
    public int CollectedCharacterCount => _collectedCharacterCount;

    private void Awake()
    {
        for(int i = 0; i < _characters.Length; i++)
        {
            if(_characters[i].IsCollected)
            {
                _collectedCharData.Add(_characters[i].CharData);
            }
        }
        _characterCount = _characters.Length;
        _collectedCharacterCount = _collectedCharData.Count;
    }
}