using UnityEngine;

[CreateAssetMenu(fileName = "MagicStone_Attack_MeteorStrike", menuName = "Data/MagicStoneData/Attack/MeteorStrike")]
public class MagicStone_Attack_MeteorStrike : MagicStoneData
{
    [SerializeField] private float _meteorSpeed;
    [SerializeField] private string _meteorAddress; 

    [SerializeField] private float _spawnOffsetX;
    [SerializeField] private float _spawnOffsetY;

    public override void UseMagicStone(Vector2 pos)
    {
        // 적 위치 평균 좌표 -> 중앙값
        Vector2 targetPos = Vector2.zero;

        foreach (var target in GetMultipleTargets(pos))
        {
            targetPos += (Vector2)target.transform.position;
        }
        targetPos = targetPos / GetMultipleTargets(pos).Length;

        // 운석 생성
        Vector2 camTopLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, Camera.main.nearClipPlane));
        Vector2 spawnPos = camTopLeft + new Vector2(_spawnOffsetX, _spawnOffsetY);

        GameObject meteor = Manager.Resources.Instantiate<GameObject>(_meteorAddress, spawnPos);
        
        MeteorBehaviour mb = meteor.GetComponent<MeteorBehaviour>();

        mb.Init(targetPos, _meteorSpeed, (targetPos) =>
        {
            foreach (var target in GetMultipleTargets(pos))
            {
                if (target == null) continue;
                SpawnEffect(target.transform.position);
            }

            ApplyTakeDamageMultiple(GetMultipleTargets(pos));
        });
    }

    protected override void SpawnEffect(Vector2 pos)
    {
        GameObject effect = Manager.Resources.Instantiate<GameObject>(Address, pos, true);
        if (effect == null) return;
        Manager.Resources.Destroy(effect, EffectDuration);
    }
}
