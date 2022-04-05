using System.Collections.Generic;
using UnityEngine;
using ZestGames.Utility;

/// <summary>
/// Holds all data about this game.
/// </summary>
[RequireComponent(typeof(GameManager))]
public class StatManager : MonoBehaviour
{
    private SavePlayerData _savePlayerData;
    private bool _deleteSaveData = false;

    private GameManager gameManager;
    public GameManager GameManager => gameManager == null ? gameManager = GetComponent<GameManager>() : gameManager;

    [Header("-- SETUP --")]
    [SerializeField] private int carryCapacity = 20;
    [SerializeField] private int moneyValue = 100;
    [SerializeField] private int spendValue = 100;
    [SerializeField] private float spendTime = 0.1f;
    [SerializeField] private float takeIncomeTime = 0.25f;

    public static int CarryCapacity, MoneyValue, SpendValue;
    public static List<Money> CollectedMoney;

    public static int CurrentCarry { get; private set; }
    public static int TotalMoney { get; private set; }
    public static float SpendTime { get; private set; }
    public static float TakeIncomeTime { get; private set; }
    public int RewardMoney { get; private set; }
    public static int MoneyCapacity => CarryCapacity * MoneyValue;

    [Header("-- FOR LOADING --")]
    [SerializeField] private GameObject money;
    [SerializeField] private Transform moneyStack;

    private void Init()
    {
        // Default Stats
        TotalMoney = CurrentCarry = 0;
        CarryCapacity = carryCapacity;
        MoneyValue = moneyValue;
        SpendValue = spendValue;

        CollectedMoney = new List<Money>();
        CollectedMoney.Clear();
        RewardMoney = 0;

        LoadStats();
        UpdatePlayerStats();

        SpendTime = spendTime;
        TakeIncomeTime = takeIncomeTime;

        Delayer.DoActionAfterDelay(this, 1f, CalculateSpendTime);
        Delayer.DoActionAfterDelay(this, 1f, CalculateIncomeTakeTime);
    }

    private void LoadStats()
    {
        _savePlayerData = LoadHandler.LoadPlayerData();

        TotalMoney = _savePlayerData.TotalMoney;
        CurrentCarry = _savePlayerData.CurrentCarry;
        SpendTime = _savePlayerData.SpendTime;
        TakeIncomeTime = _savePlayerData.TakeIncomeTime;
    }

    private void UpdatePlayerStats()
    {
        for (int i = 0; i < CurrentCarry; i++)
        {
            Money mny = Instantiate(money, Vector3.zero, Quaternion.identity, moneyStack).GetComponent<Money>();
            mny.transform.localPosition = new Vector3(0f, (i * 0.15f), 0f);
            mny.transform.localRotation = Quaternion.Euler(0f, 90f, 0f);
            mny.SetMoneyAsCollected(i);
        }
    }

    private void OnApplicationPause(bool pause)
    {
        SaveHandler.SavePlayerData(this);

        if (_deleteSaveData)
            PlayerPrefs.DeleteAll();
    }

    private void OnApplicationQuit()
    {
        SaveHandler.SavePlayerData(this);

        if (_deleteSaveData)
            PlayerPrefs.DeleteAll();
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
        ValueBarEvents.OnValueLevelIncrease += CalculateSpendTime;
        ValueBarEvents.OnValueLevelIncrease += CalculateIncomeTakeTime;

        _deleteSaveData = GameManager.Instance.DeleteSaveGame;
    }

    private void OnDisable()
    {
        PlayerEvents.OnCollectedMoney -= HandleCollectMoney;
        PlayerEvents.OnSpendMoney -= HandleSpendMoney;
        GameEvents.OnCalculateReward -= CalculateReward;
        ValueBarEvents.OnValueLevelIncrease -= CalculateSpendTime;
        ValueBarEvents.OnValueLevelIncrease -= CalculateIncomeTakeTime;
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
    private void CalculateSpendTime()
    {
        SpendTime -= NeighborhoodManager.ValueSystem.ValueLevel * .015f;

        if (SpendTime < 0.001f)
            SpendTime = 0.001f;

        Debug.Log("Spend Time: " + SpendTime);
    }

    private void CalculateIncomeTakeTime()
    {
        TakeIncomeTime -= NeighborhoodManager.ValueSystem.ValueLevel * .015f;

        if (TakeIncomeTime < 0.03f)
            TakeIncomeTime = 0.03f;

        Debug.Log("Take Income Time: " + TakeIncomeTime);
    }
}
