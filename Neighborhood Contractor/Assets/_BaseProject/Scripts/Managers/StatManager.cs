using UnityEngine;

/// <summary>
/// Holds all data about this game.
/// </summary>
[RequireComponent(typeof(GameManager))]
public class StatManager : MonoBehaviour
{
    private GameManager gameManager;
    public GameManager GameManager => gameManager == null ? gameManager = GetComponent<GameManager>() : gameManager;

    public static int CarryCapacity, CurrentCarry, MoneyValue;

    public int TotalMoney { get; private set; }
    public int RewardMoney { get; private set; }
    public static int MoneyCapacity => CarryCapacity * MoneyValue;

    private void Init()
    {
        // Default Stats
        CurrentCarry = 0;
        CarryCapacity = 20;
        MoneyValue = 100;

        TotalMoney = PlayerPrefs.GetInt("TotalMoney", 0);
        RewardMoney = 0;
    }

    private void Awake()
    {
        Init();
    }

    private void Start()
    {
        PlayerEvents.OnCollectedMoney += HandleCollectMoney;
        GameEvents.OnCalculateReward += CalculateReward;
    }

    private void OnDisable()
    {
        PlayerEvents.OnCollectedMoney -= HandleCollectMoney;
        GameEvents.OnCalculateReward -= CalculateReward;
    }

    private void HandleCollectMoney()
    {
        CurrentCarry++;
        IncreaseTotalMoney(MoneyValue);
        CollectableEvents.OnIncreaseMoney(MoneyValue);
    }

    private void HandleSpendMoney()
    {
        CurrentCarry--;
        DecreaseTotalMoney(MoneyValue);
        CollectableEvents.OnDecreaseMoney(MoneyValue);
    }

    private void DecreaseTotalMoney(int amount) => TotalMoney -= amount;

    private void IncreaseTotalMoney(int amount)
    {
        TotalMoney += amount;
        //PlayerPrefs.SetInt("TotalCoin", TotalMoney);
        //PlayerPrefs.Save();
    }

    private void CalculateReward() => RewardMoney = 55;
}
