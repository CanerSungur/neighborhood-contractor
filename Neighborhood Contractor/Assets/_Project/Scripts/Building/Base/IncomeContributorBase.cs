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

    [Header("-- CONSTRUCTION SETUP --")]
    [SerializeField] private GameObject finishedHouse;
    [SerializeField] private GameObject constructionLevel_1;
    [SerializeField] private GameObject constructionLevel_2;
    
    [Header("-- UPDATE SETUP --")]
    [SerializeField] private GameObject[] upgradedHouses;

    [Header("-- BUILD SETUP --")]
    [SerializeField] private int cost = 10000;
    [SerializeField] private int upgradeCost = 2000;
    [SerializeField] private Transform moneyPointTransform;
    private int _consumedMoney;
    
    [Header("-- PROPERTIES --")]
    [SerializeField] private int neighborhoodValueContribution = 25;
    [SerializeField] private int neighborhoodPopulationContribution = 3;
    [SerializeField] private int maxLevel = 3;
    private int currentLevel;
    
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

        EnableArea(buildArea.gameObject);
        DisableArea(incomeArea.gameObject);
        DisableArea(upgradeArea.gameObject);

        PlayerIsInBuildArea = false;
        _consumedMoney = 0;
        currentLevel = 0;

        EnableRelevantHouse(currentLevel);
    }

    private void OnEnable()
    {
        Init();
    }

    private void EnableRelevantHouse(int currentLevel)
    {
        for (int i = 0; i < upgradedHouses.Length; i++)
            upgradedHouses[i].SetActive(false);

        if (currentLevel == 0)
        {
            finishedHouse.SetActive(false);
            constructionLevel_1.SetActive(true);
            constructionLevel_2.SetActive(false);
        }
        else if (currentLevel == 1)
        {
            finishedHouse.SetActive(true);
            constructionLevel_1.SetActive(false);
            constructionLevel_2.SetActive(false);
        }
        else if (currentLevel > 2)
        {
            finishedHouse.SetActive(false);
            constructionLevel_1.SetActive(false);
            constructionLevel_2.SetActive(false);
            upgradedHouses[currentLevel - 2].SetActive(true);
        }
    }

    #region Check Build States

    private void UpdateConstructionState()
    {
        if (_consumedMoney >= cost * 0.5f)
        {
            constructionLevel_1.SetActive(false);
            constructionLevel_2.SetActive(true);
        }
        else
        {
            constructionLevel_1.SetActive(true);
            constructionLevel_2.SetActive(false);
        }
    }

    private void FinishConstruction()
    {
        finishedHouse.SetActive(true);
        constructionLevel_1.SetActive(false);
        constructionLevel_2.SetActive(false);
    }

    private void UpdateUpgradeState()
    {
        finishedHouse.SetActive(false);
        if (currentLevel - 3 >= 0)
            upgradedHouses[currentLevel - 3].SetActive(false);
        
        upgradedHouses[currentLevel - 2].SetActive(true);
    }

    #endregion

    public void ConsumeMoney(int amount)
    {
        if (CanBeBuilt)
        {
            _consumedMoney += amount;
            _textHandler.SetMoneyText(cost - _consumedMoney);

            UpdateConstructionState();
        }
    }

    public void FinishBuilding()
    {
        _textHandler.DisableMoneyText();

        DisableArea(buildArea.gameObject);
        EnableArea(incomeArea.gameObject);
        EnableArea(upgradeArea.gameObject);

        currentLevel++;
        OnStartSpawningIncome?.Invoke();
        NeighborhoodEvents.OnIncreaseValue?.Invoke(neighborhoodValueContribution);
        NeighborhoodEvents.OnIncreasePopulation?.Invoke(neighborhoodPopulationContribution);
        NeighborhoodEvents.OnCheckForPopulationSufficiency?.Invoke();

        FinishConstruction();
    }

    public void UpgradeBuilding()
    {
        currentLevel++;
        NeighborhoodEvents.OnIncreaseValue?.Invoke(neighborhoodValueContribution);
        NeighborhoodEvents.OnIncreasePopulation?.Invoke(neighborhoodPopulationContribution);
        NeighborhoodEvents.OnCheckForPopulationSufficiency?.Invoke();

        UpdateUpgradeState();

        if (currentLevel == maxLevel)
            DisableArea(upgradeArea.gameObject);
    }

    private void EnableArea(GameObject area) => area.SetActive(true);
    private void DisableArea(GameObject area) => area.SetActive(false);
    public void IncomeMoneyIsSent(Money money) => _incomeHandler.RemoveIncomeMoney(money);
}
