
#region Synergy
using System;

public enum ClassType
{
    Tank = 0, Melee, Ranged, Support
}

public enum Synergy
{
    A = 4, B, C, D, Length
}
#endregion


public enum Grade
{
    Normal, Rare, Unique, Legendary
}

#region Type
/// <summary>
/// 이벤트 트리거 타입
/// </summary>
public enum TriggerType
{
    Base,
    OnBattleStart,
    OnAttack,
    OnInterval,
    OnBattleEnded,
}

/// <summary>
/// 효과 대상 타입
/// </summary>
public enum EffectTargetType
{
    Enemy,
    Ally,
    SameSynergy,
    Column,
    Row,
    ColumnAndRow,
    Cross
}

/// <summary>
/// 버프디버프, 즉시증가
/// </summary>
public enum EffectType
{
    Buff_Debuff,
    Increase,    
}

/// <summary>
/// 효과 공격 타입, 각자공격 or 전체공격
/// </summary>
public enum EffectAttackType
{
    Self,
    All,    
}

public enum TargetType
{
    Enemy,
    Ally,
    Self,
}

public enum AttackAreaType
{
    Single, Radius, Pierce, Row
}

public enum DamageType
{
    Physical, Magic
}

public enum SearchType
{
    Circle, Box, Capsule
}

#endregion

public enum Priority
{
    None,
    Close,
    Far,
    LowHp,
    HightHp,
    Tank,
    Melee,
    Ranged,
    Support
}

public enum StatType
{
    // Status
    MaxHealth,
    MaxMana,
    ManaGain,
    AttackSpeed,
    MoveSpeed,

    // Damage
    PhysicalDamage,
    MagicDamage,

    // Crit
    CritChance,
    CritDamage,

    // Defense
    PhysicalDefense,
    MagicDefense,
    Shield,

    // Range
    AttackRange,
    AttackCount,

    // Default
    CurHp,
    CurMana,
}