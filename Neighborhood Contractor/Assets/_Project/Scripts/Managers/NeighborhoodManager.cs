using UnityEngine;

public class NeighborhoodManager : MonoBehaviour
{
    private SaveNeighborhoodData _saveNeighborhoodData;

    [Header("-- DEFAULT VALUES --")]
    private int _currentValue = 0;
    private int _valueLevel = 1;

    public int CurrentValue => _currentValue;
    public int ValueLevel => _valueLevel;
    public static ValueSystem ValueSystem { get; private set; }
    public static int Population { get; private set; }

    private void Init()
    {
        ValueSystem = new ValueSystem(_currentValue, _valueLevel);

        _saveNeighborhoodData = LoadHandler.LoadNeighborhoodData();
        ValueSystem.SetCurrentValue(_saveNeighborhoodData.Value);
        ValueSystem.SetCurrentValueLevel(_saveNeighborhoodData.ValueLevel);
        Population = _saveNeighborhoodData.Population;
        
    }

    private void Awake()
    {
        Init();
    }

    private void OnApplicationPause(bool pause)
    {
        SaveHandler.SaveNeighborhoodData(this);
    }

    private void OnApplicationQuit()
    {
        SaveHandler.SaveNeighborhoodData(this);
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
    }
    private void IncreaseTotalValue(int amount)
    {
        ValueSystem.AddValue(amount);
    }
}
