using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

public class UnitPassive
{
    private SynergyEffect _effect;
    private UnitStatusController _owner;
    private int _currentActivations;
    private int _mulriplier;

    private CancellationTokenSource _cts; // Interval 루프 중단용

    public UnitPassive(SynergyEffect effect, UnitStatusController owner, int mulriplier)
    {
        _currentActivations = 0;
        _effect = effect;
        _owner = owner;
        _mulriplier = mulriplier;

        _cts = new CancellationTokenSource();
    }

    /// <summary>
    /// 패시브 발동 시작
    /// </summary>
    public void Active()
    {
        switch (_effect.TriggerType)
        {
            case TriggerType.Base:
                EffectTypeActive();
                break;

            case TriggerType.OnAttack:
                _owner.OnAttack += EffectActive;
                break;

            case TriggerType.OnInterval:
                OnInterval(_cts.Token).Forget();
                break;

            case TriggerType.OnBattleStart:
                BattleManager.OnBattleStarted += EffectActive;               
                break;  
                
            case TriggerType.OnBattleEnded:
                BattleManager.OnBattleEnded += EffectActive;
                break;
        }
    }

    /// <summary>
    /// 패시브 비활성화 및 정리
    /// </summary>
    public void Deactive()
    {
        _cts?.Cancel();
        _cts?.Dispose();
        _cts = new CancellationTokenSource();

        // 이벤트 구독 해제
        _owner.OnAttack -= EffectActive;
        BattleManager.OnBattleStarted -= EffectActive;
        BattleManager.OnBattleEnded -= EffectActive;
    }

    private async UniTask OnInterval(CancellationToken token)
    {
        while (_currentActivations < _effect.MaxActivations && !token.IsCancellationRequested)
        {
            _currentActivations++;
            EffectTypeActive();
            try
            {
                await UniTask.WaitForSeconds(_effect.Interval, cancellationToken: token);
            }
            catch (OperationCanceledException)
            {
                break;
            }
        }
    }

    private void EffectActive()
    {
        if (_currentActivations >= _effect.MaxActivations) return;

        EffectTypeActive();
    }

    private void EffectTypeActive()
    {
        switch (_effect.EffectType)
        {
            case EffectType.Buff_Debuff:
                for (int i = 0; i < _effect.SynergyBuffDatas.Length; i++)
                {
                    SynergyBuffData data = _effect.SynergyBuffDatas[i];
                    _owner.ApplyEffect(
                        new BuffEffectData { StatType = data.StatType, Duration = data.Duration },
                        data.Value * _mulriplier, _effect.Key);
                }
                break;

            case EffectType.Increase:
                foreach (var stat in _effect.StatModifiers)
                {
                    if (stat.StatType == StatType.CurHp)
                        _owner.IncreaseHealth(stat.Value * _mulriplier);
                    else if (stat.StatType == StatType.CurMana)
                        _owner.IncreaseMana(stat.Value * _mulriplier);
                    else
                        _owner.AddStat(stat.StatType, stat.Value * _mulriplier, _effect.Key);
                }
                break;
        }
    }
}
