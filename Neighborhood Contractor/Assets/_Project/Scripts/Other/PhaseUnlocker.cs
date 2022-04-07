using UnityEngine;

public class PhaseUnlocker : MonoBehaviour
{
    [Header("-- SCRIPT REFERENCES --")]
    private PhaseUnlockerTextHandler _textHandler;

    [Header("-- BUILD SETUP --")]
    [SerializeField] private int phaseToUnlock = 2;
    [SerializeField] private int cost = 10000;
    [SerializeField] private int requiredPopulation = 5;
    [SerializeField] private Transform moneyPointTransform;
    private int _consumedMoney;

    public bool PlayerIsInBuildArea { get; set; }
    public int RequiredPopulation => requiredPopulation;
    public Transform MoneyPointTransform => moneyPointTransform;
    public bool EnoughPopulation => NeighborhoodManager.Population >= requiredPopulation;
    public bool CanBeBuilt => StatManager.CurrentCarry > 0 && !Built && EnoughPopulation;
    public bool Built => _consumedMoney == cost;
    public int PhaseToUnlock => phaseToUnlock;
    public int ConsumedMoney => _consumedMoney;

    private void Init()
    {
        _textHandler = GetComponent<PhaseUnlockerTextHandler>();

        PlayerIsInBuildArea = false;
        //_consumedMoney = 0;
        if (PhaseManager.CurrentPhase + 1 == PhaseToUnlock)
            _consumedMoney = PhaseManager.CurrentlyConsumedMoney;
        else
            _consumedMoney = 0;

        CheckForPopulationSufficiency();
        _textHandler.SetConsumedMoneyText(_consumedMoney);
        _textHandler.SetRequiredMoneyText(cost);
    }

    public void UpdateConsumedMoney()
    {
        _consumedMoney = PhaseManager.CurrentlyConsumedMoney;
        _textHandler.SetConsumedMoneyText(_consumedMoney);
    }

    private void OnEnable()
    {
        Init();

        NeighborhoodEvents.OnCheckForPopulationSufficiency += CheckForPopulationSufficiency;
    }

    private void OnDisable()
    {
        NeighborhoodEvents.OnCheckForPopulationSufficiency -= CheckForPopulationSufficiency;
    }

    private void CheckForPopulationSufficiency()
    {
        if (GameManager.Instance)
            ApplyBuildableState(EnoughPopulation);
    }

    private void ApplyBuildableState(bool buildable)
    {
        if (buildable)
        {
            _textHandler.SetConsumedMoneyText(_consumedMoney);
            _textHandler.SetRequiredMoneyText(cost);
            //_textHandler.MakePopulationTextEmpty();
            _textHandler.DisablePopulationText();
        }
        else
        {
            _textHandler.SetConsumedMoneyText(_consumedMoney);
            _textHandler.SetRequiredMoneyText(cost);
            _textHandler.SetPopulationText(RequiredPopulation);
        }
    }

    public void ConsumeMoney(int amount)
    {
        _consumedMoney += amount;
        PhaseManager.CurrentlyConsumedMoney = _consumedMoney;
        _textHandler.SetConsumedMoneyText(_consumedMoney);
    }

    public void EnableNextPhase()
    {
        NeighborhoodEvents.OnEnableThisPhase?.Invoke(this, phaseToUnlock);
        ObjectPooler.Instance.SpawnFromPool("Confetti", transform.position, Quaternion.identity);

        AnalyticEvents.OnPhaseUnlocked?.Invoke();
    }
}
