using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LobbySynergyController : MonoBehaviour
{
    [SerializeField] GameObject _synergySlotPrefab;
    [SerializeField] GameObject _classSynergySlotPrefab;
    [SerializeField] Transform _content;
    [SerializeField] Transform _classContent;

    private Dictionary<string, LobbySynergySlot> _synergySlots = new(50);
    private Dictionary<string, LobbyClassSynergySlot> _classSynergySlots = new(50);

    private TeamOrganizeManager _manager;

    private void Start()
    {
        _manager = GetComponentInParent<TeamOrganizeManager>();
        InitializeSynergySlots();
    }

    private void OnEnable()
    {
        _manager.OnCharacterDataChanged += UpdateSynergySlots;
        _manager.OnCharacterDataChanged += UpdateClassSynergySlots;
    }

    private void OnDisable()
    {
        _manager.OnCharacterDataChanged -= UpdateSynergySlots;
        _manager.OnCharacterDataChanged -= UpdateClassSynergySlots;
    }

    private void InitializeSynergySlots()
    {
        foreach (var data in SynergyController.SynergyDB._synergyDataDic.Values)
        {
            if (data is ClassSynergyData classSynergy)
            {
                LobbyClassSynergySlot slot = Instantiate(_classSynergySlotPrefab, _classContent).GetComponent<LobbyClassSynergySlot>();
                slot.Init(data, 0);
                _classSynergySlots.Add(classSynergy.Synergy.ToString(), slot);
            }
            else if (data is UnitSynergyData unitSynergy)
            {
                LobbySynergySlot slot = Instantiate(_synergySlotPrefab, _content).GetComponent<LobbySynergySlot>();
                slot.Init(data, 0);
                _synergySlots.Add(unitSynergy.Synergy.ToString(), slot);
            }
        }
    }    

    private void UpdateSynergySlots()
    {
        Dictionary<Synergy, int> synergyCount = _manager.GetSynergyCount();

        foreach(var data in synergyCount)
        {
            Synergy synergy = data.Key;
            int count = data.Value;

            _synergySlots[synergy.ToString()].UpdateUI(count);
        }

        SetHiararchy();
    }

    private void UpdateClassSynergySlots()
    {
        Dictionary<ClassType, int> classSynergyCount = _manager.GetClassSynergyCount();

        foreach(var data in classSynergyCount)
        {
            ClassType classSynergy = data.Key;
            int count = data.Value;

            _classSynergySlots[classSynergy.ToString()].UpdateUI(count);
        }

        SetClassHiararchy();
    }    

    private void SetHiararchy()
    {
        List<LobbySynergySlot> slots = new List<LobbySynergySlot>(_synergySlots.Values);

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
    }

    private void SetClassHiararchy()
    {
        List<LobbyClassSynergySlot> slots = new List<LobbyClassSynergySlot>(_classSynergySlots.Values);

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
    }
}