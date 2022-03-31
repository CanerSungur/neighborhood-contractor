using UnityEngine;
using System;

[RequireComponent(typeof(Building))]
public class Upgradeable : MonoBehaviour
{
    private Building building;
    public Building Building => building == null ? building = GetComponent<Building>() : building;

    [Header("-- SETUP --")]
    [SerializeField] private int cost = 1000;
    [SerializeField] private GameObject[] upgradePhases;
    [SerializeField] private GameObject upgradeArea;
    [SerializeField] private float upgradeCostIncreaseRate = 1.25f;
    [SerializeField] private Transform moneyPointTransform;
    private int _maxLevel, _currentLevel;

    #region Properties

    public bool PlayerIsInArea { get; set; }
    public Transform MoneyPointTransform => moneyPointTransform;
    public bool CanBeUpgraded => _currentLevel < _maxLevel && Building.Built;
    public int UpgradeCost => (int)(cost * upgradeCostIncreaseRate * _currentLevel);
    public int NextLevelNumber => _currentLevel + 1;

    #endregion

    public event Action OnUpgradeHappened;

    public void Init()
    {
        PlayerIsInArea = false;
        upgradeArea.SetActive(false);
        EnableRelevantPhase();
    }

    public void Activate()
    {
        _currentLevel = 1;
        _maxLevel = upgradePhases.Length + 1;
        upgradeArea.SetActive(true);
    }

    private void EnableRelevantPhase()
    {
        for (int i = 0; i < upgradePhases.Length; i++)
        {
            if (_currentLevel - 2 == i)
                upgradePhases[i].SetActive(true);
            else
                upgradePhases[i].SetActive(false);
        }
    }

    private void UpdatePhases()
    {
        if (_currentLevel - 3 >= 0)
            upgradePhases[_currentLevel - 3].SetActive(false);

        upgradePhases[_currentLevel - 2].SetActive(true);
    }

    public void UpgradeBuilding()
    {
        OnUpgradeHappened?.Invoke();

        _currentLevel++;
        if (Building.ContributionHandler)
            Building.ContributionHandler.TriggerContribution();
        NeighborhoodEvents.OnCheckForPopulationSufficiency?.Invoke();

        UpdatePhases();

        if (_currentLevel == _maxLevel)
            upgradeArea.SetActive(false);
    }
}
