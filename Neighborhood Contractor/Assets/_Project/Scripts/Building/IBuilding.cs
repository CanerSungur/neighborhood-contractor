using System.Collections.Generic;
using UnityEngine;

public interface IBuilding
{
    public Transform MoneyPointTransform { get; }
    public bool PlayerIsInBuildArea { get; set; }
    public int Cost { get; }
    public bool CanBeBuilt { get; }
    public bool Builded { get; }
    public int ConsumedMoney { get; }
    void ConsumeMoney(int amount);
    void FinishBuilding();

    void IncomeMoneyIsSent(Money money);
    public int IncomeMoneyCount { get; }
    public bool CanCollectIncome { get; }
    public List<Money> IncomeMoney { get; }
}
