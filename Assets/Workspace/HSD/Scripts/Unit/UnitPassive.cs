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

    public UnitPassive(SynergyEffect effect, UnitStatusController owner, int mulriplier = 1)
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
                EffectActive();
                break;

            case TriggerType.OnAttack:
                _owner.OnAttack += EffectActive;
                break;
            case TriggerType.OnUseSkill:
                _owner.OnSkill += EffectActive;
                break;
            case TriggerType.OnInterval:
                BattleManager.OnBattleStarted += OnInterval;
                BattleManager.OnBattleEnded += () => TokenClear();
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
        TokenClear();

        // 이벤트 구독 해제
        _owner.OnAttack -= EffectActive;
        BattleManager.OnBattleStarted -= EffectActive;
        BattleManager.OnBattleEnded -= EffectActive;
    }

    private void TokenClear()
    {
        _cts?.Cancel();
        _cts?.Dispose();
        _cts = new CancellationTokenSource();
    }

    private void OnInterval()
    {
        OnIntervalEffectAsync(_cts.Token).Forget();
    }

    private async UniTask OnIntervalEffectAsync(CancellationToken token)
    {
        while (_currentActivations < _effect.MaxActivations && !token.IsCancellationRequested)
        {
            EffectAvtives();
            try
            {
                await UniTask.WaitForSeconds(_effect.Interval, cancellationToken: token);
            }
            catch (OperationCanceledException)
            {
                break;
            }
        }

        if(_effect.NextEffect != null)
        {
            while (true)
            {
                NextEffectActives();
                try
                {
                    await UniTask.WaitForSeconds(_effect.NextEffect.Interval, cancellationToken: token);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
            }
        }
    }

    private void EffectAvtives()
    {
        if(_currentActivations < _effect.MaxActivations)
        {
            EffectActive();
            AttackActive();
        }
        else
        {
            NextEffectActives();
        }

        _currentActivations++;
    }

    private void NextEffectActives()
    {
        NextEffectActive();
        NextAttackActive();
    }

    private void EffectActive()
    {
        if (!_effect.IsBuff)
            return;

        EffectTypeActive(_effect);
    }
    private void NextEffectActive()
    {
        if (!_effect.NextEffect.IsBuff)
            return;

        EffectTypeActive(_effect.NextEffect);
    }

    private void EffectTypeActive(SynergyEffect _effect)
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
                        _owner.IncreaseHealth(Mathf.RoundToInt(stat.Value * _mulriplier));
                    else if (stat.StatType == StatType.CurMana)
                        _owner.IncreaseMana(Mathf.RoundToInt(stat.Value * _mulriplier));
                    else if (stat.StatType == StatType.Shield)
                        _owner.IncreaseShield(Mathf.RoundToInt(stat.Value * _mulriplier));
                    else
                        _owner.AddStat(stat.StatType, stat.Value * _mulriplier, _effect.Key);
                }
                break;
        }
    }

    private void AttackActive()
    {
        if (!_effect.IsAttack)
            return;

        GameObject.Instantiate(_effect.Prefab, _owner.transform.position, Quaternion.identity);
    }

    private void NextAttackActive()
    {
        if (!_effect.NextEffect.IsAttack)
            return;

        GameObject.Instantiate(_effect.NextEffect.Prefab, _owner.transform.position, Quaternion.identity);
    }
}
