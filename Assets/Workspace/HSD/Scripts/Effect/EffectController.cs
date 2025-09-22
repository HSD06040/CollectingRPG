using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

public class EffectController : IDisposable
{
    private readonly EffectAddressData _addressData;
    private readonly Transform _transform;
    private readonly CancellationTokenSource _cts;

    public EffectController(Transform transform, CancellationTokenSource cts)
    {
        _cts = cts;
        _transform = transform;
        _addressData = Manager.Resources.Load<EffectAddressData>("Data/EffectAddressData");
    }

    public void AddBuffEffect(BuffEffect buffEffect, float duration = 2)
    {
        string address = _addressData.GetBuffAddress(buffEffect);

        if (string.IsNullOrEmpty(address))
        {
            Debug.LogError($"BuffEffect {buffEffect}에 대한 주소가 없음!");
            return;
        }

        SpawnEffect(address, duration, GetSpawnPos(buffEffect), _cts.Token).Forget();
    }

    private async UniTask SpawnEffect(string address, float duration, Vector3 spawnPos,CancellationToken token)
    {
        GameObject effectPrefab = Manager.Resources.Load<GameObject>(address);
        GameObject effect = Manager.Resources.Instantiate(effectPrefab, spawnPos, effectPrefab.transform.rotation, _transform, true);

        try
        {
            await UniTask.WaitForSeconds(duration, cancellationToken: token);
        }
        catch (OperationCanceledException)
        {
            // 정상적인 취소 → 무시
        }
        finally
        {
            if (effect != null)
                Manager.Resources.Destroy(effect);
        }

        Manager.Resources.Destroy(effect);
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

    public void Dispose()
    {
        _cts.Cancel();
        _cts.Dispose();
    }
}
