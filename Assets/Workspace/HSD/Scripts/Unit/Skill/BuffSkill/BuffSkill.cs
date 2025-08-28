using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffSkill : UnitSkill
{
    [Header("Buff")]
    [SerializeField] private TargetType TargetType;
    [SerializeField] private BuffEffectData BuffEffectData;

    public override void Active(IAttacker attacker)
    {
        base.Active(attacker);

        foreach (GameObject target in GetTargetFromTargetType(attacker))
        {
            ComponentProvider.Get<UnitStatusController>(target).ApplyEffect(BuffEffectData, (int)Power, name);
        }
    }

    protected GameObject[] GetTargetFromTargetType(IAttacker attacker)
    {
        switch (TargetType)
        {
            case TargetType.Self:
                return new GameObject[] { attacker.GetTransform().gameObject };
            case TargetType.Ally:
                return Utils.GetTargetsNonAlloc(
                    attacker, attacker.GetTransform().position, SearchType.Circle, 100f,
                    Vector2.zero, 0, MaxCount, GetAllyLayerMask(attacker));
            case TargetType.Enemy:
                return Utils.GetTargetsNonAlloc(
                    attacker, attacker.GetTransform().position, SearchType.Circle, 100f,
                    Vector2.zero, 0, MaxCount, attacker.TargetLayer);
            default:
                return null;
        }
    }

    /// <summary>
    /// Attacker의 아군 LayerMask를 반환
    /// </summary>    
    private LayerMask GetAllyLayerMask(IAttacker attacker)
    {
        return attacker.TargetLayer == LayerMask.GetMask("Enemy") ? LayerMask.GetMask("Player") : LayerMask.GetMask("Enemy");
    }
}