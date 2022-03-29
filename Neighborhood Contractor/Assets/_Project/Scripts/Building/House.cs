using UnityEngine;

public class House : IncomeContributorBuilding, IRentable
{
    [Header("-- RENTABLE SETUP --")]
    [SerializeField] private GameObject housePopulation;
    [SerializeField] private int maxBuildingPopulation = 4;
    private int _currentBuildingPopulation = 0;

    public GameObject BuildingPopulation => housePopulation;
    public int CurrentBuildingPopulation => throw new System.NotImplementedException();
    public int MaxBuildingPopulation => maxBuildingPopulation;
}
