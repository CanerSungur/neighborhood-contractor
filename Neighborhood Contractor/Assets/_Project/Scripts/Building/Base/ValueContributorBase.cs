using UnityEngine;

public abstract class ValueContributorBase : MonoBehaviour, IBuilding, IContributorValue
{
    [Header("-- SCRIPT REFERENCES --")]
    private BuildingTextHandler _textHandler;

    [Header("-- AREA SETUP --")]
    [SerializeField] private Transform lockedBuildArea;
    [SerializeField] private Transform buildArea;

    [Header("-- APPEREANCE SETUP --")]
    [SerializeField] private Material constructionMat;
    [SerializeField] private Material finishedMat;
    private Renderer _renderer;

    [Header("-- BUILD SETUP --")]
    [SerializeField] private int cost = 5000;
    [SerializeField] private int upgradeCost = 2000;
    [SerializeField] private Transform moneyPointTransform;
    private int _consumedMoney;

    [Header("-- PROPERTIES --")]
    [SerializeField] private int neighborhoodValueContribution = 100;
    [SerializeField] private int requiredPopulation = 5;
    private int currentLevel;
    private int maxLevel = 1;

    #region Building Properties

    public bool PlayerIsInBuildArea { get; set; }
    public int BuildCost => cost;
    public int UpgradeCost => upgradeCost;
    public int ConsumedMoney => _consumedMoney;
    public bool Built => _consumedMoney == cost;
    public bool CanBeBuilt => StatManager.CurrentCarry > 0 && !Built && PopulationIsEnough;
    public bool CanBeUpgraded => Built && currentLevel < maxLevel;
    public Transform MoneyPointTransform => moneyPointTransform;

    #endregion

    #region Value Handler Properties

    public int ValueContribution => neighborhoodValueContribution;
    public int RequiredPopulation => requiredPopulation;
    public bool PopulationIsEnough => GameManager.Instance.neighborhoodManager.Population >= RequiredPopulation;

    #endregion

    private void Init()
    {
        _textHandler = GetComponent<BuildingTextHandler>();

        _renderer = GetComponent<Renderer>();
        _renderer.material = constructionMat;

        CheckForPopulationSufficiency();

        PlayerIsInBuildArea = false;
        _consumedMoney = 0;
        currentLevel = 0;
    }

    private void Start()
    {
        Init();

        NeighborhoodEvents.OnCheckForPopulationSufficiency += CheckForPopulationSufficiency;
    }

    private void OnDisable()
    {
        NeighborhoodEvents.OnCheckForPopulationSufficiency -= CheckForPopulationSufficiency;
    }

    public void ConsumeMoney(int amount)
    {
        if (CanBeBuilt)
        {
            _consumedMoney += amount;
            _textHandler.SetMoneyText(cost - _consumedMoney);
        }
    }

    public void FinishBuilding()
    {
        _renderer.material = finishedMat;
        _textHandler.DisableMoneyText();
        
        DisableArea(buildArea.gameObject);

        currentLevel++;
        NeighborhoodEvents.OnIncreaseValue?.Invoke(neighborhoodValueContribution);
    }

    public void UpgradeBuilding() { }

    private void ApplyBuildableState(bool buildable)
    {
        if (buildable)
        {
            DisableArea(lockedBuildArea.gameObject);
            EnableArea(buildArea.gameObject);

            _textHandler.SetMoneyText(cost);
        }
        else
        {
            EnableArea(lockedBuildArea.gameObject);
            DisableArea(buildArea.gameObject);

            _textHandler.SetPopulationText(RequiredPopulation);
        }
    }

    private void CheckForPopulationSufficiency()
    {
        if (GameManager.Instance)
            ApplyBuildableState(PopulationIsEnough);
    }

    private void EnableArea(GameObject area) => area.SetActive(true);
    private void DisableArea(GameObject area) => area.SetActive(false);
}
