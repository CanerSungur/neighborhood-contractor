using UnityEngine;

public static class SaveHandler
{
    // Things that will be saved:
    // Money that player has on its back.
    // Money that buildings have on their income areas
    // Building's build state. 
    // Building's upgrade state.
    // Game phase unlock state.
    // Consumed money of all things.
    // Value and population.

    public static void SavePlayerData(StatManager statManager)
    {
        PlayerPrefs.SetInt("TotalMoney", StatManager.TotalMoney);
        PlayerPrefs.SetInt("CurrentCarry", StatManager.CurrentCarry);
        PlayerPrefs.SetFloat("SpendTime", StatManager.SpendTime);
        PlayerPrefs.SetFloat("TakeIncomeTime", StatManager.TakeIncomeTime);
        PlayerPrefs.SetInt("CurrentCarryRow", StatManager.CurrentCarryRow);
        PlayerPrefs.SetInt("CurrentCarryColumn", StatManager.CurrentCarryColumn);

        PlayerPrefs.Save();
    }

    public static void SaveNeighborhoodData(NeighborhoodManager neighborhoodManager)
    {
        PlayerPrefs.SetInt("Value", NeighborhoodManager.ValueSystem.CurrentValue);
        PlayerPrefs.SetInt("ValueLevel", NeighborhoodManager.ValueSystem.ValueLevel);
        PlayerPrefs.SetInt("Population", NeighborhoodManager.Population);

        PlayerPrefs.Save();
    }

    public static void SavePhaseData(PhaseManager phaseManager)
    {
        PlayerPrefs.SetInt("CurrentPhase", PhaseManager.CurrentPhase);
        PlayerPrefs.SetInt("CurrentlyConsumedMoney", PhaseManager.CurrentlyConsumedMoney);

        PlayerPrefs.Save();
    }

    public static void SaveIncomeSpawnerData(IncomeSpawner incomeSpawner)
    {
        PlayerPrefs.SetInt($"MoneyCount_{incomeSpawner.Building.Index}", incomeSpawner.MoneyCount);

        PlayerPrefs.Save();
    }

    public static void DeleteAll()
    {
        PlayerPrefs.DeleteAll();
    }
}
