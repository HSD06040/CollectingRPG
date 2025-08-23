using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SynergyIconSlot : MonoBehaviour
{
    [SerializeField] Image _icon;
    [SerializeField] Image _upgradeColorImage;
    private SynergyData _synergyData;
    public int ActiveCount;
    // ToolTip도 보여야 함

    public void Init(SynergyData data)
    {
        _synergyData = data;
        _icon.sprite = data.Icon;
        ActiveCount = data.CurrentUpgradeIdx;
    }

    public void UpdateIcon()
    {
        ActiveCount = _synergyData.CurrentUpgradeIdx;
        _upgradeColorImage.color = _synergyData.GetSynergyColor();        
    }
}
