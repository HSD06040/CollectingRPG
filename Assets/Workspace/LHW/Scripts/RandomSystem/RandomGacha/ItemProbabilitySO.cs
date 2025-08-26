using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Probability", menuName ="Data/Probability")]
public class ItemProbabilitySO : ScriptableObject
{
    [field:SerializeField] public List<ProbableItems> itemsProbability { get; private set; }
}

[Serializable]
public class ProbableItems
{
    public UnitData CharData;
    public float Probability;
}