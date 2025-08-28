using System;

public struct SynergyStatModifier
{
    public TargetType TargetType;
    public StatType StatType;
    public int Value;
}

public struct BuffEffectData
{  
    public StatType StatType;
    public float Duration;
    public bool IsTicking;
    public float TickInterval;
}
public struct BuffKey : IEquatable<BuffKey>
{
    public StatType StatType { get; }
    public string Source { get; }

    public BuffKey(StatType statType, string source)
    {
        StatType = statType;
        Source = source;
    }

    // Dictionary에서 키 Equals, GetHashCode 구현
    public bool Equals(BuffKey other) =>
        StatType == other.StatType && Source == other.Source;

    public override bool Equals(object obj) =>
        obj is BuffKey other && Equals(other);

    public override int GetHashCode() =>
        HashCode.Combine(StatType, Source);

    public override string ToString() => $"{StatType} ({Source})";
}