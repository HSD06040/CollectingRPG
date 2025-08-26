using UnityEngine;

public class InGameManager : MonoBehaviour
{
    public Property<int> Gold = new(); 
    [SerializeField] int _startingGold = 100;

    #region Singleton Pattern
    public static InGameManager Instance { get; private set; }

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

    private void Start()
    {
        Gold.Value = _startingGold;
    }

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
}
