using UnityEngine;
using System;

// Handle only build mechanic
[RequireComponent(typeof(Building))]
public class Build : MonoBehaviour
{
    private Building building;
    public Building Building => building == null ? building = GetComponent<Building>() : building;

    [Header("-- SETUP --")]
    [SerializeField] private int buildCost = 1000;
    [SerializeField] private Transform moneyPointTransform;
    private int _consumedMoney;

    [Header("-- PHASE SETUP --")]
    [SerializeField] private GameObject finishedHouse;
    [SerializeField] private GameObject constructionLevel_1;
    [SerializeField] private GameObject constructionLevel_2;

    public bool PlayerIsInBuildArea { get; set; }
    public int BuildCost => buildCost;
    public bool Built => _consumedMoney == buildCost;
    public bool CanBeBuilt => StatManager.CurrentCarry > 0 && !Built;
    public Transform MoneyPointTransform => moneyPointTransform;

    public event Action OnBuildFinished;

    public void Init(Building building)
    {
        _consumedMoney = 0;
    }

    public void ConsumeMoney(int amount)
    {
        if (CanBeBuilt)
        {
            _consumedMoney += amount;
            Building.TextHandler.SetConsumedMoneyText(_consumedMoney);

            UpdateConstructionState();
        }
    }

    private void UpdateConstructionState()
    {
        if (_consumedMoney >= buildCost * 0.5f)
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

    public void FinishBuilding()
    {
        OnBuildFinished?.Invoke();

        //_textHandler.DisableMoneyText();

        //DisableArea(buildArea.gameObject);
        //EnableArea(incomeArea.gameObject);
        //EnableArea(upgradeArea.gameObject);

        //_currentLevel++;
        //OnStartSpawningIncome?.Invoke();
        //NeighborhoodEvents.OnIncreaseValue?.Invoke(neighborhoodValueContribution);
        //NeighborhoodEvents.OnIncreasePopulation?.Invoke(neighborhoodPopulationContribution);
        //NeighborhoodEvents.OnCheckForPopulationSufficiency?.Invoke();

        FinishConstruction();

        //// we set income text on first enable of income area.
        //Building.TextHandler.SetIncomePerSecondText(incomeHandler.IncomePerSecond);
    }

    private void EnableArea(GameObject area) => area.SetActive(true);
    private void DisableArea(GameObject area) => area.SetActive(false);
    //public void IncomeMoneyIsSent(Money money) => incomeHandler.RemoveIncomeMoney(money);
}
