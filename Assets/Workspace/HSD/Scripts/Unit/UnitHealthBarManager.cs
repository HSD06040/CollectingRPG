using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitHealthBarManager : MonoBehaviour
{
    [SerializeField] Transform _healthBarContent;    
    [SerializeField] string _healthBarAddress;

    public void Init(UnitBase[] playerUnits, UnitBase[] enemyUnits)
    {
        foreach (var unit in playerUnits)
        {          
            SetHealthBar(unit);
        }        

        foreach (var unit in enemyUnits)
        {
            SetHealthBar(unit);
        }
    }

    public void SetHealthBar(UnitBase unit)
    {
        if (unit == null) return;

        UnitHealthBar bar = Manager.Resources.Instantiate<GameObject>(
            _healthBarAddress,
            unit.GetBarPosition(),
            Quaternion.identity,
            _healthBarContent,
            true
            ).GetComponent<UnitHealthBar>();

        bar.Setup(unit);
    }

    public void Clear()
    {
        foreach (Transform healthBar in _healthBarContent)
        {            
            Manager.Resources.Destroy(healthBar.gameObject);
        }
    }
}
