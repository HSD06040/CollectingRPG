using UnityEngine;

[CreateAssetMenu(fileName = "MagicStone_Contrl_HammerOfTime", menuName = "Data/MagicStoneData/Control/HammerOfTime")]
public class MagicStone_Contrl_HammerOfTime : MagicStoneData
{
    [SerializeField] private string _hammerEffectAddress;
    [SerializeField] private bool _swingLeft = true;

    [SerializeField] private float _hammerSpawnX;
    [SerializeField] private float _hammerSpawnY;
    [SerializeField] private float _stunEffectOffsetY;

    public override void UseMagicStone(Vector2 pos)
    {
        Vector2 spawnPos = new Vector2(_hammerSpawnX, _hammerSpawnY);

        GameObject hammer = SpawnHammerEffect(spawnPos, _hammerEffectAddress);
        HammerEvent hammerEffect = hammer.GetComponent<HammerEvent>();

        if (_swingLeft)
        {
            hammer.transform.rotation = Quaternion.Euler(0, 180, 0);
        }

        if (hammerEffect != null)
        {
            hammerEffect.OnHammerDestroyed = () =>
            {
                ApplyStunMultiple(GetMultipleTargets(pos));

                foreach (var enemy in GetMultipleTargets(pos))
                {
                    if (enemy == null) continue;
                    Vector2 headPos = (Vector2)enemy.transform.position + Vector2.up * _stunEffectOffsetY;
                    SpawnStunEffect(headPos, Address);
                }
            };
        }
    }

    private GameObject SpawnHammerEffect(Vector2 pos, string address)
    {
        if (string.IsNullOrEmpty(Address))
            return null;

        GameObject effect = Manager.Resources.Instantiate<GameObject>(address, pos, true);

        if (effect == null) 
            return null;

        return effect;
    }

    private void SpawnStunEffect(Vector2 pos, string address)
    {
        if (string.IsNullOrEmpty(Address))
            return;

        GameObject effect = Manager.Resources.Instantiate<GameObject>(address, pos, true);

        if (effect == null) return;

        Manager.Resources.Destroy(effect, EffectDuration);
    }
}