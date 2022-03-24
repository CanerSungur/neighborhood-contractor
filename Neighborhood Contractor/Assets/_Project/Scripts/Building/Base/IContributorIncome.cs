using System;
using System.Collections.Generic;

public interface IContributorIncome
{
    void IncomeMoneyIsSent(Money money);
    public int IncomeMoneyCount { get; }
    public bool CanCollectIncome { get; }
    public List<Money> IncomeMoney { get; }
    public event Action OnStartSpawningIncome;
}
