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

    // 캐릭터 스테이터스 적용
    private Dictionary<UnitBase, HashSet<string>> _appliedStatusAugments = new();

    private Dictionary<AUGData, HashSet<string>> _appliedCurrencyAugments = new();
    #endregion

    private void Start()
    {
        SelectAugment(AUGData);
    }

    public void SelectAugment(AUGData data)
    {
        currentAugment = data;
    }

    #region Unit Status Augment

    /// <summary>
    /// 유닛 스테이터스 증강 추가
    /// </summary>
    /// <param name="unit"></param>
    public void ApplyAugment(UnitBase unit)
    {
        if (currentAugment == null || !IsAugmentTarget(unit) || currentAugment.EffectType != EffectType.Buff_Debuff) return;

        if (!_appliedStatusAugments.ContainsKey(unit))
            _appliedStatusAugments[unit] = new HashSet<string>();

        if (_appliedStatusAugments[unit].Contains(currentAugment.AUGID))
        {
            Debug.LogWarning($"[AugmentManager] {unit.name} 에 {currentAugment.AUGID} 는 이미 적용됨.");
            return;
        }

        _appliedStatusAugments[unit].Add(currentAugment.AUGID);
        currentAugment.ApplyBuffEffect(unit);
    }

    /// <summary>
    /// 유닛 스테이터스 증강 해제
    /// </summary>
    /// <param name="unit"></param>
    public void ReleaseAugment(UnitBase unit)
    {
        if (currentAugment == null) return;

        if (_appliedStatusAugments.TryGetValue(unit, out var augments))
        {
            if (augments.Contains(currentAugment.AUGID))
            {
                currentAugment.RemoveBuffEffect(unit);
                augments.Remove(currentAugment.AUGID);

                if (augments.Count == 0)
                    _appliedStatusAugments.Remove(unit);
            }
            else
            {
                Debug.LogWarning($"[AugmentManager] {unit.name} 에 {currentAugment.AUGID} 는 적용되지 않아 해제할 수 없음.");
            }
        }
    }

    /// <summary>
    /// 유닛 스테이터스 증강 적용 여부 판정
    /// </summary>
    /// <param name="unit"></param>
    /// <returns></returns>
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

    #endregion

    #region Unit Increase

    public void ApplyHealAugment(UnitBase unit)
    {
        if (currentAugment == null || !IsAugmentTarget(unit) || currentAugment.EffectType != EffectType.Increase) return;

        if (!_appliedStatusAugments.ContainsKey(unit))
            _appliedStatusAugments[unit] = new HashSet<string>();

        if (_appliedStatusAugments[unit].Contains(currentAugment.AUGID))
        {
            Debug.LogWarning($"[AugmentManager] {unit.name} 에 {currentAugment.AUGID} 는 이미 적용됨.");
            return;
        }

        _appliedStatusAugments[unit].Add(currentAugment.AUGID);
        currentAugment.ApplyIncreaseEffect(unit);
    }

    public void ReleaseHealAugment(UnitBase unit)
    {
        if (currentAugment == null) return;

        if (_appliedStatusAugments.TryGetValue(unit, out var augments))
        {
            if (augments.Contains(currentAugment.AUGID))
            {
                augments.Remove(currentAugment.AUGID);

                if (augments.Count == 0)
                    _appliedStatusAugments.Remove(unit);
            }
            else
            {
                Debug.LogWarning($"[AugmentManager] {unit.name} 에 {currentAugment.AUGID} 는 적용되지 않아 해제할 수 없음.");
            }
        }
    }

    #endregion

    #region Currency Augment

    /// <summary>
    /// 재화 획득 증강 추가
    /// </summary>
    public void ApplyAugment()
    {
        if (currentAugment == null) return;

        if (!_appliedCurrencyAugments.ContainsKey(currentAugment))
            _appliedCurrencyAugments[currentAugment] = new HashSet<string>();

        if (_appliedCurrencyAugments[currentAugment].Contains(currentAugment.AUGID))
        {
            Debug.LogWarning($"[AugmentManager] {currentAugment.name} 에 {currentAugment.AUGID} 는 이미 적용됨.");
            return;
        }

        _appliedCurrencyAugments[currentAugment].Add(currentAugment.AUGID);
        for (int i = 0; i < currentAugment.StatTypes.Length; i++)
        {
            AddAugment(currentAugment.StatTypes[i], currentAugment.currentRate, currentAugment.Name);
        }
    }

    /// <summary>
    /// 재화 획득 증강 해제
    /// </summary>
    public void ReleaseAugment()
    {
        if (currentAugment == null) return;

        if (_appliedCurrencyAugments.TryGetValue(currentAugment, out var augments))
        {
            if (augments.Contains(currentAugment.AUGID))
            {
                for (int i = 0; i < currentAugment.StatTypes.Length; i++)
                {
                    RemoveAugment(currentAugment.StatTypes[i], currentAugment.Name);
                }
                augments.Remove(currentAugment.AUGID);

                if (augments.Count == 0)
                    _appliedCurrencyAugments.Remove(currentAugment);
            }
            else
            {
                Debug.LogWarning($"[AugmentManager] {currentAugment.name} 에 {currentAugment.AUGID} 는 적용되지 않아 해제할 수 없음.");
            }
        }
    }

    #endregion

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
