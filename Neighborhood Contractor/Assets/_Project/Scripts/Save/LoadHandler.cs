using UnityEngine;

public static class LoadHandler
{
    public static SavePlayerData LoadPlayerData()
    {
        int totalMoney = PlayerPrefs.GetInt("TotalMoney", 0);
        int currentCarry = PlayerPrefs.GetInt("CurrentCarry", 0);
        float spendTime = PlayerPrefs.GetFloat("SpendTime");
        float takeIncomeTime = PlayerPrefs.GetFloat("TakeIncomeTime");

        SavePlayerData saveData = new SavePlayerData()
        {
            TotalMoney = totalMoney,
            CurrentCarry = currentCarry,
            SpendTime = spendTime,
            TakeIncomeTime = takeIncomeTime,
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
}
