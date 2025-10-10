using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitStatusUI : MonoBehaviour
{
    [SerializeField] GridLayoutGroup _gridLayoutGroup;
    [SerializeField] StatusSlot _physicalDamageSlot;
    [SerializeField] StatusSlot _magicDamageSlot;
    [SerializeField] StatusSlot _physicalDefenseSlot;
    [SerializeField] StatusSlot _magicDefenseSlot;
    [SerializeField] StatusSlot _critChanceSlot;
    [SerializeField] StatusSlot _attackSpeedSlot;
    [SerializeField] StatusSlot _attackRangeSlot;

    private void Awake()
    {
        _gridLayoutGroup.SetupGridLayoutGroup(transform, 6, 1, _gridLayoutGroup.cellSize, 0);
    }

    public void Setup(UnitStats stat)
    {
        _physicalDamageSlot.Setup(stat.PhysicalDamage);
        _magicDamageSlot.Setup(stat.MagicDamage);
        _physicalDefenseSlot.Setup(stat.PhysicalDefense);
        _magicDefenseSlot.Setup(stat.MagicDefense);
        _critChanceSlot.Setup(stat.CritChance);
        _attackSpeedSlot.Setup(stat.AttackSpeed);
        _attackRangeSlot.Setup(stat.AttackRange);
    }
}
