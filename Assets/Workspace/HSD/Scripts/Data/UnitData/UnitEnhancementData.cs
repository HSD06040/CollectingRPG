using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UnitEnhancementData", menuName = "Data/Unit/UnitEnhancementData")]
public class UnitEnhancementData : ScriptableObject
{
    [Header("Upgrade")]
    public int UpgradeCount;
    public UnitUpgradeData UpgradeData;

    [Header("Synergy")]
    public ClassSynergy ClassSynergy;
    public Synergy Synergy;     
}
