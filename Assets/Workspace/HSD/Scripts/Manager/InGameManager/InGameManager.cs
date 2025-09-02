using System;
using UnityEngine;

public class InGameManager : MonoBehaviour
{
    public Property<int> Gold = new(); 
    public bool IsBattle = false;
    public int SpawnGold = 20;
    [SerializeField] int _startingGold = 100;

    #region Singleton Pattern
    private static InGameManager instance;
    public static InGameManager Instance 
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<InGameManager>();
            }

            return instance;
        }
        private set => instance = value; 
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
    }
    #endregion

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
