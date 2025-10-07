using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ClassSynergyData", menuName = "Data/Synergy/Class")]
public class ClassSynergyData : SynergyData
{
   public ClassType Synergy;

    public override void Init()
    {
        base.Init();
        _synergy = (int)Synergy;

        string synergy = Synergy.ToString();
        synergy = $"{char.ToUpper(synergy[0])}{synergy.Substring(1).ToLower()}";

        deActiveIconAddress = $"{synergy}_DeActive";
        activeIconAddress = $"{synergy}_Active";
    }
}
