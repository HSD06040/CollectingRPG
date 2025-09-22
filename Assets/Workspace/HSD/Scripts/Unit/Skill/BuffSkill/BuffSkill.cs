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

        SpawnEffect(attacker);

        if (TargetType == TargetType.Self)
        {
            attacker.GetStatusController().ApplyEffect(BuffEffectData, (int)Power, name);
            return;
        }

        if (Priority == Priority.None)
        {
            foreach (GameObject target in GetTargetFromTargetType(attacker))
            {
                ComponentProvider.Get<UnitBase>(target).StatusController.ApplyEffect(BuffEffectData, Power, name);
            }
        }
        else
        {
            ComponentProvider.Get<UnitBase>(GetTargetPrioty(attacker)).StatusController.ApplyEffect(BuffEffectData, Power, name);
        }
    }

    protected GameObject[] GetTargetFromTargetType(IAttacker attacker)
    {
        float range = _isRange ? _range : 100;
        
        switch (TargetType)
        {            
            case TargetType.Ally:
                return Utils.GetTargetsNonAlloc(
                    attacker, attacker.GetCenter(), SearchType.Circle, range,
                    Vector2.zero, 0, MaxCount, attacker.GetAllyLayerMask());
            case TargetType.Enemy:
                return Utils.GetTargetsNonAlloc(
                    attacker, attacker.GetCenter(), SearchType.Circle, range,
                    Vector2.zero, 0, MaxCount, attacker.TargetLayer);
            default:
                return null;
        }
    }

    private GameObject GetTargetPrioty(IAttacker attacker)
    {
        LayerMask targetLayer;

        if (TargetType == TargetType.Ally)
            targetLayer = attacker.GetAllyLayerMask();
        else if (TargetType == TargetType.Enemy)
            targetLayer = attacker.TargetLayer;
            
        return Utils.GetTargetsNonAllocSingle(attacker, SearchType.Circle, 100, Vector2.zero, 1, attacker.TargetLayer, GetPriorityFilter());        
    }

#if UNITY_EDITOR
    public override void DrawGizmos(IAttacker attacker)
    {
        base.DrawGizmos(attacker);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(attacker.GetCenter(), _range);
    }
#endif
}