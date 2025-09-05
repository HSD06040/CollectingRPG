using System;

#region Synergy
public enum ClassType
{
    Tank = 0, Melee, Ranged, Support
}

public enum Synergy
{
    KingdomGuard = 4,     // 왕국 경비대
    ForestPatrol,         // 숲의 순찰자
    HighMageOrder,        // 고위 마법사단
    SacredOrder,          // 신성 교단
    NightStreetAssassins, // 밤거리 암살단
    UndergroundOrg,      // 지하 조직
    Length
}
#endregion

public enum Grade
{
    Normal, Rare, Unique, Legendary
}

#region Type

public enum SpawnPositionType
{
    Self,
    Target
}

public enum UnitMultiplierType
{
    None,
    UnitLevel,
}

public enum PopupType
{
    PhysicalDamage,
    MagicDamage,
    Heal,
    Mana,
    Buff,
    Debuff,    
    Crit
}

public enum MagicStoneSearchType
{
    All,
    LowHp,
    Random
}

/// <summary>
/// 유닛 레벨에 따른 가중치 곱하기 or 1단계 아래 레벨
/// </summary>
public enum SpawnStatType
{
    Level,
    LowUpgrade
}

/// <summary>
/// 이벤트 트리거 타입
/// </summary>
public enum TriggerType
{
    Base,
    OnBattleStart,
    OnDied,
    OnAttack,
    OnUseSkill,
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
public enum EffectApplyType
{
    Self,
    All,    
}

public enum TargetType
{
    Enemy,
    Ally,
    Self,
    Boss
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

public enum CsvType
{
    UnitStat,
    Skill,
}

public enum AutoUnitType
{
    Unit,
    Slot
}

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