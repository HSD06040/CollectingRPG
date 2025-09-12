using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class UnitManager : MonoBehaviour
{
    [Header("Test")]
    public bool IsTest;

    [Header("Center")]
    [SerializeField] Transform _center;

    [Header("BattleManager")]
    [SerializeField] BattleManager _battleManager;

    [Header("UI")]
    [SerializeField] UnitUIManager _unitUIManager;
    [SerializeField] UI_UnitSlotController _unitSlotController;

    [Header("Unit_Controller")]
    public UnitController UnitController;
    public EnemyController EnemyController;

    [Header("Data")]
    [SerializeField] UnitData[] _unitDatas;
    [SerializeField] int _upgradeNeedCount = 3;

    private List<UnitBase> _spawnUnitList = new List<UnitBase>(10);

    private void Awake()
    {
        if (IsTest)
            CsvDownloader.OnDataSetupCompleted += InitAsync;
        else
            InitAsync();
    }
    private void OnDestroy()
    {
        UnSubscrube();        
    }

    private async void InitAsync()
    {
        await Manager.Resources.LoadLabel("Stage");

        Manager.Data.SynergyDB.ResetSynergys();

        UnitController.Init();
        EnemyController.Init();

        //_unitDatas = Manager.Data.UnitDataDic.Values.ToArray();
        _unitDatas = Manager.Data.EnemyUnitDatas;

        Subscribe();

        _unitUIManager.SynergyPanel.Init(Manager.Data.SynergyDB);
        _unitUIManager.SynergySlotPanel.Init(Manager.Data.SynergyDB);

        if (TempDataManager.Instance == null)
            return;

        TeamPresetData preset = TempDataManager.Instance.ReadCurrentSelectedPreset();

        if (preset == null)
            return;

        for (int i = 0; i < preset.Statuses.Length; i++)
        {
            if (preset.Statuses[i].Data != null)
            {
                AddSlotUnit(preset.Statuses[i], _unitSlotController.GetEmptySlot());
            }
        }
    }

    //public void Init()
    //{
    //    //Manager.Pool.PopUpInit();

    //    Manager.Data.SynergyDB.ResetSynergys();

    //    UnitController.Init();
    //    _unitUIManager.Init();

    //    _unitDatas = Manager.Data.UnitDatas;

    //    Subscribe();

    //    _unitUIManager.SynergyPanel.Init(Manager.Data.SynergyDB);
    //    _unitUIManager.SynergySlotPanel.Init(Manager.Data.SynergyDB);
                       
    //    TeamPresetData preset = TempDataManager.Instance.ReadCurrentSelectedPreset();

    //    if (preset == null)
    //        return;

    //    for (int i = 0; i < preset.Statuses.Length; i++)
    //    {
    //        if(preset.Statuses[i].Data != null)
    //        {
    //            AddSlotUnit(preset.Statuses[i], _unitSlotController.GetEmptySlot());
    //        }
    //    }
    //}

    #region EventHandler
    private void Subscribe()
    {
        BattleManager.OnSpawnUnit += SpawnUnitAdded;
        BattleManager.OnBattleEnded += GameEndedUnitStandby;

        UnitController.OnUnitChanged += _unitUIManager.FightSlotController.Init;
        UnitController.SynergyController.OnSynergyChanged += _unitUIManager.SynergySlotPanel.UpdateSynergySlot;
        UnitController.SynergyController.OnSynergyChanged += _unitUIManager.SynergyPanel.UpdateSynergySlot;

        UnitController.OnUnitCountChanged += _unitUIManager.UnitCountPanel.UpdateUnitCount;

        for (int i = 0; i < _unitUIManager.UnitTotalPowerPanel.Length; i++)
        {
            UnitController.OnUnitPowerChanged += _unitUIManager.UnitTotalPowerPanel[i].UpdateTotalPower;
        }
    }

    private void UnSubscrube()
    {
        BattleManager.OnSpawnUnit -= SpawnUnitAdded;
        BattleManager.OnBattleEnded -= GameEndedUnitStandby;

        UnitController.OnUnitChanged -= _unitUIManager.FightSlotController.Init;
        UnitController.SynergyController.OnSynergyChanged -= _unitUIManager.SynergySlotPanel.UpdateSynergySlot;
        UnitController.SynergyController.OnSynergyChanged -= _unitUIManager.SynergyPanel.UpdateSynergySlot;

        UnitController.OnUnitCountChanged -= _unitUIManager.UnitCountPanel.UpdateUnitCount;

        for (int i = 0; i < _unitUIManager.UnitTotalPowerPanel.Length; i++)
        {
            UnitController.OnUnitPowerChanged -= _unitUIManager.UnitTotalPowerPanel[i].UpdateTotalPower;
        }
    }
    #endregion

    #region Fight
    public void Fight()
    {
        if(UnitController.GetUnitsCount() == 0)
            return;

        _unitUIManager.StandbyUIDeActive();

        FightRoutine().Forget();        
    }

    private async UniTask FightRoutine()
    {
        SlotsDeActive();

        await Camera.main.DOFieldOfView(120, 0.5f).SetEase(Ease.OutQuad).AsyncWaitForCompletion();

        await UniTask.Delay(TimeSpan.FromSeconds(.2f));
        UnitsMove();

        UnitController.BattleParent.DOMoveX(_center.position.x - 5, 2).SetEase(Ease.Linear);
        Camera.main.transform.DOMoveX(_center.position.x, 2);
        await UniTask.Delay(TimeSpan.FromSeconds(1));
        EnemyController.BattleParent.DOMoveX(-(_center.position.x - 5), 1).SetEase(Ease.Linear);
        await UniTask.Delay(TimeSpan.FromSeconds(1));

        UnitsIdle();

        await UniTask.Delay(TimeSpan.FromSeconds(0.3f));

        UnitController.UnitFight();
        EnemyController.EnemyFight();

        FightUISetup();
        _battleManager.BattleStart();
        _battleManager.Init(UnitController.GetUnits(), EnemyController.GetUnits());
    }

    private void SlotsDeActive()
    {
        UnitController.SlotsDeActive();
        EnemyController.SlotsDeActive();
    }

    private void FightUISetup()
    {
        _unitUIManager.BattleUISetting();

        _unitUIManager.DamageMeterController.Init(UnitController.GetUnits());
        _unitUIManager.SkillPopUpController.Init(UnitController.GetUnits(), EnemyController.GetUnits());
        _unitUIManager.HpMeterController.Init(UnitController.GetUnits(), EnemyController.GetUnits());
        _unitUIManager.UnitHealthBarManager.Init(UnitController.GetUnits(), EnemyController.GetUnits());
    }
    #endregion
    public void GameEndedUnitStandby()
    {
        SpawnUnitStnaby();
        UnitController.UnitsGameEndedStandby();
        EnemyController.EnemyStandby();
    }

    public void StandbyGame()
    {
        ClearSpawnUnit();
        EnemyController.ResetEnemy();
        UnitController.UnitStandbyAndSetSlotPosition();
        _unitUIManager.StandbyUISetting();

        _battleManager.GameStanby();
    }

    private void UnitsIdle()
    {
        UnitController.UnitIdle();
        EnemyController.EnemyIdle();
    }

    private void UnitsMove()
    {
        UnitController.UnitMove();
        EnemyController.EnemyMove();
    }

    #region Unit
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
        UnitData unit = _unitDatas[UnityEngine.Random.Range(0, _unitDatas.Length)];
        UnitStatus unitStatus = new UnitStatus(unit);

        AddSlotUnit(unitStatus, slotIdx);
    }

    public void AddSlotUnit(UnitStatus unit, int slotIdx)
    {
        SetSlot(unit, slotIdx);
    }

    public void AddBattleUnit(UnitStatus unit, UnitSlot slot)
    {
        GameObject obj = Instantiate(unit.Data.UnitPrefab);
        UnitBase unitBase = ComponentProvider.Get<UnitBase>(obj);
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
            GameObject obj = Instantiate(newUnit.Data.UnitPrefab);
            UnitBase unitBase = ComponentProvider.Get<UnitBase>(obj);
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
    #endregion

    private void SpawnUnitAdded(UnitBase unit)
    {
        _spawnUnitList.Add(unit);
    }

    private void SpawnUnitStnaby()
    {
        foreach (var unit in _spawnUnitList)
        {
            unit.GameEndedStanby();
        }
    }

    private void ClearSpawnUnit()
    {
        foreach (var unit in _spawnUnitList)
        {
            Destroy(unit.gameObject);
        }

        _spawnUnitList.Clear();
    }
}
