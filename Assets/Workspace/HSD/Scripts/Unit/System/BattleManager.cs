using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static event Action OnBattleStarted;
    public static event Action OnBattleEnded;

    [SerializeField] LayerMask playerLayer;

    [Header("UnitCount")]
    private int playerUnitCount;
    private int enemyUnitCount;

    public void Init(UnitBase[] playerUnits, UnitBase[] enemyUnits)
    {
        ClearEvent();

        UnitBase[] notNullPlayerUnits = GetNotNullUnits(playerUnits);
        UnitBase[] notNullEnemyUnits = GetNotNullUnits(enemyUnits);

        RegisterEvent(notNullPlayerUnits);
        RegisterEvent(notNullEnemyUnits);

        playerUnitCount = notNullPlayerUnits.Length;
        enemyUnitCount = notNullEnemyUnits.Length;
    }

    public void BattleStart()
    {
        OnBattleStarted?.Invoke();
    }

    private void CheckBattleEnded(UnitStatusController statusCon)
    {
        if (!statusCon.IsDead) return;

        if (playerLayer.Contain(statusCon.gameObject.layer))
        {
            playerUnitCount--;
        }
        else
        {
            enemyUnitCount--;
        }
        Debug.Log($"플레이어 유닛 수: {playerUnitCount}, 적 유닛 수: {enemyUnitCount}");
        statusCon.OnUnitDied -= CheckBattleEnded;

        if (playerUnitCount > 0 && enemyUnitCount > 0)
            return;

        if (playerUnitCount <= 0)
        {
            Debug.Log("플레이어 패배");
        }
        else if (enemyUnitCount <= 0)
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

    private void ClearEvent()
    {
        OnBattleStarted = null;
        OnBattleEnded = null;
    }
}
