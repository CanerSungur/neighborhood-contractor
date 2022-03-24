using UnityEngine;

public class NeighborhoodManager : MonoBehaviour
{
    public int Value { get; private set; }
    public int Population { get; private set; }

    private void Start()
    {
        Population = Value = 0;
        Debug.Log("Population: " + Population);
        Debug.Log("Value: " + Value);

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
        Value += amount;
        Debug.Log("Value: " + Value);
    }
}
