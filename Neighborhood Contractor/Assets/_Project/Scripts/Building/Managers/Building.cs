using UnityEngine;

public class Building : MonoBehaviour
{
    [SerializeField] private int index;
    public int Index => index;

    private IncomeSpawner incomeSpawner;
    public IncomeSpawner IncomeSpawner => incomeSpawner == null ? incomeSpawner = GetComponent<IncomeSpawner>() : incomeSpawner;

    [Header("-- REFERENCES --")]
    private Buildable _buildable;
    private Upgradeable _upgradeable;
    private RequirePopulation _requirePopulation;
    private ContributionHandler _contributionHandler;
    private Rentable _rentable;
    private IncomeSpawner _incomeSpawner;

    #region Properties

    public bool Built => _buildable.Built;
    public Buildable Buildable => _buildable;
    public RequirePopulation RequirePopulation => _requirePopulation;
    public ContributionHandler ContributionHandler => _contributionHandler;
    public Rentable Rentable => _rentable;
    //public IncomeSpawner IncomeSpawner => _incomeSpawner;
    public Upgradeable Upgradeable => _upgradeable;

    #endregion

    private void Init() 
    {
        if (TryGetComponent(out _requirePopulation))
            _requirePopulation.Init();

        _buildable = GetComponent<Buildable>();
        _buildable.Init(this);

        if (TryGetComponent(out _upgradeable))
            _upgradeable.Init();
        if (TryGetComponent(out _rentable))
            _rentable.Init();
        if (TryGetComponent(out _contributionHandler))
            _contributionHandler.Init(this);

        //if (TryGetComponent(out _incomeSpawner))
        //    _incomeSpawner.Init();
        if (IncomeSpawner)
            IncomeSpawner.Init();

        LoadData();
    }

    private void SaveData()
    {
        if (Built)
        {
            PlayerPrefs.SetString($"Buildable_{Index}", "Built");

            if (Upgradeable)
            {
                PlayerPrefs.SetInt($"CurrentLevel_{Index}", Upgradeable.CurrentLevel);
            }
        }
        else
        {
            PlayerPrefs.SetString($"Buildable_{Index}", "NotBuilt");
            PlayerPrefs.SetInt($"ConsumedMoney_{Index}", Buildable.ConsumedMoney);
        }

        PlayerPrefs.Save();
    }

    private void LoadData()
    {
        if (PlayerPrefs.GetString($"Buildable_{Index}") == "Built")
        {
            // check for upgrade part
            Buildable.SkipThisState();
            if (_upgradeable)
            {
                if (PlayerPrefs.GetInt($"CurrentLevel_{Index}") > 1)
                    Buildable.DisableMesh();

                Upgradeable.CheckThisState(PlayerPrefs.GetInt($"CurrentLevel_{Index}"));
            }

            if (IncomeSpawner)
            {
                IncomeSpawner.CheckThisState();

                if (_rentable)
                {
                    Rentable.BuildingIsFinished();
                    if (Upgradeable)
                    {
                        for (int i = 1; i < Upgradeable.CurrentLevel; i++)
                            Rentable.UpdateProperties();
                    }
                    IncomeSpawner.WaitForRent();
                    for (int i = 0; i < Rentable.MaxBuildingPopulation; i++)
                    {
                        Rentable.Rented();
                    }
                }
                else
                    IncomeSpawner.StartSpawningIncome();
            }
        }
        else
        {
            Buildable.CheckThisState(PlayerPrefs.GetInt($"ConsumedMoney_{Index}"));
        }
    }

    private void OnEnable()
    {
        Init();

        if (_upgradeable)
        {
            _buildable.OnBuildFinished += () => _upgradeable.Activate();
            _upgradeable.OnUpgradeHappened += () => _buildable.DisableMesh();
        }

        if (_requirePopulation)
            _requirePopulation.OnPopulationSufficient += () => _buildable.Activate();

        if (!_rentable && IncomeSpawner)
            _buildable.OnBuildFinished += () => IncomeSpawner.StartSpawningIncome();

        if (_rentable && IncomeSpawner)
            _buildable.OnBuildFinished += () => IncomeSpawner.WaitForRent();
    }

    private void OnDisable()
    {
        if (_upgradeable)
        {
            _buildable.OnBuildFinished -= () => _upgradeable.Activate();
            _upgradeable.OnUpgradeHappened -= () => _buildable.DisableMesh();
        }

        if (_requirePopulation)
            _requirePopulation.OnPopulationSufficient -= () => _buildable.Activate();

        if (!_rentable && IncomeSpawner)
            _buildable.OnBuildFinished -= () => IncomeSpawner.StartSpawningIncome();

        if (_rentable && IncomeSpawner)
            _buildable.OnBuildFinished -= () => IncomeSpawner.WaitForRent();

        SaveData();

        //PlayerPrefs.DeleteAll();
    }
}
