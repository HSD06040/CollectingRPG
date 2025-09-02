using UnityEngine;

public class UnitManager : MonoBehaviour
{
    [Header("Test")]
    public bool IsTest;

    [Header("BattleManager")]
    [SerializeField] BattleManager _battleManager;

    [Header("UI")]
    [SerializeField] UnitUIManager _unitUIManager;
    [SerializeField] UnitStanbyUIManager _unitStanbyUIManager;
    [SerializeField] UI_UnitSlotController _unitSlotController;

    [Header("Unit_Controller")]
    public UnitController UnitController;
    public EnemyController _enemyController;

    [Header("Data")]
    [SerializeField] UnitData[] _unitDatas;
    [SerializeField] int _upgradeNeedCount = 3;
    
    private void Awake()
    {
        Utils.Initialize();

        if(IsTest)
            CsvDownloader.OnDataSetupCompleted += Init;
        else
            Init();
    }

    private void Init()
    {
        UnitController.Init();
        _unitStanbyUIManager.Init();

        _unitDatas = Manager.Data.UnitDatas;

        Subscribe();

        _unitStanbyUIManager.SynergyPanel.Init(Manager.Data.SynergyDB);
        _unitStanbyUIManager.SynergySlotPanel.Init(Manager.Data.SynergyDB);
                       
        TeamPresetData preset = TempDataManager.Instance.ReadCurrentSelectedPreset();

        if (preset == null)
            return;

        for (int i = 0; i < preset.Statuses.Length; i++)
        {
            if(preset.Statuses[i].Data != null)
            {
                AddSlotUnit(preset.Statuses[i], _unitSlotController.GetEmptySlot());
            }
        }
    }

    private void OnDestroy()
    {
        UnSubscrube();
    }

    #region EventHandler
    private void Subscribe()
    {
        BattleManager.OnBattleEnded += GameEndedUnitStandby;
        UnitController.OnUnitChanged += _unitUIManager.FightSlotController.Init;
        UnitController.SynergyController.OnSynergyChanged += _unitStanbyUIManager.SynergySlotPanel.UpdateSynergySlot;
        UnitController.SynergyController.OnSynergyChanged += _unitStanbyUIManager.SynergyPanel.UpdateSynergySlot;

        UnitController.OnUnitCountChanged += _unitStanbyUIManager.UnitCountPanel.UpdateUnitCount;

        for (int i = 0; i < _unitStanbyUIManager.UnitTotalPowerPanel.Length; i++)
        {
            UnitController.OnUnitPowerChanged += _unitStanbyUIManager.UnitTotalPowerPanel[i].UpdateTotalPower;
        }
    }

    private void UnSubscrube()
    {
        BattleManager.OnBattleEnded -= GameEndedUnitStandby;
        UnitController.OnUnitChanged -= _unitUIManager.FightSlotController.Init;
        UnitController.SynergyController.OnSynergyChanged -= _unitStanbyUIManager.SynergySlotPanel.UpdateSynergySlot;
        UnitController.SynergyController.OnSynergyChanged -= _unitStanbyUIManager.SynergyPanel.UpdateSynergySlot;

        UnitController.OnUnitCountChanged -= _unitStanbyUIManager.UnitCountPanel.UpdateUnitCount;

        for (int i = 0; i < _unitStanbyUIManager.UnitTotalPowerPanel.Length; i++)
        {
            UnitController.OnUnitPowerChanged -= _unitStanbyUIManager.UnitTotalPowerPanel[i].UpdateTotalPower;
        }
    }
    #endregion

    #region Fight
    public void Fight()
    {
        if(UnitController.GetUnitsCount() == 0)
            return;

        UnitController.UnitFight();
        _enemyController.EnemyFight();
        
        FightUISetup();
        _battleManager.BattleStart();
        _battleManager.Init(UnitController.GetUnits(), _enemyController.GetUnits());
    }

    public void GameEndedUnitStandby()
    {
        UnitController.UnitsStanby();
        _enemyController.EnemyStanby();
        
    }

    private void FightUISetup()
    {
        _unitUIManager.BattleUIInit();

        _unitUIManager.DamageMeterController.Init(UnitController.GetUnits());
        _unitUIManager.SkillPopUpController.Init(UnitController.GetUnits(), _enemyController.GetUnits());
        _unitUIManager.HpMeterController.Init(UnitController.GetUnits(), _enemyController.GetUnits());
    }
    #endregion

    public void RandomSpawn()
    {
        int slotIdx = _unitSlotController.GetEmptySlot();

        if (slotIdx == -1)
        {
            Debug.Log("슬롯이 부족합니다.");
            return;
        }

        if (!InGameManager.Instance.SpendGold(InGameManager.Instance.SpawnGold))
        {
            Debug.Log("골드가 부족합니다.");
            return;
        }

        UnitData unit = _unitDatas[Random.Range(0, _unitDatas.Length)];
        UnitStatus unitStatus = new UnitStatus(unit);

        AddSlotUnit(unitStatus, slotIdx);
    }

    public void AddSlotUnit(UnitStatus unit, int slotIdx)
    {
        SetSlot(unit, slotIdx);
    }

    public void AddBattleUnit(UnitStatus unit, UnitSlot slot)
    {
        UnitBase unitBase = Instantiate(unit.Data.UnitPrefab).GetComponent<UnitBase>();
        unitBase.Status = unit;
        unitBase.Init();

        UnitController.AddUnit(slot, unitBase);
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
        int unitCount = UnitController.GetUnitCount(unit);

        Vector2Int pos = Vector2Int.zero;

        for (int i = 0; i < slotCount; i++)
        {
            upgradeNeedCount--;
            _unitSlotController.RemoveLastUnit(unit);
        }

        for (int i = 0; i < upgradeNeedCount; i++)
        {
            pos = UnitController.RemoveUnitGetPosition(unit);
        }

        if (pos != Vector2Int.zero && !UnitController.IsUnitMaxCount())
        {
            UnitBase unitBase = Instantiate(newUnit.Data.UnitPrefab).GetComponent<UnitBase>();
            unitBase.Status = newUnit;
            unitBase.Init();

            UnitController.AddUnit(unitBase, pos);
        }
        else
        {
            _unitSlotController.SetSlot(newUnit, _unitSlotController.GetEmptySlot());
        }

        CheckUpgrade(newUnit);
    }

    private int GetUnitCount(UnitStatus unit)
    {
        return UnitController.GetUnitCount(unit) + _unitSlotController.GetUnitCount(unit);
    }
}
