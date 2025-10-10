using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolTipController : MonoBehaviour
{
    public UnitToolTip UnitToolTip;
    public SynergyToolTip SynergyToolTip;

    public void CloseAll()
    {
        UnitToolTip.Close();
        SynergyToolTip.Close();
    }
}
