using System;
using UnityEngine;

[Serializable]
public struct SynergyStatModifier
{
    public StatType StatType;
    public float Value;
}

[Serializable]
public struct SynergyBuffData
{
    public StatType StatType;
    public float Duration;
    public float Value;
}

[Serializable]
public struct BuffEffectData
{  
    public StatType StatType;
    public float Duration;
    public bool IsTicking;
    public float TickInterval;
}

[Serializable]
public struct CsvData
{
    public CsvType CsvType;
    [TextArea]
    public string URL;
    public int StartLine;
    [Header("예시 : A2:C12")]
    public string Range;
}

public readonly struct SourceKey : IEquatable<SourceKey>
{
    public StatType StatType { get; }
    public string Source { get; }

    public SourceKey(StatType statType, string source)
    {
        StatType = statType;
        Source = source;
    }

    public bool Equals(SourceKey other) =>
        StatType == other.StatType && Source == other.Source;

    public override bool Equals(object obj) =>
        obj is SourceKey other && Equals(other);

    public override int GetHashCode() =>
        HashCode.Combine(StatType, Source);

    public override string ToString() => $"{StatType} ({Source})";
}