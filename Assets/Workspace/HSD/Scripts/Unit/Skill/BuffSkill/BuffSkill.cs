using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "BuffSkill", menuName = "Data/Unit/Skill/BuffSkill")]
public class BuffSkill : UnitSkill
{
    [Header("Range Setting")]
    [SerializeField] bool _isRange;
    [SerializeField] float _range;

    [Header("Buff")]
    [SerializeField] private TargetType TargetType;
    [SerializeField] private BuffEffectData BuffEffectData;

    public override void Active(IAttacker attacker)
    {
        base.Active(attacker);

        if(TargetType == TargetType.Self)
        {
            attacker.GetStatusController().ApplyEffect(BuffEffectData, (int)Power, name);
        }

        foreach (GameObject target in GetTargetFromTargetType(attacker))
        {
            ComponentProvider.Get<UnitBase>(target).StatusController.ApplyEffect(BuffEffectData, (int)Power, name);
        }
    }

    protected GameObject[] GetTargetFromTargetType(IAttacker attacker)
    {
        float range = _isRange ? _range : 100;

        switch (TargetType)
        {            
            case TargetType.Ally:
                return Utils.GetTargetsNonAlloc(
                    attacker, attacker.GetTransform().position, SearchType.Circle, range,
                    Vector2.zero, 0, MaxCount, GetAllyLayerMask(attacker));
            case TargetType.Enemy:
                return Utils.GetTargetsNonAlloc(
                    attacker, attacker.GetTransform().position, SearchType.Circle, range,
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

    public override void DrawGizmos(IAttacker attacker)
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(attacker.GetTransform().position, _range);
    }
}