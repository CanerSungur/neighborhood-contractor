using UnityEngine;

public interface IRentable 
{
    GameObject BuildingPopulation { get; }
    public int CurrentBuildingPopulation { get; }
    public int MaxBuildingPopulation { get; }
}
