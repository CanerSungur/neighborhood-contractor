using UnityEngine;

public class NeighborhoodManager : MonoBehaviour
{
    private SaveNeighborhoodData _saveNeighborhoodData;

    [Header("-- DEFAULT VALUES --")]
    private int _currentValue = 0;
    private int _valueLevel = 1;
    private int _idleNeighborActivatedHouseLimit = 10;
    private int _currentActivatedIdleNeighborHouses = 0;

    public int CurrentValue => _currentValue;
    public int ValueLevel => _valueLevel;
    public static ValueSystem ValueSystem { get; private set; }
    public static int Population { get; private set; }
    public static bool CanActivateIdleNeighbors { get; private set; }

    private void Init()
    {
        ValueSystem = new ValueSystem(_currentValue, _valueLevel);

        _saveNeighborhoodData = LoadHandler.LoadNeighborhoodData();
        ValueSystem.SetCurrentValue(_saveNeighborhoodData.Value);
        ValueSystem.SetCurrentValueLevel(_saveNeighborhoodData.ValueLevel);
        Population = _saveNeighborhoodData.Population;

        _currentActivatedIdleNeighborHouses = 0;
        CanActivateIdleNeighbors = _currentActivatedIdleNeighborHouses < _idleNeighborActivatedHouseLimit;
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
        NeighborhoodEvents.OnRequestIdleNeighborActivation += IdleNeighborActivationRequest;
        NeighborhoodEvents.OnDeactivateIdleNeighbor += IdleNeighborDeactivation;
    }

    private void OnDisable()
    {
        NeighborhoodEvents.OnIncreasePopulation -= IncreaseTotalPopulation;
        NeighborhoodEvents.OnIncreaseValue -= IncreaseTotalValue;
        NeighborhoodEvents.OnRequestIdleNeighborActivation -= IdleNeighborActivationRequest;
        NeighborhoodEvents.OnDeactivateIdleNeighbor -= IdleNeighborDeactivation;
    }

    private void IncreaseTotalPopulation(int amount)
    {
        Population += amount;
    }
    private void IncreaseTotalValue(int amount)
    {
        ValueSystem.AddValue(amount);
    }
    private void IdleNeighborActivationRequest(Building building)
    {
        if (CanActivateIdleNeighbors)
            IdleNeighborActivationApprove(building);
        else
            Debug.Log("Max idle neighbors reached.");
    }

    private void IdleNeighborActivationApprove(Building building)
    {
        if (!CanActivateIdleNeighbors) return;
        
        NeighborhoodEvents.OnActivateIdleNeighbor?.Invoke(building);
        _currentActivatedIdleNeighborHouses++;
        CanActivateIdleNeighbors = _currentActivatedIdleNeighborHouses < _idleNeighborActivatedHouseLimit;
    }

    private void IdleNeighborDeactivation(Building building)
    {
        _currentActivatedIdleNeighborHouses--;
        if (_currentActivatedIdleNeighborHouses < 0)
            _currentActivatedIdleNeighborHouses = 0;

        CanActivateIdleNeighbors = _currentActivatedIdleNeighborHouses < _idleNeighborActivatedHouseLimit;
    }
}
