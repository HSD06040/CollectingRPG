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
        _addressData = Manager.Resources.Get<EffectAddressData>("Data/EffectAddressData");
    }

    public void AddBuffEffect(BuffEffect buffEffect, float duration = 2)
    {
        string address = _addressData.GetBuffAddress(buffEffect);
        if (string.IsNullOrEmpty(address))
        {
            Debug.LogError($"BuffEffect {buffEffect}에 대한 주소가 없음!");
            return;
        }

        SpawnEffect(address, duration, _cts.Token).Forget();
    }

    private async UniTask SpawnEffect(string address, float duration, CancellationToken token)
    {
        GameObject effect = Manager.Resources.Instantiate<GameObject>(address, _transform.position, true);

        try
        {
            await UniTask.Delay(TimeSpan.FromSeconds(duration), cancellationToken: token);
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

    public void Dispose()
    {
        _cts.Cancel();
        _cts.Dispose();
    }
}
