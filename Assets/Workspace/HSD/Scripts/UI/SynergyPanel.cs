using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class SynergyPanel : MonoBehaviour
{
    [SerializeField] GameObject _synergySlotPrefab;
    [SerializeField] Transform _content;

    private Dictionary<string, SynergySlot> _synergySlots = new(50);

    public void Init(SynergyDatabase db)
    {
        CreateSynergtSlots(db);
    }

    private void CreateSynergtSlots(SynergyDatabase db)
    {
        foreach (var data in db._synergyDataDic.Values)
        {
            SynergySlot slot = Instantiate(_synergySlotPrefab, _content).GetComponent<SynergySlot>();
            slot.Init(data, 0);

            if(data is ClassSynergyData classSynergy)
                _synergySlots.Add(classSynergy.Synergy.ToString(), slot);
            else if (data is UnitSynergyData unitSynergy)
                _synergySlots.Add(unitSynergy.Synergy.ToString(), slot);            
        }
    }

    public void UpdateSynergySlot(string synergy, int activeCount)
    {
        _synergySlots[synergy].UpdateUI(activeCount);
    }
}
