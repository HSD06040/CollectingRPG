using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MagicStone_StatModifierData_ShadowCloak", menuName = "Data/MagicStoneData/StatModifier/ShadowCloak")]
public class MagicStone_StatModifier_ShadowCloak : MagicStoneData
{
    [SerializeField] StatType _statType;
    [SerializeField] private float _effectScale;
    [SerializeField] private float _yOffset;
    [SerializeField] private float _xOffset;

    public override void UseMagicStone(Vector2 pos)
    {
        foreach (var target in GetMultipleTargets(pos))
        {
            SpawnEffect(target.transform.position);
        }

        ApplyStatMultiple(GetMultipleTargets(pos), _statType);
    }

    protected override void SpawnEffect(Vector2 pos)
    {
        Vector2 spawnPos = pos + new Vector2(_xOffset, _yOffset);

        GameObject effect = Manager.Resources.Instantiate<GameObject>(Address, pos, true);

        effect.transform.localScale = Vector3.one * _effectScale;
        effect.transform.position = pos + new Vector2(_xOffset, _yOffset);

        Manager.Resources.Destroy(effect, EffectDuration);
    }
}