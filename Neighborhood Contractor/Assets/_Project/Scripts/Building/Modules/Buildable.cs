using UnityEngine;
using TMPro;
using System;

[RequireComponent(typeof(Building))]
public class Buildable : MonoBehaviour
{
    private Building building;
    public Building Building => building == null ? building = GetComponent<Building>() : building;

    [Header("-- PROPERTIES --")]
    [SerializeField] private int buildCost = 1000;
    private int _consumedMoney;

    [Header("-- OBJECT REFERENCES --")]
    [SerializeField] private GameObject buildArea;
    [SerializeField] private Transform moneyPointTransform;

    [Header("-- TEXT REFERENCES --")]
    [SerializeField] private TextMeshProUGUI requiredMoneyText;
    [SerializeField] private TextMeshProUGUI consumedMoneyText;
 
    [Header("-- BUILD PHASE SETUP --")]
    [SerializeField] private GameObject finishedHouse;
    [SerializeField] private GameObject constructionLevel_1;
    [SerializeField] private GameObject constructionLevel_2;

    public bool PlayerIsInBuildArea { get; set; }

    #region Public Getters

    public int ConsumedMoney => _consumedMoney;
    public int BuildCost => buildCost;
    public Transform MoneyPointTransform => moneyPointTransform;

    #endregion

    #region Controls

    public bool Built => _consumedMoney == buildCost;
    public bool CanBeBuilt => StatManager.CurrentCarry > 0 && !Built;

    #endregion

    public event Action OnBuildFinished;

    public void Init(Building building)
    {
        PlayerIsInBuildArea = false;
        _consumedMoney = 0;

        EnableRelevantBuildPhase();

        if (building.RequirePopulation)
        {
            if (building.RequirePopulation.PopulationIsEnough)
                Activate();
            else
                buildArea.SetActive(false);
        }
        else
            Activate();
    }

    public void Activate()
    {
        if (Built) return;

        buildArea.SetActive(true);
        requiredMoneyText.text = buildCost.ToString("#,##0") + "$";
        consumedMoneyText.text = _consumedMoney.ToString("#,##0") + "$";
    }

    public void DisableMesh()
    {
        finishedHouse.SetActive(false);
    }

    public void ConsumeMoney(int amount)
    {
        if (CanBeBuilt)
        {
            _consumedMoney += amount;
            consumedMoneyText.text = _consumedMoney.ToString("#,##0") + "$";

            UpdateBuildPhases();
        }
    }

    private void EnableRelevantBuildPhase()
    {
        finishedHouse.SetActive(false);
        constructionLevel_1.SetActive(true);
        constructionLevel_2.SetActive(false);
    }

    private void UpdateBuildPhases()
    {
        if (_consumedMoney == buildCost)
        {
            finishedHouse.SetActive(true);
            
            constructionLevel_1.SetActive(false);
            constructionLevel_2.SetActive(false);
        }
        if (_consumedMoney >= buildCost * 0.5f && _consumedMoney < buildCost)
        {
            constructionLevel_2.SetActive(true);

            finishedHouse.SetActive(false);
            constructionLevel_1.SetActive(false);
        }
        else
        {
            constructionLevel_1.SetActive(true);

            finishedHouse.SetActive(false);
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
        buildArea.SetActive(false);

        //_currentLevel++;
        //OnStartSpawningIncome?.Invoke();
        //NeighborhoodEvents.OnIncreaseValue?.Invoke(neighborhoodValueContribution);
        if (Building.ContributionHandler)
            Building.ContributionHandler.TriggerContribution();
        NeighborhoodEvents.OnCheckForPopulationSufficiency?.Invoke();

        OnBuildFinished?.Invoke();
        FinishConstruction();

        //// we set income text on first enable of income area.
        //Building.TextHandler.SetIncomePerSecondText(incomeHandler.IncomePerSecond);
    }
}
