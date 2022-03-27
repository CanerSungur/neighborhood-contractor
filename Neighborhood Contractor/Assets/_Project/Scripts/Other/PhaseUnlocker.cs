using System;
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

    private void Init()
    {
        _textHandler = GetComponent<PhaseUnlockerTextHandler>();
        _textHandler.SetMoneyText(cost);

        PlayerIsInBuildArea = false;
        _consumedMoney = 0;

        CheckForPopulationSufficiency();
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
            _textHandler.SetMoneyText(cost);
            _textHandler.MakePopulationTextEmpty();
        }
        else
        {
            _textHandler.SetPopulationText(RequiredPopulation);
            //_textHandler.MakeMoneyTextEmpty();
        }
    }

    public void ConsumeMoney(int amount)
    {
        _consumedMoney += amount;
        _textHandler.SetMoneyText(cost - _consumedMoney);
    }

    public void EnableNextPhase()
    {
        NeighborhoodEvents.OnEnableThisPhase?.Invoke(this, phaseToUnlock);
        ObjectPooler.Instance.SpawnFromPool("Confetti", transform.position, Quaternion.identity);
    }
}
