using UnityEngine;

public class InGameManager : InGameSingleton<InGameManager>
{
    public Property<int> Gold = new();
    public bool IsBattle = false;
    public int SpawnGold = 20;
    [SerializeField] int _startingGold = 100;

    #region Life Cycle
    private void OnEnable()
    {
        BattleManager.OnBattleStarted += BattleStart;
    }

    private void OnDisable()
    {
        BattleManager.OnBattleStarted -= BattleStart;
    }

    private void Start()
    {
        Gold.Value = _startingGold;
    }
    #endregion

    public void AddGold(int amount)
    {
        Gold.Value += amount;
    }

    /// <summary>
    /// 전투 종료 후 골드 수치 추가
    /// </summary>
    /// <param name="amount"></param>
    public void AddGoldWithRate(int amount)
    {
        if (AugmentManager.Instance.currentAugment == null) return;
        if (AugmentManager.Instance.currentAugment.EffectType != EffectType.Currency) return;

        Gold.Value += (int)(amount * (1 + (AugmentManager.Instance.currentAugment.currentRate) / 100));
    }

    public bool SpendGold(int amount)
    {
        if (Gold.Value >= amount)
        {
            Gold.Value -= amount;
            return true;
        }
        return false;
    }

    private void BattleStart()
    {
        IsBattle = true;
    }
}