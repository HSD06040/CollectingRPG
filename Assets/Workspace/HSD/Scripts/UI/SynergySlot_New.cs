using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SynergySlot_New : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] Image _icon;
    [SerializeField] SynergyActiveSlot_Controller _synergyActiveSlot_Controller;

    private SynergyData _synergyData;
    private SynergyToolTip _synergyToolTip;    
    private int[] _synergyCountArray;

    public int ActiveCount;
    public int UpgradeCount => _synergyData.CurrentUpgradeIdx;

    public void Init(SynergyData data, int activeCount, SynergyToolTip synergyToolTip)
    {
        _synergyData = data;
        _icon.sprite = data.ActiveIcon;
        _synergyToolTip = synergyToolTip;
        SetSynergyCount();
        UpdateUI(activeCount);
        _synergyActiveSlot_Controller.Init(_synergyData.SynergyColor);
    }

    public void UpdateUI(int activeCount)
    {
        ActiveCount = activeCount;

        if(activeCount <= 1)
        {
            _icon.sprite = _synergyData.DeActiveIcon;
        }
        else
        {
            _icon.sprite = _synergyData.ActiveIcon;
        }

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

    public void OnDrag(PointerEventData eventData)
    {
        if (_synergyData == null) return;

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _synergyToolTip.Close();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (_synergyData == null) return;

        _synergyToolTip.Show(eventData, _synergyData); 
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (_synergyData == null) return;

        _synergyToolTip.Show(eventData, _synergyData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _synergyToolTip.Close();
    }
}
