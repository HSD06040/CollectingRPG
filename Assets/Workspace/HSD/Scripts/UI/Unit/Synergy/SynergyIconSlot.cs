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
    public int UpgradeCount => _synergyData.CurrentUpgradeIdx;
    // ToolTip도 보여야 함

    public void Init(SynergyData data)
    {
        _synergyData = data;
        _icon.sprite = data.Icon;
        UpdateIcon(0);
    }

    public void UpdateIcon(int activeCount)
    {
        ActiveCount = activeCount;
        _upgradeColorImage.color = _synergyData.GetSynergyColor();        
    }
}
