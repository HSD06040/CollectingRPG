
public enum ClassType
{
    Tank = 0, Melee, Ranged, Support
}

public enum Synergy
{
    A = 4, B, C, D, Length
}

public enum AttackAreaType
{
    Single, Radius, Pierce, Row
}

public enum DamageType
{
    Physical, Magic
}

public enum TargetType
{
    Enemy, Ally, Self, SameSynergy
}

public enum SearchType
{
    Circle, Box, Capsule
}

public enum Grade
{
    Normal, Rare, Unique, Legendary
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