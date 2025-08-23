using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class SynergySlotPanel : MonoBehaviour
{
    [SerializeField] GameObject _synergyIconPrefab;
    [SerializeField] Transform _content;
    private Dictionary<string, int> _synergyIconSlotDic = new(50);
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
            slot.Init(datas[i]);
            
            _synergyIconSlots[i] = slot;

            if (datas[i] is ClassSynergyData classSynergy)
                _synergyIconSlotDic.Add(classSynergy.Synergy.ToString(), i);
            else if (datas[i] is UnitSynergyData unitSynergy)
                _synergyIconSlotDic.Add(unitSynergy.Synergy.ToString(), i);
        }        
    }

    public void UpdateSynergySlot(string synergy, int activeCount)
    {
        _synergyIconSlots[_synergyIconSlotDic[synergy]].UpdateIcon();
        SetHiararchy();
    }


    private void SetHiararchy()
    {
        List<SynergyIconSlot> slots = _synergyIconSlots.ToList();

        slots.RemoveAll(s => s.ActiveCount <= 0);

        slots.Sort((a, b) => b.ActiveCount.CompareTo(a.ActiveCount));

        for (int i = 0; i < slots.Count; i++)
        {
            slots[i].transform.SetSiblingIndex(i);
        }
    }
}
