using UnityEngine;

public class NeighborhoodManager : MonoBehaviour
{
    private ValueSystem _valueSystem;

    [Header("-- DEFAULT VALUES --")]
    private int _currentValue = 0;
    private int _valueLevel = 1;
    private int _requiredValueForNextLevel = 50;

    public static int CurrentValue { get; private set; }
    public static int ValueLevel { get; private set; }
    public static int RequiredValueForNextLevel { get; private set; }
    public static int Population { get; private set; }

    private void Init()
    {
        _valueSystem = new ValueSystem(_currentValue, _valueLevel, _requiredValueForNextLevel);
        UpdateValueProperties();
    }

    private void Awake()
    {
        Init();
    }

    private void Start()
    {
        NeighborhoodEvents.OnIncreasePopulation += IncreaseTotalPopulation;
        NeighborhoodEvents.OnIncreaseValue += IncreaseTotalValue;
    }

    private void OnDisable()
    {
        NeighborhoodEvents.OnIncreasePopulation -= IncreaseTotalPopulation;
        NeighborhoodEvents.OnIncreaseValue -= IncreaseTotalValue;
    }

    private void IncreaseTotalPopulation(int amount)
    {
        Population += amount;
        Debug.Log("Population: " + Population);
    }
    private void IncreaseTotalValue(int amount)
    {
        _valueSystem.AddValue(amount);
        UpdateValueProperties();
    }

    private void UpdateValueProperties()
    {
        CurrentValue = _valueSystem.CurrentValue;
        ValueLevel = _valueSystem.ValueLevel;
        RequiredValueForNextLevel = _valueSystem.RequiredValueForNextLevel;
    }
}
