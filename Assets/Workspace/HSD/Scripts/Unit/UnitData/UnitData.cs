using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UnitData", menuName = "Data/Unit/UnitData")]
public class UnitData : BaseUnitData
{
    [Header("Upgrade")]
    public int UpgradeCount;
    public UnitUpgradeData UpgradeData;

    [Header("Synergy")]
    public ClassSynergy ClassSynergy;
    public Synergy Synergy;     
}
