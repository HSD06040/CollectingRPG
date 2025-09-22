using System.Collections.Generic;
using UnityEngine;

public class AugmentManager : InGameSingleton<AugmentManager>
{
    #region Stat
    [Header("Status")]
    public Stat<int> MaxHealth;
    public Stat<float> AttackSpeed;

    [Header("Damage")]
    public Stat<int> PhysicalDamage;
    public Stat<int> MagicDamage;

    [Header("Defense")]
    public Stat<int> PhysicalDefense;
    public Stat<int> MagicDefense;

    [Header("CritRate")]
    public Stat<int> CritChance;

    [Header("Currency")]
    public Stat<float> GoldBonus;

    [Header("CurrentAugment")]
    public AUGData currentAugment;
    // 테스트용
    [SerializeField] private AUGData AUGData;

    private Dictionary<UnitBase, HashSet<string>> _appliedAugments = new();
    #endregion

    private void Start()
    {
        SelectAugment(AUGData);
    }

    public void SelectAugment(AUGData data)
    {
        currentAugment = data;
    }

    public void ApplyAugment(UnitBase unit)
    {
        if(currentAugment == null) return;
        if(!IsAugmentTarget(unit)) return;

        if(!_appliedAugments.ContainsKey(unit))
            _appliedAugments[unit] = new HashSet<string>();

        if (_appliedAugments[unit].Contains(currentAugment.AUGID))
        {
            Debug.LogWarning($"[AugmentManager] {unit.name} 에 {currentAugment.AUGID} 는 이미 적용됨.");
            return;
        }

        _appliedAugments[unit].Add(currentAugment.AUGID);
        currentAugment.ApplyEffect(unit);
    }

    public void ReleaseAugment(UnitBase unit)
    {
        if (currentAugment == null) return;

        if (_appliedAugments.TryGetValue(unit, out var augments))
        {
            if (augments.Contains(currentAugment.AUGID))
            {
                currentAugment.RemoveEffect(unit);
                augments.Remove(currentAugment.AUGID);

                if (augments.Count == 0)
                    _appliedAugments.Remove(unit);
            }
            else
            {
                Debug.LogWarning($"[AugmentManager] {unit.name} 에 {currentAugment.AUGID} 는 적용되지 않아 해제할 수 없음.");
            }
        }
    }

    public bool IsAugmentTarget(UnitBase unit)
    {
        if (currentAugment.TargetType == EffectTargetType.Ally) return true;
        else if (currentAugment.TargetType == EffectTargetType.SameClassType && unit.Status.Data.ClassSynergy == currentAugment.Class)
        {
            Debug.Log("클래스가 같음");
            return true;
        }
        // 리더일 경우 추가 필요

        return false;
    }

    #region Augument Management
    public void AddAugment(StatType statType, float value, string source)
    {
        switch (statType)
        {
            case StatType.MaxHealth:
                MaxHealth.AddModifier((int)value, source);
                break;
            case StatType.PhysicalDamage:
                PhysicalDamage.AddModifier((int)value, source);
                break;
            case StatType.MagicDamage:
                MagicDamage.AddModifier((int)value, source);
                break;
            case StatType.CritChance:
                CritChance.AddModifier((int)value, source);
                break;
            case StatType.PhysicalDefense:
                PhysicalDefense.AddModifier((int)value, source);
                break;
            case StatType.MagicDefense:
                MagicDefense.AddModifier((int)value, source);
                break;
            case StatType.AttackSpeed:
                AttackSpeed.AddModifier(value, source);
                break;
            case StatType.GoldBonus:
                GoldBonus.AddModifier(value, source);
                break;
        }
    }

    public void RemoveAugment(StatType statType, string source)
    {
        switch (statType)
        {
            case StatType.MaxHealth:
                MaxHealth.RemoveModifier(source);
                break;
            case StatType.PhysicalDamage:
                PhysicalDamage.RemoveModifier(source);
                break;
            case StatType.MagicDamage:
                MagicDamage.RemoveModifier(source);
                break;
            case StatType.CritChance:
                CritChance.RemoveModifier(source);
                break;
            case StatType.PhysicalDefense:
                PhysicalDefense.RemoveModifier(source);
                break;
            case StatType.MagicDefense:
                MagicDefense.RemoveModifier(source);
                break;
            case StatType.AttackSpeed:
                AttackSpeed.RemoveModifier(source);
                break;
            case StatType.GoldBonus:
                GoldBonus.RemoveModifier(source);
                break;
        }
    }
    #endregion
}
