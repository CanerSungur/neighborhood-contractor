using UnityEngine;

public interface IBuilding
{
    public Transform MoneyPointTransform { get; }
    public bool PlayerIsInBuildArea { get; set; }
    public int BuildCost { get; }
    public int UpgradeCost { get; }
    public int NextLevelNumber { get; }
    public bool CanBeBuilt { get; }
    public bool CanBeUpgraded { get; }
    public bool Built { get; }
    public int ConsumedMoney { get; }
    void ConsumeMoney(int amount);
    void FinishBuilding();
    void UpgradeBuilding();
}
