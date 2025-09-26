using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

public class EffectController
{
    private readonly EffectAddressData _addressData;
    private readonly Transform _transform;

    public EffectController(Transform transform)
    {
        _transform = transform;
        _addressData = Manager.Resources.Load<EffectAddressData>("Data/EffectAddressData");
    }

    public void AddBuffEffect(BuffEffect buffEffect, float duration = 2)
    {
        if (!InGameManager.Instance.IsBattle)
            return;

        string address = _addressData.GetBuffAddress(buffEffect);

        if (string.IsNullOrEmpty(address))
        {
            Debug.LogError($"BuffEffect {buffEffect}에 대한 주소가 없음!");
            return;
        }

        SpawnEffect(address, duration, GetSpawnPos(buffEffect));
    }

    private void SpawnEffect(string address, float duration, Vector3 spawnPos)
    {
        Manager.Resources.Destroy(
            Manager.Resources.Instantiate<GameObject>(
                address, spawnPos, Quaternion.identity, _transform, true
                ),
            duration
            );
    }

    private Vector3 GetSpawnPos(BuffEffect buffEffect)
    {
        return (buffEffect) switch
        {
            BuffEffect.Buff => _transform.position,
            BuffEffect.Stun => _transform.GetTopPosition(),
            BuffEffect.Debuff => _transform.GetTopPosition(),
            BuffEffect.Damage => _transform.GetCenterPosition(),
            BuffEffect.Heal => _transform.GetCenterPosition(),
            BuffEffect.Shield => _transform.GetCenterPosition(),
            BuffEffect.AttackSpeed => _transform.GetCenterPosition(),
            BuffEffect.Defense => _transform.GetCenterPosition(),
        };
    }
}
