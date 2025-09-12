using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class UnitHealthBar : MonoBehaviour
{
    [SerializeField] Slider _slider;
    private UnitBase _owner;
    private CancellationTokenSource _cts;

    public void Setup(UnitBase owner)
    {
        if (_owner != null)
            Dispose();

        _owner = owner;
        _slider.maxValue = owner.StatusController.MaxHealth.Value;
        _slider.value = owner.StatusController.MaxHealth.Value;

        Subcribe();

        _cts = new CancellationTokenSource();
        ChaseOwner(_cts.Token).Forget();
    }

    private async UniTask ChaseOwner(CancellationToken token)
    {
        try
        {
            while (_owner != null && !token.IsCancellationRequested)
            {
                transform.position = _owner.GetBarPosition();
                await UniTask.Yield(PlayerLoopTiming.Update, token);
            }
        }
        catch (OperationCanceledException)
        {

        }
    }

    private void Subcribe()
    {
        _owner.StatusController.CurHp.AddEvent(UpdateValue);
        _owner.StatusController.OnDied += BarDestroy;
    }

    private void Dispose()
    {
        if (_owner != null)
        {
            _owner.StatusController.CurHp.RemoveEvent(UpdateValue);
            _owner.StatusController.OnDied -= BarDestroy;
        }

        _owner = null;

        _cts?.Cancel();
        _cts?.Dispose();
        _cts = null;
    }

    private void UpdateValue(int value)
    {
        _slider.value = value;
    }

    private void BarDestroy()
    {
        Dispose();
        Manager.Resources.Destroy(gameObject);
    }
}
