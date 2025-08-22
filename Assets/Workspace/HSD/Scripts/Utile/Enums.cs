
public enum ClassType
{
    Tank, Melee, Ranged, Support
}

public enum Synergy
{
    A, B, C, D
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

    // Range
    AttackRange,
    AttackCount
}