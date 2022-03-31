using UnityEngine;

public class Building : MonoBehaviour
{
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
    public IncomeSpawner IncomeSpawner => _incomeSpawner;
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

        if (TryGetComponent(out _incomeSpawner))
            _incomeSpawner.Init();
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
    }
}
