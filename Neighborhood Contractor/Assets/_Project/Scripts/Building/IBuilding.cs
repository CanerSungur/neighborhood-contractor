using UnityEngine;

public interface IBuilding
{
    public Transform MoneyPointTransform { get; }
    public bool PlayerIsInBuildArea { get; set; }
    public int Cost { get; }
    public bool Builded { get; }
    public int ConsumedMoney { get; }
    void ConsumeMoney(int amount, float rate);
}