using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.UI;

public class SynergyPanel : MonoBehaviour
{
    private GameObject _synergySlotPrefab;

    [SerializeField] string _synergySlotAddress;
    [SerializeField] Transform _content;
    [SerializeField] SynergyToolTip _synergyTooltip;
    [SerializeField] GridLayoutGroup _gridLayoutGroup;
    [SerializeField] int _offset;
    [SerializeField] GameObject _nextButton;
    [SerializeField] GameObject _prevButton;

    private Dictionary<int, SynergySlot_New> _synergySlots = new(10);
    private int _currentPage;
    private int _maxPage;

    private void Start()
    {        
        _gridLayoutGroup.SetupGridLayoutGroup(_content, 6, 1, _gridLayoutGroup.cellSize, _offset, true);
    }

    public void Init(SynergyDatabase db)
    {
        _synergySlotPrefab = Manager.Resources.Load<GameObject>(_synergySlotAddress);

        CreateSynergtSlots(db);
        SetActivate();
        _maxPage = _content.childCount / 6;

        SetPageButton();
    }

    public void PageChange(int num)
    {
        num = Mathf.Clamp(num, -1, 1);

        if (_currentPage + num > _maxPage || _currentPage + num < 0)
            return;

        _currentPage += num;

        SetActivate();
        SetPageButton();
    }

    private void CreateSynergtSlots(SynergyDatabase db)
    {
        foreach (var data in db._synergyDataDic.Values)
        {
            SynergySlot_New slot = Instantiate(_synergySlotPrefab, _content).GetComponent<SynergySlot_New>();
            slot.Init(data, 0, _synergyTooltip);

            if(data is ClassSynergyData classSynergy)
                _synergySlots.Add((int)classSynergy.Synergy, slot);
            else if (data is UnitSynergyData unitSynergy)
                _synergySlots.Add((int)unitSynergy.Synergy, slot);            
        }
    }

    public void UpdateSynergySlot(int synergy, int activeCount)
    {
        _synergySlots[synergy].UpdateUI(activeCount);
        //SetHiararchy();
    }
    

    private void SetHiararchy()
    {
        List<SynergySlot_New> slots = new List<SynergySlot_New>(_synergySlots.Values);

        slots.RemoveAll(s => s.ActiveCount <= 0);

        slots.Sort((a, b) =>
        {
            int result = b.ActiveCount.CompareTo(a.ActiveCount);
            if (result == 0)
                result = b.UpgradeCount.CompareTo(a.UpgradeCount);

            return result;
        });

        for (int i = 0; i < slots.Count; i++)
        {
            slots[i].transform.SetSiblingIndex(i);
        }

        SetActivate();
    }

    private void SetActivate()
    {
        int start = 0 + 6 * _currentPage;
        int end = start + 6;

        for (int i = 0; i < _content.childCount; i++)
        {
            if(i >= start && i < end)
                _content.GetChild(i).gameObject.SetActive(true);
            else
                _content.GetChild(i).gameObject.SetActive(false);
        }
    }

    private void SetPageButton()
    {
        if (_currentPage <= 0)
            _prevButton.SetActive(false);
        else
            _prevButton.SetActive(true);

        if (_currentPage >= _maxPage)
            _nextButton.SetActive(false);
        else
            _nextButton.SetActive(true);
    }
}
