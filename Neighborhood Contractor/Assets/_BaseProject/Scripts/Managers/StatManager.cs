using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Holds all data about this game.
/// </summary>
[RequireComponent(typeof(GameManager))]
public class StatManager : MonoBehaviour
{
    private GameManager gameManager;
    public GameManager GameManager => gameManager == null ? gameManager = GetComponent<GameManager>() : gameManager;

    public static int CarryCapacity, CurrentCarry, MoneyValue, SpendValue;
    public static float SpendRate;
    public static List<Money> CollectedMoney;

    public int TotalMoney { get; private set; }
    public int RewardMoney { get; private set; }
    public static int MoneyCapacity => CarryCapacity * MoneyValue;

    private void Init()
    {
        // Default Stats
        CurrentCarry = 0;
        CarryCapacity = 20;
        MoneyValue = SpendValue = 100;
        SpendRate = 1f;

        CollectedMoney = new List<Money>();
        CollectedMoney.Clear();

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
        PlayerEvents.OnSpendMoney += HandleSpendMoney;
        GameEvents.OnCalculateReward += CalculateReward;
    }

    private void OnDisable()
    {
        PlayerEvents.OnCollectedMoney -= HandleCollectMoney;
        PlayerEvents.OnSpendMoney -= HandleSpendMoney;
        GameEvents.OnCalculateReward -= CalculateReward;
    }

    private void HandleCollectMoney()
    {
        CurrentCarry++;
        IncreaseTotalMoney(MoneyValue);
        CollectableEvents.OnIncreaseMoney?.Invoke(MoneyValue);
    }

    private void HandleSpendMoney()
    {
        CurrentCarry--;
        DecreaseTotalMoney(MoneyValue);
        CollectableEvents.OnDecreaseMoney?.Invoke(MoneyValue);
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