using Cysharp.Threading.Tasks;
using Firebase.Auth;
using System;
using System.Threading;
using UnityEngine;

/// <summary>
/// 전장 전체(플레이어 전체/전투 전체)에 적용되는 패시브
/// </summary>
public class GlobalPassive
{
    private SynergyEffect _effect;
    private int _currentActivations;
    private int _mulriplier;

    private CancellationTokenSource _cts;
    private Transform _center;
    private UnitBase[] _units;

    public GlobalPassive(SynergyEffect effect, UnitBase[] units, Transform center, int mulriplier = 1)
    {
        _currentActivations = 0;
        _effect = effect;
        _units = units;
        _center = center;
        _mulriplier = mulriplier;

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
                EffectActive();
                break;
            case TriggerType.OnDied:
                UnitsDiedEventSubscribe();
                break;
            case TriggerType.OnInterval:
                BattleManager.OnBattleStarted += OnInterval;
                BattleManager.OnBattleEnded += TokenClear;
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

        switch (_effect.TriggerType)
        {            
            case TriggerType.OnDied:
                OnUnitDieEventUnSubscribe();
                break;
            case TriggerType.OnInterval:
                BattleManager.OnBattleStarted -= OnInterval;
                BattleManager.OnBattleEnded -= TokenClear;
                break;

            case TriggerType.OnBattleStart:
                BattleManager.OnBattleStarted -= EffectActive;
                break;

            case TriggerType.OnBattleEnded:
                BattleManager.OnBattleEnded -= EffectActive;
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
        OnIntervalEffectAsync(_cts.Token).Forget();
    }

    private async UniTask OnIntervalEffectAsync(CancellationToken token)
    {
        while (_currentActivations < _effect.MaxActivations && !token.IsCancellationRequested)
        {
            EffectActives();
            try
            {
                await UniTask.WaitForSeconds(_effect.Interval, cancellationToken: token);
            }
            catch (OperationCanceledException)
            {
                break;
            }
        }

        if (_effect.NextEffect != null)
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
    #endregion

    #region DiedEventSubscribe
    private void UnitsDiedEventSubscribe()
    {
        if(_effect.IsUnitPosition)
        {
            foreach (var unit in _units)
            {
                unit.StatusController.OnUnitDied += EffectActives;
            }
        }    
        else
        {
            foreach (var unit in _units)
            {
                unit.StatusController.OnDied += EffectActives;
            }
        }
    }

    private void OnUnitDieEventUnSubscribe()
    {
        if (_effect.IsUnitPosition)
        {
            foreach (var unit in _units)
            {
                unit.StatusController.OnUnitDied -= EffectActives;
            }
        }
        else
        {
            foreach (var unit in _units)
            {
                unit.StatusController.OnDied -= EffectActives;
            }
        }        
    }
    #endregion

#region EffectActives
    private void EffectActives()
    {
        if (_currentActivations < _effect.MaxActivations)
        {
            SpawnActive();
            EffectActive();
            AttackActive();
        }
        else
        {            
            NextEffectActives();
        }

        _currentActivations++;
    }

    private void EffectActives(UnitStatusController unit)
    {
        if (_currentActivations < _effect.MaxActivations)
        {
            SpawnActive(unit);
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
        if (_effect.NextEffect == null) return;

        NextEffectActive();
        NextAttackActive();
    }

    #region BuffEffect
    private void EffectActive()
    {
        if (!_effect.IsBuff)
            return;

        BuffEffectActive(_effect);
    }

    private void NextEffectActive()
    {
        if (!_effect.NextEffect.IsBuff)
            return;

        BuffEffectActive(_effect.NextEffect);
    }

    /// <summary>
    /// 글로벌 패시브는 유닛이 아니라 전장 전체에 적용
    /// </summary>
    private void BuffEffectActive(SynergyEffect effect)
    {
        switch (effect.EffectType)
        {
            case EffectType.Buff_Debuff:
                foreach (var unit in _units) // 전장 유닛 전체에게 적용
                {
                    for (int i = 0; i < effect.SynergyBuffDatas.Length; i++)
                    {
                        SynergyBuffData data = effect.SynergyBuffDatas[i];
                        unit.StatusController.ApplyEffect(
                            new BuffEffectData { StatType = data.StatType, Duration = data.Duration },
                            data.Value * _mulriplier, effect.Key);
                    }
                }
                break;

            case EffectType.Increase:
                foreach (var unit in _units)
                {
                    foreach (var stat in effect.StatModifiers)
                    {
                        if (stat.StatType == StatType.CurHp)
                            unit.StatusController.IncreaseHealth(Mathf.RoundToInt(stat.Value * _mulriplier));
                        else if (stat.StatType == StatType.CurMana)
                            unit.StatusController.IncreaseMana(Mathf.RoundToInt(stat.Value * _mulriplier));
                        else if (stat.StatType == StatType.Shield)
                            unit.StatusController.IncreaseShield(Mathf.RoundToInt(stat.Value * _mulriplier));
                        else
                            unit.StatusController.AddStat(stat.StatType, stat.Value * _mulriplier, effect.Key);
                    }
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

        // 글로벌 공격은 전장 중앙이나 특정 지점에 소환하는 식으로 처리
        Vector3 pos = _center.position;
        GameObject.Instantiate(_effect.Prefab, pos, Quaternion.identity);
    }

    private void NextAttackActive()
    {
        if (!_effect.NextEffect.IsAttack)
            return;

        Vector3 pos = _center.position;
        GameObject.Instantiate(_effect.NextEffect.Prefab, pos, Quaternion.identity);
    }
    #endregion

    private void SpawnActive()
    {
        if (!_effect.IsSpawn)
            return;

        Vector3 pos = _center.position;
        Spawn(pos);
    }

    private void SpawnActive(UnitStatusController unit)
    {
        if (!_effect.IsSpawn)
            return;

        Spawn(unit.transform.position);
    }

    private void Spawn(Vector3 pos)
    {
        GameObject obj = GameObject.Instantiate(_effect.SpawnPrefab, pos, Quaternion.identity);

        if (_effect.IsMultiplier)
        {
            if (_effect.SpawnType == SpawnType.Unit)
            {
                // 나중에 아마 소환진을 스폰할 예정 (이펙트 리소스가 기획측에서 들어오면 수정 예정)
                UnitBase unit = obj.GetComponent<UnitBase>();

                if (unit != null)
                {
                    if (_effect.IsMultiplier)
                    {
                        unit.StatusController.StatMultiplier = _units.GetSynergyUnitsTotalLevel(_effect.SpawnSynergy);
                        unit.Init(_effect.SpawnUnitStats);
                    }
                    else
                    {
                        unit.Init();
                    }
                }
            }
            else if (_effect.SpawnType == SpawnType.AttackObject)
            {

            }
        }
    }
    #endregion
}
