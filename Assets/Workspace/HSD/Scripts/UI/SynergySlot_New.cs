using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SynergySlot_New : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] Image _icon;
    [SerializeField] Image _synergyColorSlot;
    [SerializeField] SynergyActiveSlot_Controller _synergyActiveSlot_Controller;

    private SynergyData _synergyData;
    private SynergyToolTip _synergyToolTip;    
    private int[] _synergyCountArray;

    public int ActiveCount;
    public int UpgradeCount => _synergyData.CurrentUpgradeIdx;

    public void Init(SynergyData data, int activeCount, SynergyToolTip synergyToolTip)
    {
        _synergyData = data;
        _icon.sprite = data.Icon;
        _synergyToolTip = synergyToolTip;
        SetSynergyCount();
        UpdateUI(activeCount);
    }

    public void UpdateUI(int activeCount)
    {
        ActiveCount = activeCount;
        _synergyColorSlot.color = _synergyData.SynergyColor;
        _synergyActiveSlot_Controller.Active(_synergyData.CurrentUpgradeIdx + 1);
    }

    private void SetSynergyCount()
    {
        _synergyCountArray = new int[_synergyData.SynergyLevelData.Length];

        for (int i = 0; i < _synergyData.SynergyLevelData.Length; i++)
        {
            _synergyCountArray[i] = _synergyData.SynergyLevelData[i].SynergyNeedCount;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (_synergyData == null) return;

        _synergyToolTip.Show(eventData, _synergyData);
    }
}
