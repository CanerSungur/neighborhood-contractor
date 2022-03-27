public class ValueSystem
{
    public int ValueLevel { get; private set; }
    public int CurrentValue { get; private set; }
    public int RequiredValueForNextLevel { get; private set; }

    public ValueSystem(int currentValue, int valueLevel)
    {
        CurrentValue = currentValue;
        ValueLevel = valueLevel;
        UpdateRequiredValueForNextLevel();
    }

    public void AddValue(int amount)
    {
        CurrentValue += amount;
        ValueBarEvents.OnValueIncrease?.Invoke();
        while (CurrentValue >= RequiredValueForNextLevel)
        {
            // Enough value to level up
            ValueLevel++;

            CurrentValue -= RequiredValueForNextLevel;
            UpdateRequiredValueForNextLevel();
            ValueBarEvents.OnValueLevelIncrease?.Invoke();
        }
    }

    private void UpdateRequiredValueForNextLevel()
    {
        RequiredValueForNextLevel = ValueLevel * 50;
    }
}
