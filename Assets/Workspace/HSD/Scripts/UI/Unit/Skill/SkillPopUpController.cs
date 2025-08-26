using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillPopUpController : MonoBehaviour
{
    [SerializeField] GameObject _playerUnitSkillPopUpPrefab;
    [SerializeField] GameObject _enemyUnitSkillPopUpPrefab;

    [SerializeField] Transform _playerContent;
    [SerializeField] Transform _enemyContent;

    [SerializeField] UnitBase[] _playerUnits;
    [SerializeField] UnitBase[] _enemyUnits;

    public void Init(UnitBase[] playerUnits, UnitBase[] enemyUnits)
    {
        if(_playerUnits != null && _playerUnits.Length != 0)
        {
            foreach (UnitBase unit in _playerUnits)
            {
                if(unit != null)
                    unit.StatusController.UseSkill -= AddPlayerSkillPopUp;
            }
        }
            
        if (_enemyUnits != null && _enemyUnits.Length != 0)
        {
            foreach (UnitBase unit in _enemyUnits)
            {
                if(unit != null)
                    unit.StatusController.UseSkill -= AddEnemySkillPopUp;
            }
        }
            
        _playerUnits = playerUnits;
        _enemyUnits = enemyUnits;

        if (_playerUnits != null && _playerUnits.Length != 0)
        {
            foreach (UnitBase unit in _playerUnits)
            {
                if (unit != null)
                    unit.StatusController.UseSkill += AddPlayerSkillPopUp;
            }
        }

        if (_enemyUnits != null && _enemyUnits.Length != 0)
        {
            foreach (UnitBase unit in _enemyUnits)
            {
                if (unit != null)
                    unit.StatusController.UseSkill += AddEnemySkillPopUp;
            }
        }
    }

    public void AddEnemySkillPopUp(UnitStatus status)
    {
        Instantiate(_enemyUnitSkillPopUpPrefab, _enemyContent).GetComponent<SkillPopUp>().Setup(status);
    }

    public void AddPlayerSkillPopUp(UnitStatus status)
    {
        Instantiate(_playerUnitSkillPopUpPrefab, _playerContent).GetComponent<SkillPopUp>().Setup(status, true);
    }
}
