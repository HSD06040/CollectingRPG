using UnityEngine;

public class InGameManager : InGameSingleton<InGameManager>
{
    public Property<int> Silver = new();
    public Property<int> Energy = new();
    public bool IsBattle = false;
    public bool IsOneBattle = false;
    public int SpawnEnergy = 20;
    [SerializeField] int _startingSilver = 50;
    [SerializeField] int _startingEnergy = 100;

    #region Life Cycle
    private void OnEnable()
    {
        BattleManager.OnBattleStarted += BattleStart;
        BattleManager.OnGameStanby += BattleEnded;
    }

    private void OnDisable()
    {
        BattleManager.OnBattleStarted -= BattleStart;
        BattleManager.OnGameStanby -= BattleEnded;
    }

    private void Start()
    {
        Silver.Value = _startingSilver;
        Energy.Value = _startingEnergy;
    }
    #endregion

    public void AddSilver(int amount)
    {
        Silver.Value += amount;
    }
    public void AddEnergy(int amount)
    {
        Energy.Value += amount;
    }

    /// <summary>
    /// 전투 종료 후 골드 수치 추가
    /// </summary>
    /// <param name="amount"></param>
    public void AddGoldWithRate(int amount)
    {
        for (int i = 0; i < AugmentManager.Instance.currentAugment.Count; i++)
        {
            if (AugmentManager.Instance.currentAugment == null) return;
            if (AugmentManager.Instance.currentAugment[i].EffectType != EffectType.Currency) return;

            Silver.Value += (int)(amount * (1 + (AugmentManager.Instance.currentAugment[i].currentRate) / 100));
        }
    }

    public bool SpendGold(int amount)
    {
        if (Silver.Value >= amount)
        {
            Silver.Value -= amount;
            return true;
        }
        return false;
    }

    public bool SpendEnergy(int amount)
    {
        if (Energy.Value >= amount)
        {
            Energy.Value -= amount;
            return true;
        }
        return false;
    }

    private void BattleStart()
    {
        IsBattle = true;
    }
    private void BattleEnded()
    {
        IsBattle = false;
    }
}