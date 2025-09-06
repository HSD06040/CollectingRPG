using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static event Action OnBattleStarted;
    public static event Action OnBattleEnded;
    public static Action<UnitBase> OnSpawnUnit;

    [SerializeField] LayerMask _playerLayer;

    [Header("UnitCount")]
    private int _playerUnitCount;
    private int _enemyUnitCount;

    private void OnDestroy()
    {
        OnBattleStarted = null;
        OnBattleEnded = null;
        OnSpawnUnit = null;
    }

    public void Init(UnitBase[] playerUnits, UnitBase[] enemyUnits)
    {
        UnitBase[] notNullPlayerUnits = GetNotNullUnits(playerUnits);
        UnitBase[] notNullEnemyUnits = GetNotNullUnits(enemyUnits);

        RegisterEvent(notNullPlayerUnits);
        RegisterEvent(notNullEnemyUnits);

        _playerUnitCount = notNullPlayerUnits.Length;
        _enemyUnitCount = notNullEnemyUnits.Length;

        OnSpawnUnit += SpawnUnit;
    }

    private void SpawnUnit(UnitBase unit)
    {
        _playerUnitCount++;
        unit.StatusController.OnUnitDied += CheckBattleEnded;
    }

    public void BattleStart()
    {
        OnBattleStarted?.Invoke();
    }

    private void CheckBattleEnded(UnitStatusController statusCon)
    {
        if (!statusCon.IsDead) return;

        if (_playerLayer.Contain(statusCon.gameObject.layer))
        {
            _playerUnitCount--;
        }
        else
        {
            _enemyUnitCount--;
        }
        Debug.Log($"플레이어 유닛 수: {_playerUnitCount}, 적 유닛 수: {_enemyUnitCount}");
        statusCon.OnUnitDied -= CheckBattleEnded;

        if (_playerUnitCount > 0 && _enemyUnitCount > 0)
            return;

        if (_playerUnitCount <= 0)
        {
            Debug.Log("플레이어 패배");
        }
        else if (_enemyUnitCount <= 0)
        {
            Debug.Log("플레이어 승리");
        }

        OnBattleEnded?.Invoke();
    }

    private UnitBase[] GetNotNullUnits(UnitBase[] units)
    {
        List<UnitBase> notNullUnits = new List<UnitBase>();
        foreach (var unit in units)
        {
            if (unit != null)
                notNullUnits.Add(unit);
        }
        return notNullUnits.ToArray();
    }

    private void RegisterEvent(UnitBase[] units)
    {
        for (int i = 0; i < units.Length; i++)
        {
            units[i].StatusController.OnUnitDied += CheckBattleEnded;
        }
    }

    private void UnRegisterEvent(UnitBase[] units)
    {
        for (int i = 0; i < units.Length; i++)
        {
            units[i].StatusController.OnUnitDied -= CheckBattleEnded;
        }
    }
}
