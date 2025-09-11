using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitHealthBar : MonoBehaviour
{
    [SerializeField] Slider _slider;
    private UnitBase _owner;

    public void Setup(UnitBase owner)
    {
        if (_owner != null)
            Dispose();

        _owner = owner;
        _slider.maxValue = owner.StatusController.MaxHealth.Value;
        _slider.value = owner.StatusController.MaxHealth.Value;

        Subcribe();
        ChaseOwner().Forget();
    }

    private async UniTask ChaseOwner()
    {
        while (_owner != null)
        {
            transform.position = _owner.GetBarPosition();
            await UniTask.Yield(PlayerLoopTiming.Update);
        }
    }

    private void Subcribe()
    {
        _owner.StatusController.CurHp.AddEvent(UpdateValue);
        _owner.StatusController.OnDied += BarDestroy;
    }

    private void Dispose()
    {
        _owner.StatusController.CurHp.RemoveEvent(UpdateValue);
        _owner.StatusController.OnDied -= BarDestroy;
    }

    private void UpdateValue(int value)
    {
        _slider.value = value;
    }

    private void BarDestroy()
    {
        Manager.Resources.Destroy(gameObject);
    }
}
