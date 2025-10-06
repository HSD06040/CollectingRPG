using UnityEngine;

[CreateAssetMenu(fileName = "MagicStone_Contrl_RootOfForest", menuName = "Data/MagicStoneData/Control/RootOfForest")]

public class MagicStone_Control_RootOfForest : MagicStoneData
{
    [SerializeField] private string _effectAddress;

    [SerializeField] private float _effectSpawnX;
    [SerializeField] private float _effectSpawnY;

    public override void UseMagicStone(Vector2 pos)
    {
        Vector2 spawnPos = new Vector2(_effectSpawnX, _effectSpawnY);

        SpawnEffect(spawnPos, _effectAddress);
        ApplyStunMultiple(GetMultipleTargets(pos));

        foreach (var enemy in GetMultipleTargets(pos))
        {
            if (enemy == null) continue;
            Vector2 centerPos = enemy.transform.position;
            SpawnEffect(centerPos, Address);
        }
    }

    protected void SpawnEffect(Vector2 pos, string address)
    {
        if (string.IsNullOrEmpty(address))
            return;

        GameObject effect = Manager.Resources.Instantiate<GameObject>(address, pos, true);

        if (effect == null)
            return;

        Manager.Resources.Destroy(effect, EffectDuration);
    }
}
