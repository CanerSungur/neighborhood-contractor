using UnityEngine;

public abstract class BuildingBase : IBuilding
{
    [Header("-- BUILDING SETUP --")]
    [SerializeField] private int cost = 5000;
    [SerializeField] private Transform moneyPointTransform;
    private int _consumedMoney;

    [Header("-- PROPERTIES --")]
    [SerializeField] private int maxLevel = 1;
    [SerializeField] private float upgradeCostIncreaseRate = 1.25f;
    private int _currentLevel;

    [Header("-- REQUIREMENTS --")]
    [SerializeField] private int requiredPopulation = 5;

    public bool PlayerIsInBuildArea { get; set; }
    public Transform MoneyPointTransform => moneyPointTransform;
    public int BuildCost => cost;
    public int UpgradeCost => (int)(cost * upgradeCostIncreaseRate * _currentLevel);
    public int NextLevelNumber => _currentLevel + 1;
    public bool CanBeBuilt => StatManager.CurrentCarry > 0 && !Built && PopulationIsEnough;
    public bool CanBeUpgraded => Built && _currentLevel < maxLevel;
    public bool Built => _consumedMoney == cost;
    public int ConsumedMoney => _consumedMoney;
    public int RequiredPopulation => requiredPopulation;
    public bool PopulationIsEnough => NeighborhoodManager.Population >= RequiredPopulation;

    public void ConsumeMoney(int amount)
    {
        if (CanBeBuilt)
        {
            //_consumedMoney += amount;
            //_textHandler.SetConsumedMoneyText(_consumedMoney);

            //UpdateConstructionState();
        }
    }

    public void FinishBuilding()
    {
        throw new System.NotImplementedException();
    }

    public void UpgradeBuilding()
    {
        throw new System.NotImplementedException();
    }
}
