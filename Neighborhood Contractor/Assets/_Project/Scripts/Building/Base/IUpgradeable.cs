using UnityEngine;

public interface IUpgradeable
{
    public Transform MoneyPointTransform { get; }
    public bool PlayerısInBuildArea { get; set; }
    public int UpgradeCost { get; }
    public bool CanBeUpgraded { get; }
    public int ConsumedMoney { get; }
    void ConsumeMoney(int amount);
    void UpgradeBuilding();
}
