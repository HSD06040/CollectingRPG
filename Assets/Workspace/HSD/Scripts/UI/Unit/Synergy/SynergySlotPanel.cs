using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class SynergySlotPanel : MonoBehaviour
{
    [SerializeField] GameObject _synergyIconPrefab;
    [SerializeField] Transform _content;
    [SerializeField] SynergyToolTip _synergyTooltip;
    private Dictionary<int, int> _synergyIconSlotDic = new(10);
    private SynergyIconSlot[] _synergyIconSlots;

    public void Init(SynergyDatabase db)
    {
        CreateSynergtSlots(db);
    }

    private void CreateSynergtSlots(SynergyDatabase db)
    {
        SynergyData[] datas = db._synergyDataDic.Values.ToArray();
        _synergyIconSlots = new SynergyIconSlot[datas.Length];

        for (int i = 0; i < datas.Length; i++)
        {
            SynergyIconSlot slot = Instantiate(_synergyIconPrefab, _content).GetComponent<SynergyIconSlot>();
            slot.Init(datas[i], _synergyTooltip);
            
            _synergyIconSlots[i] = slot;

            if (datas[i] is ClassSynergyData classSynergy)
                _synergyIconSlotDic.Add((int)classSynergy.Synergy, i);
            else if (datas[i] is UnitSynergyData unitSynergy)
                _synergyIconSlotDic.Add((int)unitSynergy.Synergy, i);
        }        
    }

    public void UpdateSynergySlot(int synergy, int activeCount)
    {
        _synergyIconSlots[_synergyIconSlotDic[synergy]].UpdateIcon(activeCount);
        SetHiararchy();
    }


    private void SetHiararchy()
    {
        List<SynergyIconSlot> slots = _synergyIconSlots.ToList();

        slots.RemoveAll(s => s.ActiveCount <= 0);

        slots.Sort((a, b) =>
        {
            int result = b.ActiveCount.CompareTo(a.ActiveCount);
            if(result == 0)
                result = b.UpgradeCount.CompareTo(a.UpgradeCount);

            return result;
        });

        for (int i = 0; i < slots.Count; i++)
        {
            slots[i].transform.SetSiblingIndex(i);
        }
    }
}
