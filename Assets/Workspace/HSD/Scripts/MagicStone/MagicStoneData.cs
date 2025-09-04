using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MagicStoneData", menuName = "Data/MagicStoneData")]
public class MagicStoneData : ScriptableObject
{
    public Grade Grade;
    public Sprite Icon;
    public string Name;
    [TextArea]
    public string Description;
}
