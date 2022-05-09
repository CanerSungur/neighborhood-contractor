using UnityEngine;

public class Building : MonoBehaviour
{
    [SerializeField] private int index;
    public int Index => index;
    private bool _deleteSaveData = false;

    [Header("-- REFERENCES --")]
    private Buildable _buildable;
    private Upgradeable _upgradeable;
    private RequirePopulation _requirePopulation;
    private ContributionHandler _contributionHandler;
    private Rentable _rentable;
    private IncomeSpawner _incomeSpawner;
    private Repairable _repairable;
    private AccidentCauser _accidentCauser;

    #region Properties

    public int CurrentLevel { get; set; }
    public bool Built => _buildable.Built;
    public bool CanBeRepaired => Built && _accidentCauser.AccidentHappened && _repairable.CanBeRepaired;
    public bool CanAccidentHappen => Built && !_accidentCauser.AccidentHappened && _rentable.BuildingIsFull && !BuildingUpgradeUI.IsOpen && StatManager.CurrentAccidentCount < StatManager.MaxAccidentCount;
    public Buildable Buildable => _buildable;
    public RequirePopulation RequirePopulation => _requirePopulation;
    public ContributionHandler ContributionHandler => _contributionHandler;
    public Rentable Rentable => _rentable;
    public IncomeSpawner IncomeSpawner => _incomeSpawner;
    public Upgradeable Upgradeable => _upgradeable;
    public Repairable Repairable => _repairable;
    public AccidentCauser AccidentCauser => _accidentCauser;

    #endregion

    private void Init() 
    {
        CurrentLevel = 0;

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

        if (TryGetComponent(out _incomeSpawner))
            _incomeSpawner.Init();
        //if (IncomeSpawner)
        //    IncomeSpawner.Init();

        LoadData();
        if (BuildManager.Instance.BuildingCount >= 2 && SingleMoneySpawnPointManager.FreeSpawnActive)
            NeighborhoodEvents.OnDisableFreeMoneySpawn?.Invoke();
        else
            SingleMoneySpawnPointManager.FreeSpawnActive = true;

        if (Built)
            CurrentLevel = 1;

        if (Upgradeable)
            CurrentLevel = Upgradeable.CurrentLevel;

        if (TryGetComponent(out _accidentCauser))
            _accidentCauser.Init(this);

        if (TryGetComponent(out _repairable))
            _repairable.Init(this);
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

            if (_incomeSpawner)
            {
                ZestGames.Utility.Delayer.DoActionAfterDelay(this, 0.5f, () => {
                    _incomeSpawner.CheckThisState();
                    //_incomeSpawner.CheckIfThisHasMoney();
                   
                    ZestGames.Utility.Delayer.DoActionAfterDelay(this, 1f, () => {
                        if (_rentable)
                        {
                            Rentable.BuildingIsFinished();
                            if (Upgradeable)
                            {
                                for (int i = 1; i < Upgradeable.CurrentLevel; i++)
                                    Rentable.UpdateProperties();
                            }
                            _incomeSpawner.WaitForRent();
                            for (int i = 0; i < Rentable.MaxBuildingPopulation; i++)
                            {
                                Rentable.RentForLoad();
                            }
                        }
                        else
                            _incomeSpawner.StartSpawningIncome();
                        //ZestGames.Utility.Delayer.DoActionAfterDelay(this, 2f, () => _incomeSpawner.StartSpawningIncome());
                    });
                });
            }
        }
        else
        {
            ZestGames.Utility.Delayer.DoActionAfterDelay(this, 0.5f, () => Buildable.CheckThisState(PlayerPrefs.GetInt($"ConsumedMoney_{Index}")));
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

        if (!_rentable && _incomeSpawner)
            _buildable.OnBuildFinished += () => _incomeSpawner.StartSpawningIncome();

        if (_rentable && _incomeSpawner)
            _buildable.OnBuildFinished += () => _incomeSpawner.WaitForRent();

        if (_accidentCauser && _repairable)
        {
            _repairable.OnBuildingRepaired += () => _accidentCauser.Repaired();
            _accidentCauser.OnAccidentHappened += (Building building) => _repairable.Broken(building);
        }

        ZestGames.Utility.Delayer.DoActionAfterDelay(this, 0.5f, () => _deleteSaveData = GameManager.Instance.DeleteSaveGame);
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

        if (!_rentable && _incomeSpawner)
            _buildable.OnBuildFinished -= () => _incomeSpawner.StartSpawningIncome();

        if (_rentable && _incomeSpawner)
            _buildable.OnBuildFinished -= () => _incomeSpawner.WaitForRent();

        if (_accidentCauser && _repairable)
        {
            _repairable.OnBuildingRepaired -= () => _accidentCauser.Repaired();
            _accidentCauser.OnAccidentHappened -= (Building building) => _repairable.Broken(building);
        }
    }

    private void OnApplicationPause(bool pause)
    {
        SaveData();

        if (_deleteSaveData)
            PlayerPrefs.DeleteAll();
    }

    private void OnApplicationQuit()
    {
        SaveData();

        if (_deleteSaveData)
            PlayerPrefs.DeleteAll();
    }
}
