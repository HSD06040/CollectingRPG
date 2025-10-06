using UnityEngine;

[CreateAssetMenu(fileName = "MagicStone_Contrl_VoidGravity", menuName = "Data/MagicStoneData/Control/VoidGravity")]

public class MagicStone_Control_VoidGravity : MagicStoneData
{
    public override void UseMagicStone(Vector2 pos)
    {
        Vector2 centerPos = Vector2.zero;

        foreach (var target in GetMultipleTargets(pos))
        {
            centerPos += (Vector2)target.transform.position;
        }
        centerPos = centerPos / GetMultipleTargets(pos).Length;

        SpawnEffect(centerPos);
        ApplyStunMultiple(GetMultipleTargets(pos));
    }

    protected override void SpawnEffect(Vector2 pos)
    {
        GameObject effect = Manager.Resources.Instantiate<GameObject>(Address, pos, true);
        if (effect == null) return;
        Manager.Resources.Destroy(effect, EffectDuration);
    }
}
