using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class IncomeContributorBase : MonoBehaviour, IBuilding, IContributorIncome
{
    [Header("-- SCRIPT REFERENCES --")]
    [SerializeField] private BuildingIncomeHandler _incomeHandler;
    private BuildingTextHandler _textHandler;

    [Header("-- AREA SETUP --")]
    [SerializeField] private Transform buildArea;
    [SerializeField] private Transform incomeArea;
    [SerializeField] private Transform upgradeArea;

    [Header("-- APPEREANCE SETUP --")]
    [SerializeField] private Material constructionMat;
    [SerializeField] private Material finishedMat;
    [SerializeField] private Material upgradedMat;
    private Renderer _renderer;

    [Header("-- BUILD SETUP --")]
    [SerializeField] private int cost = 10000;
    [SerializeField] private int upgradeCost = 2000;
    [SerializeField] private Transform moneyPointTransform;
    private int _consumedMoney;
    
    [Header("-- PROPERTIES --")]
    private int currentLevel;
    private int maxLevel = 2;
    private int neighborhoodValueContribution = 25;
    private int neighborhoodPopulationContribution = 3;

    public event Action OnStartSpawningIncome;

    #region Building Properties

    public bool PlayerIsInBuildArea { get; set; }
    public int BuildCost => cost;
    public int UpgradeCost => upgradeCost;
    public bool CanBeBuilt => StatManager.CurrentCarry > 0 && !Built;
    public bool CanBeUpgraded => Built && currentLevel < maxLevel;
    public bool Built => _consumedMoney == cost;
    public int ConsumedMoney => _consumedMoney;
    public Transform BuildArea => buildArea;
    public Transform MoneyPointTransform => moneyPointTransform;

    #endregion

    #region Income Handler Properties

    public List<Money> IncomeMoney => _incomeHandler.IncomeMoney;
    public bool CanCollectIncome => _incomeHandler.IncomeMoney.Count != 0 && _incomeHandler.MoneyCount > 0 && StatManager.CurrentCarry < StatManager.CarryCapacity;
    public int IncomeMoneyCount => _incomeHandler.MoneyCount;

    #endregion

    private void Init()
    {
        _textHandler = GetComponent<BuildingTextHandler>();
        _textHandler.SetMoneyText(cost);

        _renderer = GetComponent<Renderer>();
        _renderer.material = constructionMat;

        EnableArea(buildArea.gameObject);
        DisableArea(incomeArea.gameObject);
        DisableArea(upgradeArea.gameObject);

        PlayerIsInBuildArea = false;
        _consumedMoney = 0;
        currentLevel = 0;
    }

    private void OnEnable()
    {
        Init();
    }

    public void ConsumeMoney(int amount)
    {
        _consumedMoney += amount;
        _textHandler.SetMoneyText(cost - _consumedMoney);
    }

    public void FinishBuilding()
    {
        _renderer.material = finishedMat;
        _textHandler.DisableMoneyText();

        DisableArea(buildArea.gameObject);
        EnableArea(incomeArea.gameObject);
        EnableArea(upgradeArea.gameObject);

        currentLevel++;
        OnStartSpawningIncome?.Invoke();
        NeighborhoodEvents.OnIncreaseValue?.Invoke(neighborhoodValueContribution);
        NeighborhoodEvents.OnIncreasePopulation?.Invoke(neighborhoodPopulationContribution);
        NeighborhoodEvents.OnCheckForPopulationSufficiency?.Invoke();
    }

    public void UpgradeBuilding()
    {
        _renderer.material = upgradedMat;
        DisableArea(upgradeArea.gameObject);

        currentLevel++;
        NeighborhoodEvents.OnIncreaseValue?.Invoke(neighborhoodValueContribution);
        NeighborhoodEvents.OnIncreasePopulation?.Invoke(neighborhoodPopulationContribution);
        NeighborhoodEvents.OnCheckForPopulationSufficiency?.Invoke();
    }

    private void EnableArea(GameObject area) => area.SetActive(true);
    private void DisableArea(GameObject area) => area.SetActive(false);
    public void IncomeMoneyIsSent(Money money) => _incomeHandler.RemoveIncomeMoney(money);
}
