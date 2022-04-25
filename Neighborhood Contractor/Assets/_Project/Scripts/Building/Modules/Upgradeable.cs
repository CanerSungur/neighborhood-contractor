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

    [Header("-- COMPLAINT SETUP --")]
    [SerializeField] private GameObject upgradeDenied;
    [SerializeField] private Animation upgradeDeniedAnim;
    [SerializeField] private Collider coll;

    #region Properties

    public bool PlayerIsInArea { get; set; }
    public Transform MoneyPointTransform => moneyPointTransform;
    public bool CanBeUpgraded => _currentLevel < _maxLevel && Building.Built;
    public int UpgradeCost => (int)(cost * upgradeCostIncreaseRate * _currentLevel);
    public int NextLevelNumber => _currentLevel + 1;
    public int CurrentLevel => _currentLevel;

    #endregion

    public event Action OnUpgradeHappened;

    public void Init()
    {
        upgradeArea.SetActive(false);
        //upgradeDenied.SetActive(false);
        upgradeDeniedAnim.Rewind();
        upgradeDeniedAnim.Play("Income_Accepted_LegacyAnim");
        coll.enabled = true;
        EnableRelevantPhase();
    }

    public void CheckThisState(int currentLevel)
    {
        _currentLevel = currentLevel;
        _maxLevel = upgradePhases.Length + 1;

        for (int i = 0; i < upgradePhases.Length; i++)
            upgradePhases[i].SetActive(false);

        EnableRelevantPhase();

        if (_currentLevel == _maxLevel)
            upgradeArea.SetActive(false);
        else
            upgradeArea.SetActive(true);
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
        NeighborhoodEvents.OnBuildingUpgraded?.Invoke(Building);

        _currentLevel++;
        Building.CurrentLevel++;
        if (Building.ContributionHandler)
            Building.ContributionHandler.TriggerContribution();
        NeighborhoodEvents.OnCheckForPopulationSufficiency?.Invoke();

        UpdatePhases();

        if (_currentLevel == _maxLevel)
            upgradeArea.SetActive(false);

        FeedbackEvents.OnGiveFeedback?.Invoke("BUILDING UPGRADED!", FeedbackUI.Colors.ValueLevelIncrease);
        AudioHandler.PlayAudio(AudioHandler.AudioType.BuildingFinished);
        Player.Upgrading = false;
        AnalyticEvents.OnUpgradeFinished?.Invoke();
    }

    #region Complaint Functions

    public void StopUpgrade()
    {
        //upgradeDenied.SetActive(true);
        upgradeDeniedAnim.Rewind();
        upgradeDeniedAnim.Play("Income_Denied_LegacyAnim");
        coll.enabled = false;

    }

    public void StartUpgrade()
    {
        //upgradeDenied.SetActive(false);
        upgradeDeniedAnim.Rewind();
        upgradeDeniedAnim.Play("Income_Accepted_LegacyAnim");
        coll.enabled = true;
    }

    #endregion
}
