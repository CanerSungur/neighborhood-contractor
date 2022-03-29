using UnityEngine;

public interface IBuildable
{
    public Transform MoneyPointTransform { get; }
    public bool PlayerIsInBuildArea { get; set; }
    public bool CanBeBuilt { get; }
    public int ConsumedMoney { get; }
    public int BuildCost { get; }
    public bool Built { get; }
    void ConsumeMoney(int amount);
    void FinishBuilding();
}
