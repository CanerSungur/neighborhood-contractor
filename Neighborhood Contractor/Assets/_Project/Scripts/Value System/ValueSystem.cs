
public class ValueSystem
{
    public int ValueLevel { get; private set; }
    public int CurrentValue { get; private set; }
    public int RequiredValueForNextLevel { get; private set; }

    public ValueSystem(int currentValue, int valueLevel, int requiredValueForNextLevel)
    {
        CurrentValue = currentValue;
        ValueLevel = valueLevel;
        RequiredValueForNextLevel = requiredValueForNextLevel;
    }

    public void AddValue(int amount)
    {
        CurrentValue += amount;
        ValueBarEvents.OnValueIncrease?.Invoke();
        if (CurrentValue >= RequiredValueForNextLevel)
        {
            // Enough value to level up
            ValueLevel++;
            CurrentValue -= RequiredValueForNextLevel;
            ValueBarEvents.OnValueLevelIncrease?.Invoke();
        }
    }
}
