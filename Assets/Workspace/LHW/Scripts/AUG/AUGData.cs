using UnityEngine;

[CreateAssetMenu(fileName = "AUGEffect", menuName = "Data/AUG/AUGEffect")]
public class AUGData : MetaData
{
    public string AUGID;
    public SubGrade Grade;

    public EffectTargetType TargetType;
    public ClassType Class;

    public TriggerType Trigger;
    public EffectType EffectType;
    public EffectTime ApplyTime;

    public StatType[] StatTypes;

    public float[] Rate = new float[3];

    private float currentRate;

    public void ApplyRate(SubGrade grade)
    {
        switch (grade)
        {
            case SubGrade.SILVER: currentRate = Rate[0]; break;
            case SubGrade.GOLD: currentRate = Rate[1]; break;
            case SubGrade.PRISM: currentRate = Rate[2]; break;
            default: currentRate = 0; break;
        }
    }

    public void ApplyEffect(UnitBase unit)
    {
        ApplyRate(Grade);
        for (int i = 0; i < StatTypes.Length; i++)
        {
            unit.Status.Data.UnitStats[0].AddAugments(StatTypes[i], currentRate, Name);
        }
    }
}