using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitUIManager : MonoBehaviour
{
    [SerializeField] GameObject _stanbyUI;
    [SerializeField] GameObject _battleUI;
    public FightUnitSlotController FightSlotController;
    public DamageMeterController DamageMeterController;

    public void Init()
    {
        _stanbyUI.SetActive(false);
        _battleUI.SetActive(true);
        FightSlotController.gameObject.SetActive(true);
        DamageMeterController.gameObject.SetActive(true);
    }
}
