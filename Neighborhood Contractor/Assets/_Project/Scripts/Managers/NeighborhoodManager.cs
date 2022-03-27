using UnityEngine;

public class NeighborhoodManager : MonoBehaviour
{
    [Header("-- DEFAULT VALUES --")]
    private int _currentValue = 0;
    private int _valueLevel = 1;

    public static ValueSystem ValueSystem { get; private set; }
    public static int Population { get; private set; }

    private void Init()
    {
        ValueSystem = new ValueSystem(_currentValue, _valueLevel);
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
    }
    private void IncreaseTotalValue(int amount)
    {
        ValueSystem.AddValue(amount);
    }
}
