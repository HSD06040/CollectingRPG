using System.Collections.Generic;
using UnityEngine;

public class CollectedCharacterData : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private CharacterUnit[] _characters;
    [SerializeField] private List<CharacterSO> _collectedCharData = new List<CharacterSO>();
    public List<CharacterSO> CollectedCharData => _collectedCharData;

    private void OnEnable()
    {
        for(int i = 0; i < _characters.Length; i++)
        {
            if(_characters[i].IsCollected)
            {
                _collectedCharData.Add(_characters[i].CharData);
            }
        }
    }
}