using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[CreateAssetMenu(fileName = "MagicStone_Attack_ShadowSpear", menuName = "Data/MagicStoneData/Attack/ShadowSpear")]
public class MagicStone_Attack_ShadowSpear : MagicStoneData
{
    [SerializeField] private float _spearSpeed;
    [SerializeField] private string _spearAddress;  
    [SerializeField] private float _spawnOffsetX;  
    [SerializeField] private float _spawnOffsetY;  

    public override void UseMagicStone(Vector2 pos)
    {
        Vector2 camTopLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 0));
        Vector2 spawnPos = camTopLeft + new Vector2(_spawnOffsetX, _spawnOffsetY);

        GameObject spear = Manager.Resources.Instantiate<GameObject>(_spearAddress, spawnPos, true);
        SpearBehaviour sb = spear.GetComponent<SpearBehaviour>();

        UnitBase singleTarget = GetSingleTarget(pos);

        sb.Init(pos, _spearSpeed, (pos) =>
        {
            UnitBase target = GetSingleTarget(pos); 

            if (singleTarget == null)
            {
                return;
            }

            SpawnEffect(GetSingleTarget(pos).transform.position);

            ApplyTakeDamageSingle(GetSingleTarget(pos));
        });
    }

    protected override void SpawnEffect(Vector2 pos)
    {
        GameObject effect = Manager.Resources.Instantiate<GameObject>(Address, pos, true);
        if (effect == null) return;

        Manager.Resources.Destroy(effect, EffectDuration);
    }
}
