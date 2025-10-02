using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MagicStone_StatModifierData_HolyLight", menuName = "Data/MagicStoneData/StatModifier/HolyLight")]
public class MagicStone_StatModifier_HolyLight : MagicStoneData
{
    [SerializeField] StatType _statType;
    [SerializeField] private float _effectScale;

    public override void UseMagicStone(Vector2 pos)
    {
        // 중심 좌표
        Vector2 centerPos = Vector2.zero;

        foreach (var target in GetMultipleTargets(pos))
        {
            centerPos += (Vector2)target.transform.position;
        }
        centerPos /= GetMultipleTargets(pos).Length;

        SpawnEffect(centerPos);

        ApplyStatMultiple(GetMultipleTargets(pos), _statType);
    }

    protected override void SpawnEffect(Vector2 pos)
    {
        GameObject effect = Manager.Resources.Instantiate<GameObject>(Address, pos, true);
        effect.transform.localScale = Vector3.one * _effectScale;
        Manager.Resources.Destroy(effect, EffectDuration);
    }
}