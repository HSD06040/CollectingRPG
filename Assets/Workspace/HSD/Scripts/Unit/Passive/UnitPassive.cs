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
    private bool _isActive;

    private CancellationTokenSource _cts; // Interval 루프 중단용

    public UnitPassive(SynergyEffect effect, UnitStatusController owner, int statMulriplier = 1)
    {
        _currentActivations = 0;
        _effect = effect;
        _owner = owner;
        _mulriplier = statMulriplier;

        _cts = new CancellationTokenSource();
    }
    #region Active & Deactive
    /// <summary>
    /// 패시브 발동 시작
    /// </summary>
    public void Active()
    {
        switch (_effect.TriggerType)
        {
            case TriggerType.Base:
                EffectActives();
                break;
            case TriggerType.OnDied:
                _owner.OnDied += EffectActives;
                break;
            case TriggerType.OnAttack:
                _owner.OnAttack += EffectActives;
                break;
            case TriggerType.OnUseSkill:
                _owner.OnSkill += EffectActives;
                break;
            case TriggerType.OnInterval:
                BattleManager.OnBattleStarted += OnInterval;
                BattleManager.OnBattleEnded += TokenClear;
                break;

            case TriggerType.OnBattleStart:
                BattleManager.OnBattleStarted += EffectActives;
                break;  
                
            case TriggerType.OnBattleEnded:
                BattleManager.OnBattleEnded += EffectActives;
                break;
        }
    }

    /// <summary>
    /// 패시브 비활성화 및 정리
    /// </summary>
    public void Deactive()
    {
        TokenClear();
        _isActive = false;

        switch (_effect.TriggerType)
        {
            case TriggerType.OnAttack:
                _owner.OnAttack -= EffectActives;
                break;
            case TriggerType.OnDied:
                _owner.OnDied -= EffectActives;
                break;
            case TriggerType.OnUseSkill:
                _owner.OnSkill -= EffectActives;
                break;
            case TriggerType.OnInterval:
                BattleManager.OnBattleStarted -= OnInterval;
                BattleManager.OnBattleEnded -= TokenClear;
                break;

            case TriggerType.OnBattleStart:
                BattleManager.OnBattleStarted -= EffectActives;
                break;

            case TriggerType.OnBattleEnded:
                BattleManager.OnBattleEnded -= EffectActives;
                break;
        }
    }
    #endregion

    private void TokenClear()
    {
        _cts?.Cancel();
        _cts?.Dispose();
        _cts = new CancellationTokenSource();
    }

    #region Interval
    private void OnInterval()
    {
        if (_effect.IsDelay)
            OnDelayEffectAsync(_cts.Token).Forget();
        else
            OnIntervalEffectAsync(_cts.Token).Forget();
    }

    /// <summary>
    /// 일반적인 Interval 효과 (MaxActivations 만큼 실행)
    /// </summary>
    private async UniTask OnIntervalEffectAsync(CancellationToken token)
    {
        while (_currentActivations < _effect.MaxActivations && !token.IsCancellationRequested)
        {
            EffectActives();
            _currentActivations++;

            try
            {
                await UniTask.WaitForSeconds(_effect.Interval, cancellationToken: token);
            }
            catch (OperationCanceledException)
            {
                return;
            }
        }

        if (_effect.NextEffect != null)
        {
            await RunNextEffectAsync(token);
        }
    }

    /// <summary>
    /// Delay 효과 (첫 효과 후 Delay → NextEffect)
    /// </summary>
    private async UniTask OnDelayEffectAsync(CancellationToken token)
    {
        while (_currentActivations < _effect.MaxActivations && !token.IsCancellationRequested)
        {
            EffectActives();
            _currentActivations++;

            try
            {
                await UniTask.WaitForSeconds(_effect.Interval, cancellationToken: token);
            }
            catch (OperationCanceledException)
            {
                return;
            }
        }

        // Delay 대기
        try
        {
            await UniTask.WaitForSeconds(_effect.DelayTime, cancellationToken: token);
        }
        catch (OperationCanceledException)
        {
            return;
        }

        // NextEffect 실행
        if (_effect.NextEffect != null)
        {
            await RunNextEffectAsync(token);
        }
    }

    /// <summary>
    /// NextEffect 실행 로직 (Interval 여부에 따라 단발/주기)
    /// </summary>
    private async UniTask RunNextEffectAsync(CancellationToken token)
    {
        if (_effect.NextEffect.Interval > 0)
        {
            while (!token.IsCancellationRequested)
            {
                NextEffectActives();

                try
                {
                    await UniTask.WaitForSeconds(_effect.NextEffect.Interval, cancellationToken: token);
                }
                catch (OperationCanceledException)
                {
                    return;
                }
            }
        }
        else
        {
            NextEffectActives();
        }
    }
    #endregion

    #region EffectActives

    private void EffectActives()
    {
        if (_effect.IsFirstOnly && _isActive)
            return;

        _isActive = true;

        if (_currentActivations < _effect.MaxActivations)
        {
            SpawnActive();
            BuffEffectActive();
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
        if (_effect.NextEffect == null) return;

        NextBuffEffectActive();
        NextAttackActive();
    }

    #region BuffEffect
    private void BuffEffectActive()
    {
        if (!_effect.IsBuff)
            return;

        BuffEffectActive(_effect);
    }
    private void NextBuffEffectActive()
    {
        if (!_effect.NextEffect.IsBuff)
            return;

        BuffEffectActive(_effect.NextEffect);
    }
    private void BuffEffectActive(SynergyEffect _effect)
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
    #endregion

    #region AttackEffect
    private void AttackActive()
    {
        if (!_effect.IsAttack)
            return;

        GameObject.Instantiate(_effect.AttackPrefab, _owner.transform.position, Quaternion.identity);
    }
    private void NextAttackActive()
    {
        if (!_effect.NextEffect.IsAttack)
            return;

        GameObject.Instantiate(_effect.NextEffect.AttackPrefab, _owner.transform.position, Quaternion.identity);
    }
    #endregion

    #region SpawnEffect
    private void SpawnActive()
    {
        if (!_effect.IsSpawn)
            return;

        Vector3 pos = _owner.transform.position;
        Spawn(pos);
    }

    private void Spawn(Vector3 pos)
    {
        GameObject obj = GameObject.Instantiate(_effect.SpawnPrefab, pos, Quaternion.identity);

        UnitBase spawnUnit = ComponentProvider.Get<UnitBase>(obj);

        if (_effect.IsMultiplier)
        {
            if (spawnUnit == null)
                return;

            if (_effect.SpawnType == SpawnStatType.Level)
            {
                if (_effect.IsMultiplier)
                {
                    spawnUnit.StatusController.StatMultiplier = _effect.UnitStatMultiplier;
                    spawnUnit.Init(_effect.SpawnUnitStats);
                }
                else
                {
                    spawnUnit.Init();
                }
            }
            else if (_effect.SpawnType == SpawnStatType.LowUpgrade)
            {
                spawnUnit.Status.Level = _owner.Status.Level - 1 >= 0 ? _owner.Status.Level - 1 : 0;
                spawnUnit.Init();
            }
        }
    }
    #endregion
#endregion
}
