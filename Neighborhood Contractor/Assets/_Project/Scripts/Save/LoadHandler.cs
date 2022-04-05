using UnityEngine;

public static class LoadHandler
{
    public static SavePlayerData LoadPlayerData()
    {
        int totalMoney = PlayerPrefs.GetInt("TotalMoney", 0);
        int currentCarry = PlayerPrefs.GetInt("CurrentCarry", 0);
        float spendTime = PlayerPrefs.GetFloat("SpendTime");
        float takeIncomeTime = PlayerPrefs.GetFloat("TakeIncomeTime");
        int currentCarryRow = PlayerPrefs.GetInt("CurrentCarryRow", 0);
        int currentCarryColumn = PlayerPrefs.GetInt("CurrentCarryColumn", 0);

        SavePlayerData saveData = new SavePlayerData()
        {
            TotalMoney = totalMoney,
            CurrentCarry = currentCarry,
            SpendTime = spendTime,
            TakeIncomeTime = takeIncomeTime,
            CurrentCarryRow = currentCarryRow,
            CurrentCarryColumn = currentCarryColumn
        };

        return saveData;
    }

    public static SaveNeighborhoodData LoadNeighborhoodData()
    {
        int value = PlayerPrefs.GetInt("Value", 0);
        int valueLevel = PlayerPrefs.GetInt("ValueLevel", 1);
        int population = PlayerPrefs.GetInt("Population", 0);

        SaveNeighborhoodData saveData = new SaveNeighborhoodData()
        {
            Value = value,
            ValueLevel = valueLevel,
            Population = population
        };

        return saveData;
    }

    public static SavePhaseData LoadPhaseData()
    {
        int currentPhase = PlayerPrefs.GetInt("CurrentPhase", 1);
        int currentlyConsumedMoney = PlayerPrefs.GetInt("CurrentlyConsumedMoney", 0);

        SavePhaseData saveData = new SavePhaseData()
        {
            CurrentPhase = currentPhase,
            CurrentlyConsumedMoney = currentlyConsumedMoney
        };

        return saveData;
    }

    public static SaveIncomeSpawnerData LoadIncomeSpawnerData(IncomeSpawner incomeSpawner)
    {
        int moneyCount = PlayerPrefs.GetInt($"MoneyCount_{incomeSpawner.Building.Index}", 0);

        SaveIncomeSpawnerData saveData = new SaveIncomeSpawnerData()
        {
            MoneyCount = moneyCount
        };

        return saveData;
    }
}
