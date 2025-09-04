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
    public GameObject Prefab => Manager.Resources.Get<GameObject>(Address);
    public string Address;

    public bool IsBuff;
    public SynergyBuffData[] SynergyBuffDatas;

    public bool IsStun;
    public float StunDuration;

    public bool IsTick;
    public float TickInterval;
    public int TickCount;

    public TargetType TargetType;
    public MagicStoneSearchType MagicStoneSearchType;
    public int AttackCount;
}
