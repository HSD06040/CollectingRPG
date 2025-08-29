using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class SynergyEffectManager : MonoBehaviour
{
    #region Singleton
    private static SynergyEffectManager instance;
    public static SynergyEffectManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SynergyEffectManager>();
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
    #endregion

    private class RunningEffect
    {
        public SynergyEffect Effect;
        public CancellationTokenSource Cts;
        public Action BattleStartAction;

        public RunningEffect(SynergyEffect effect, Action battleStartAction)
        {
            Effect = effect;
            Cts = new CancellationTokenSource();
            BattleStartAction = battleStartAction;
        }
    }

    private Dictionary<string, RunningEffect> _effects = new Dictionary<string, RunningEffect>(64);

    /// <summary>
    /// 이펙트를 전투 시작 이벤트에 등록 (게임 시작 전 세팅 단계에서 호출)
    /// </summary>
    public void AddEffect(SynergyEffect effect)
    {
        if (_effects.ContainsKey(effect.Key))
            return; // 중복 방지

        // 전투 시작 시 실행할 동작 정의
        Action action = () =>
        {
            if (_effects.TryGetValue(effect.Key, out var running))
            {
                running.Cts.Cancel();
                running.Cts = new CancellationTokenSource();
                AttackSpawn(effect, running.Cts.Token).Forget();
            }
        };

        // 이벤트에 등록
        BattleManager.OnBattleStarted += action;

        _effects[effect.Key] = new RunningEffect(effect, action);
    }
    /// <summary>
    /// 특정 이펙트 제거
    /// </summary>
    public void RemoveEffect(SynergyEffect effect)
    {
        string key = effect.Key;

        if (_effects.TryGetValue(key, out var running))
        {
            // 실행 중인 Task 취소
            running.Cts.Cancel();

            // 이벤트 해제
            BattleManager.OnBattleStarted -= running.BattleStartAction;

            // Dictionary에서 제거
            _effects.Remove(key);

            Debug.Log($"Effect {key} removed");
        }
    }

    /// <summary>
    /// 모든 이펙트 취소 (게임 종료 시 호출)
    /// </summary>
    public void CancelAllEffects()
    {
        foreach (var kvp in _effects)
        {
            kvp.Value.Cts.Cancel();
            BattleManager.OnBattleStarted -= kvp.Value.BattleStartAction;
        }
        _effects.Clear();
    }

    private async UniTask AttackSpawn(SynergyEffect effect, CancellationToken token)
    {
        try
        {
            while (true)
            {
                await UniTask.WaitForSeconds(effect.Interval, cancellationToken: token);

                // TODO: 실제 공격/효과 실행
                Debug.Log($"Effect {effect.Key} triggered");
            }
        }
        catch (OperationCanceledException)
        {
            Debug.Log($"Effect {effect.Key} canceled");
        }
    }
}
