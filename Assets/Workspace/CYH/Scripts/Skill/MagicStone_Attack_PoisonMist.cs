using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MagicStone_Attack_PoisonMist", menuName = "Data/MagicStoneData/Attack/PoisonMist")]
public class MagicStone_Attack_PoisonMist : MagicStoneData
{
    [SerializeField] private TickDamageData _tickDamageData;

    public override void UseMagicStone(Vector2 pos)
    {
        Vector2 centerPos = Vector2.zero;

        foreach (var target in GetMultipleTargets(pos))
        {
            centerPos += (Vector2)target.transform.position;
        }
        centerPos /= GetMultipleTargets(pos).Length;

        SpawnEffect(centerPos);

        foreach (var obj in GetMultipleTargets(pos))
        {
            UnitBase unit = ComponentProvider.Get<UnitBase>(obj);
            if (unit != null)
            {
                ApplyTickDamageSingle(unit, _tickDamageData);
                Debug.Log(
                    $"PoisonMist 대상: {unit.name}, " +
                    $"틱당 피해량: {(int)Value}, " +
                    $"틱 수: {_tickDamageData.TickCount}, " +
                    $"틱 간격: {_tickDamageData.TickInterval}초"
                );
            }
        }
    }

    protected override void SpawnEffect(Vector2 pos)
    {
        if (string.IsNullOrEmpty(Address))
            return;

        GameObject effect = Manager.Resources.Instantiate<GameObject>(Address, pos, true);

        if (effect == null)
            return;

        Manager.Resources.Destroy(effect, EffectDuration);
    }
}