using UnityEngine;

public class UnitManager : MonoBehaviour
{
    [Header("BattleManager")]
    [SerializeField] BattleManager _battleManager;

    [Header("UI")]
    [SerializeField] UnitUIManager _unitUIManager;
    [SerializeField] UnitStanbyUIManager _unitStanbyUIManager;
    [SerializeField] UI_UnitSlotController _unitSlotController;

    [Header("Unit_Controller")]
    public UnitController _unitController;
    public EnemyController _enemyController;

    [Header("Data")]
    [SerializeField] UnitData[] _testDatas;
    [SerializeField] int _upgradeNeedCount = 3;

    private void Awake()
    {
        Utils.Initialize();
        Init();
    }

    private void Init()
    {
        _unitController.SynergyController.Init();
        _unitController.Init();
        _unitStanbyUIManager.Init();

        Subscribe();
        //_unitStanbyUIManager.SynergyPanel.Init(_unitController.SynergyController.SynergyDB);
        //_unitStanbyUIManager.SynergySlotPanel.Init(_unitController.SynergyController.SynergyDB);

        _unitStanbyUIManager.SynergyPanel.Init(SynergyController.SynergyDB);
        _unitStanbyUIManager.SynergySlotPanel.Init(SynergyController.SynergyDB);
               
        TeamPresetData preset = TempDataManager.Instance.ReadCurrentSelectedPreset();

        if (preset == null)
            return;

        for (int i = 0; i < preset.Statuses.Length; i++)
        {
            if(preset.Statuses[i].Data != null)
            {
                AddSlotUnit(preset.Statuses[i]);
            }
        }
    }

    private void OnDestroy()
    {
        UnSubscrube();
    }

    private void Subscribe()
    {       
        _unitController.OnUnitChanged += _unitUIManager.FightSlotController.Init;
        _unitController.SynergyController.OnSynergyChanged += _unitStanbyUIManager.SynergySlotPanel.UpdateSynergySlot;
        _unitController.SynergyController.OnSynergyChanged += _unitStanbyUIManager.SynergyPanel.UpdateSynergySlot;

        _unitController.OnUnitCountChanged += _unitStanbyUIManager.UnitCountPanel.UpdateUnitCount;

        for (int i = 0; i < _unitStanbyUIManager.UnitTotalPowerPanel.Length; i++)
        {
            _unitController.OnUnitPowerChanged += _unitStanbyUIManager.UnitTotalPowerPanel[i].UpdateTotalPower;
        }
    }

    private void UnSubscrube()
    {
        _unitController.OnUnitChanged -= _unitUIManager.FightSlotController.Init;
        _unitController.SynergyController.OnSynergyChanged -= _unitStanbyUIManager.SynergySlotPanel.UpdateSynergySlot;
        _unitController.SynergyController.OnSynergyChanged -= _unitStanbyUIManager.SynergyPanel.UpdateSynergySlot;

        _unitController.OnUnitCountChanged -= _unitStanbyUIManager.UnitCountPanel.UpdateUnitCount;

        for (int i = 0; i < _unitStanbyUIManager.UnitTotalPowerPanel.Length; i++)
        {
            _unitController.OnUnitPowerChanged -= _unitStanbyUIManager.UnitTotalPowerPanel[i].UpdateTotalPower;
        }
    }
    
    public void Fight()
    {
        if(_unitController.GetUnitsCount() == 0)
            return;

        _battleManager.Init(_unitController.GetUnits(), _enemyController.GetUnits());

        _unitController.UnitFight();
        _enemyController.EnemyFight();
      
        FightUISetup();       
    }

    private void FightUISetup()
    {
        _unitUIManager.BattleUIInit();

        _unitUIManager.DamageMeterController.Init(_unitController.GetUnits());
        _unitUIManager.SkillPopUpController.Init(_unitController.GetUnits(), _enemyController.GetUnits());
        _unitUIManager.HpMeterController.Init(_unitController.GetUnits(), _enemyController.GetUnits());
    }

    public void RandomSpawn()
    {
        UnitData unit = _testDatas[Random.Range(0, _testDatas.Length)];
        UnitStatus unitStatus = new UnitStatus(unit);

        AddSlotUnit(unitStatus);
    }

    public void AddSlotUnit(UnitStatus unit)
    {
        int slotIdx = _unitSlotController.GetEmptySlot();

        if (slotIdx == -1)
        {
            Debug.Log("슬롯이 부족합니다.");
            return;
        }

        SetSlot(unit, slotIdx);
    }

    public void AddBattleUnit(UnitStatus unit, UnitSlot slot)
    {
        UnitBase unitBase = Instantiate(unit.Data.UnitPrefab).GetComponent<UnitBase>();
        unitBase.Status = unit;
        unitBase.Init();

        _unitController.AddUnit(slot, unitBase);
    }

    private void SetSlot(UnitStatus unit, int idx)
    {
        _unitSlotController.SetSlot(unit, idx);

        CheckUpgrade(unit);
    }

    private void CheckUpgrade(UnitStatus unit)
    {
        if (unit.Level == 2)
        {
            Debug.Log($"최종 유닛 {unit.Data.Name} 업그레이드 불가");
            return;
        }

        if (GetUnitCount(unit) >= 3)
        {
            UpgradeUnit(unit);
        }
    }

    private void UpgradeUnit(UnitStatus unit)
    {
        UnitStatus newUnit = new UnitStatus(unit.Data, unit.Level + 1);

        int upgradeNeedCount = _upgradeNeedCount;

        int slotCount = _unitSlotController.GetUnitCount(unit);
        int unitCount = _unitController.GetUnitCount(unit);

        Vector2Int pos = Vector2Int.zero;

        for (int i = 0; i < slotCount; i++)
        {
            upgradeNeedCount--;
            _unitSlotController.RemoveLastUnit(unit);
        }

        for (int i = 0; i < upgradeNeedCount; i++)
        {
            pos = _unitController.RemoveUnit(unit);
        }

        if (pos != Vector2Int.zero && !_unitController.IsUnitMaxCount())
        {
            UnitBase unitBase = Instantiate(newUnit.Data.UnitPrefab).GetComponent<UnitBase>();
            unitBase.Status = newUnit;
            unitBase.Init();

            _unitController.AddUnit(unitBase, pos);
        }
        else
        {
            _unitSlotController.SetSlot(newUnit, _unitSlotController.GetEmptySlot());
        }

        CheckUpgrade(newUnit);
    }

    private int GetUnitCount(UnitStatus unit)
    {
        return _unitController.GetUnitCount(unit) + _unitSlotController.GetUnitCount(unit);
    }
}
