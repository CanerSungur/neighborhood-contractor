using System.Collections.Generic;
using UnityEngine;
using System;
using ZestGames.Utility;

public abstract class IncomeContributorBuilding : MonoBehaviour, IBuilding, IContributorIncome
{
    [Header("-- SCRIPT REFERENCES --")]
    [SerializeField] private BuildingIncomeHandler incomeHandler;
    private BuildingTextHandler _textHandler;

    [Header("-- AREA SETUP --")]
    [SerializeField] private Transform lockedBuildArea;
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
    [SerializeField] private float upgradeCostIncreaseRate = 1.25f;
    [SerializeField] private Transform moneyPointTransform;
    private int _consumedMoney;

    [Header("-- PROPERTIES --")]
    [SerializeField] private int requiredPopulation = 5;
    [SerializeField] private int neighborhoodValueContribution = 25;
    [SerializeField] private int neighborhoodPopulationContribution = 3;
    [SerializeField] private int maxLevel = 3;
    private int _currentLevel;

    [Header("-- RENT SETUP --")]
    private bool _isRentable = false;

    public event Action OnStartSpawningIncome;

    #region Building Properties

    public bool PlayerIsInBuildArea { get; set; }
    public int BuildCost => cost;
    public int UpgradeCost => (int)(cost * upgradeCostIncreaseRate * _currentLevel);
    public int NextLevelNumber => _currentLevel + 1;
    public bool CanBeBuilt => StatManager.CurrentCarry > 0 && !Built && PopulationIsEnough;
    public bool CanBeUpgraded => Built && _currentLevel < maxLevel;
    public bool Built => _consumedMoney == cost;
    public int ConsumedMoney => _consumedMoney;
    public Transform BuildArea => buildArea;
    public Transform MoneyPointTransform => moneyPointTransform;
    public int RequiredPopulation => requiredPopulation;
    public bool PopulationIsEnough => NeighborhoodManager.Population >= RequiredPopulation;

    #endregion

    #region Income Handler Properties

    public List<Money> IncomeMoney => incomeHandler.IncomeMoney;
    public bool CanCollectIncome => incomeHandler.IncomeMoney.Count != 0 && incomeHandler.MoneyCount > 0 && StatManager.CurrentCarry < StatManager.CarryCapacity;
    public int IncomeMoneyCount => incomeHandler.MoneyCount;

    #endregion

    private void Init()
    {
        _textHandler = GetComponent<BuildingTextHandler>();
        CheckForPopulationSufficiency();

        PlayerIsInBuildArea = false;
        _consumedMoney = 0;
        _currentLevel = 0;

        EnableRelevantHouse(_currentLevel);

        _textHandler.SetRequiredMoneyText(cost);
        _textHandler.SetConsumedMoneyText(_consumedMoney);
    }

    private void OnEnable()
    {
        Init();

        NeighborhoodEvents.OnCheckForPopulationSufficiency += CheckForPopulationSufficiency;
        ValueBarEvents.OnValueLevelIncrease += () => Delayer.DoActionAfterDelay(this, 0.1f, () =>
        {
            _textHandler.SetIncomePerSecondText(incomeHandler.IncomePerSecond);
        });
    }

    private void OnDisable()
    {
        NeighborhoodEvents.OnCheckForPopulationSufficiency -= CheckForPopulationSufficiency;
        ValueBarEvents.OnValueLevelIncrease -= () => Delayer.DoActionAfterDelay(this, 0.1f, () =>
        {
            _textHandler.SetIncomePerSecondText(incomeHandler.IncomePerSecond);
        });
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
        if (_currentLevel - 3 >= 0)
            upgradedHouses[_currentLevel - 3].SetActive(false);

        upgradedHouses[_currentLevel - 2].SetActive(true);
    }

    #endregion

    private void CheckForPopulationSufficiency() => ApplyBuildableState(PopulationIsEnough);

    private void ApplyBuildableState(bool buildable)
    {
        if (Built)
        {
            if (_currentLevel == maxLevel)
                DisableArea(upgradeArea.gameObject);
            else
                EnableArea(upgradeArea.gameObject);

            DisableArea(lockedBuildArea.gameObject);
            DisableArea(buildArea.gameObject);
            EnableArea(incomeArea.gameObject);
        }
        else
        {
            if (buildable)
            {
                DisableArea(lockedBuildArea.gameObject);
                EnableArea(buildArea.gameObject);
                DisableArea(incomeArea.gameObject);
                DisableArea(upgradeArea.gameObject);

                _textHandler.SetRequiredMoneyText(cost);
                _textHandler.SetConsumedMoneyText(_consumedMoney);
            }
            else
            {
                EnableArea(lockedBuildArea.gameObject);
                DisableArea(buildArea.gameObject);
                DisableArea(incomeArea.gameObject);
                DisableArea(upgradeArea.gameObject);

                _textHandler.SetPopulationText(RequiredPopulation);
            }
        }
    }

    public void ConsumeMoney(int amount)
    {
        if (CanBeBuilt)
        {
            _consumedMoney += amount;
            _textHandler.SetConsumedMoneyText(_consumedMoney);

            UpdateConstructionState();
        }
    }

    public void FinishBuilding()
    {
        //_textHandler.DisableMoneyText();

        DisableArea(buildArea.gameObject);
        EnableArea(incomeArea.gameObject);
        EnableArea(upgradeArea.gameObject);

        _currentLevel++;
        OnStartSpawningIncome?.Invoke();
        NeighborhoodEvents.OnIncreaseValue?.Invoke(neighborhoodValueContribution);
        NeighborhoodEvents.OnIncreasePopulation?.Invoke(neighborhoodPopulationContribution);
        NeighborhoodEvents.OnCheckForPopulationSufficiency?.Invoke();

        FinishConstruction();

        // we set income text on first enable of income area.
        _textHandler.SetIncomePerSecondText(incomeHandler.IncomePerSecond);
    }

    public void UpgradeBuilding()
    {
        _currentLevel++;
        NeighborhoodEvents.OnIncreaseValue?.Invoke(neighborhoodValueContribution);
        NeighborhoodEvents.OnIncreasePopulation?.Invoke(neighborhoodPopulationContribution);
        NeighborhoodEvents.OnCheckForPopulationSufficiency?.Invoke();

        UpdateUpgradeState();

        if (_currentLevel == maxLevel)
            DisableArea(upgradeArea.gameObject);
    }

    private void EnableArea(GameObject area) => area.SetActive(true);
    private void DisableArea(GameObject area) => area.SetActive(false);
    public void IncomeMoneyIsSent(Money money) => incomeHandler.RemoveIncomeMoney(money);
}
