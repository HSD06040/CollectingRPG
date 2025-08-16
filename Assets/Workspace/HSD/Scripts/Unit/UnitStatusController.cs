using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitStatusController : MonoBehaviour, IDamageable
{
    public UnitData Data { get; private set; }
    public Property<int> CurHp = new Property<int>();
    public Property<int> CurMana = new Property<int>();

    public void Init(UnitData data)
    {
        Data = data;
        CurHp.Value = Data.MaxHealth.Value;
        CurMana.Value = Data.MaxMana.Value;
    }

    public void TakeDamage(int amount, DamageType damageType)
    {
        int defense = damageType == DamageType.Magic ? Data.MagicDefense.Value : Data.PhysicalDefense.Value;
        int totalDamage = Utils.CalculateFinalDamage(amount, defense, damageType);
        CurHp.Value -= totalDamage;
        Debug.Log($"현재 체력 :{CurHp.Value}, 받은 데미지 : {totalDamage}");
    }
}
