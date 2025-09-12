using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchButtonController : MonoBehaviour
{
    [SerializeField] VerticalSwipePager _verticalSwipePager;
    [SerializeField] BattleCameraController _battleCameraController;

    [Header("Buttons")]
    [SerializeField] Button _battleSwitchButton;
    [SerializeField] Button _unitSwitchButton;
    [SerializeField] Button _enemySwitchButton;

    [Header("EnemyButtons")]
    [SerializeField] Button _enemyUnitSwitchButton;
    [SerializeField] Button _enemyBattleSwitchButton;

    private void Start()
    {
        _battleSwitchButton.onClick.AddListener(() => _verticalSwipePager.MoveToPage(1));

        _unitSwitchButton.onClick.AddListener(() => _verticalSwipePager.MoveToPage(0));
        _enemyUnitSwitchButton.onClick.AddListener(() => _verticalSwipePager.MoveToPage(0));

        _enemySwitchButton.onClick.AddListener(EnemySwitchButtonClick);
        _enemyBattleSwitchButton.onClick.AddListener(EnemyUnitSwitchButtonClick);

        _enemyBattleSwitchButton.transform.parent.gameObject.SetActive(false);
        _enemyUnitSwitchButton.transform.parent.gameObject.SetActive(false);
    }

    private void EnemySwitchButtonClick()
    {
        _battleCameraController.MoveToEnemy();
        _enemySwitchButton.transform.parent.gameObject.SetActive(false);
        _unitSwitchButton.transform.parent.gameObject.SetActive(false);

        _enemyBattleSwitchButton.transform.parent.gameObject.SetActive(true);
        _enemyUnitSwitchButton.transform.parent.gameObject.SetActive(true);
    }

    private void EnemyUnitSwitchButtonClick()
    {
        _battleCameraController.MoveToBattle();
        _enemySwitchButton.transform.parent.gameObject.SetActive(true);
        _unitSwitchButton.transform.parent.gameObject.SetActive(true);

        _enemyBattleSwitchButton.transform.parent.gameObject.SetActive(false);
        _enemyUnitSwitchButton.transform.parent.gameObject.SetActive(false);
    }
}
