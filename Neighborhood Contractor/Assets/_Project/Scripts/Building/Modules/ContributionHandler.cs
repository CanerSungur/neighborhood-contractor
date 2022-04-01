using UnityEngine;

public class ContributionHandler : MonoBehaviour
{
    [Header("-- SETUP --")]
    [SerializeField] private int valueContribution = 20;
    [SerializeField] private int populationContribution = 2;

    public int ValueContribution => valueContribution;
    public int PopulationContribution => populationContribution;

    public void Init(Building building)
    {
        if (building.Rentable)
            populationContribution = 0;
    }

    public void TriggerContribution()
    {
        NeighborhoodEvents.OnIncreasePopulation?.Invoke(populationContribution);
        NeighborhoodEvents.OnIncreaseValue?.Invoke(valueContribution);
    }
    public void TriggerPopulationContribution() => NeighborhoodEvents.OnIncreasePopulation?.Invoke(populationContribution);
    public void TriggerValueContribution() => NeighborhoodEvents.OnIncreaseValue?.Invoke(valueContribution);
}
