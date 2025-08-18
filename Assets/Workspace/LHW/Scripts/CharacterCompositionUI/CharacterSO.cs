using System;
using UnityEngine;

[CreateAssetMenu(fileName ="CharacterData", menuName = "Data/CharacterData")]
public class CharacterSO : ScriptableObject
{
    [field:SerializeField] public TestSynergy CharacterSynergy {  get; private set; }
    [field:SerializeField] public int Cost { get; private set; }
    [field:SerializeField] public int OverallPower { get; private set; }
}

[Serializable]
public struct TestSynergy
{
    public string JobSynergy;
    public string RoleSynergy;
}