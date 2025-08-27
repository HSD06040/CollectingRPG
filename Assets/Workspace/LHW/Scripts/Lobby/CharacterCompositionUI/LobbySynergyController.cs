using System.Collections.Generic;
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
        //InitializeSynergySlots();
        //InitializeClassSynergySlots();
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

    // TODO: 아직 구성중이라 임시 처리 
    /*
    private void InitializeSynergySlots()
    {
        foreach (Synergy synergy in SynergyController.SynergyDB._synergyDataDic)
        {
            if (_synergySlots.ContainsKey(synergy.ToString()))
                continue;

            LobbySynergySlot slot = Instantiate(_synergySlotPrefab, _content).GetComponent<LobbySynergySlot>();
            slot.SetSynergy(synergy);
            slot.SetActiveCount(0); // 초기값 0
            _synergySlots[synergy.ToString()
                ] = slot;
        }
    }

    private void InitializeClassSynergySlots()
    {
        foreach (var classType in System.Enum.GetValues(typeof(ClassType)))
        {
            if (_classSynergySlots.ContainsKey(classType.ToString()))
                continue;

            LobbyClassSynergySlot slot = Instantiate(_classSynergySlotPrefab, _classContent).GetComponent<LobbyClassSynergySlot>();
            slot.SetClassSynergy((ClassType)classType);
            slot.SetActiveCount(0);
            _classSynergySlots[classType.ToString()] = slot;
        }
    }
    */

    private void UpdateSynergySlots()
    {
        Dictionary<Synergy, int> synergyCount = _manager.GetSynergyCount();

        foreach(var data in synergyCount)
        {
            Synergy synergy = data.Key;
            int count = data.Value;

            if(!_synergySlots.ContainsKey(synergy.ToString()))
            {
                LobbySynergySlot slot = Instantiate(_synergySlotPrefab, _content).GetComponent<LobbySynergySlot>();
                _synergySlots[synergy.ToString()] = slot;
                slot.SetSynergy(synergy);
            }

            _synergySlots[synergy.ToString()].SetActiveCount(count);
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

            if (!_classSynergySlots.ContainsKey(classSynergy.ToString()))
            {
                LobbyClassSynergySlot slot = Instantiate(_classSynergySlotPrefab, _classContent).GetComponent<LobbyClassSynergySlot>();
                _classSynergySlots[classSynergy.ToString()] = slot;
                slot.SetSynergy(classSynergy);
            }

            _classSynergySlots[classSynergy.ToString()].SetActiveCount(count);
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