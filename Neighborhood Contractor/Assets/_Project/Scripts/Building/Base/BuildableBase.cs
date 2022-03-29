using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildableBase : IBuildable
{
    [Header("-- BUILDABLE SETUP --")]
    [SerializeField] private Transform moneyPointTransform;
    [SerializeField] private int buildCost = 1000;
    private int _consumedMoney;

    public Transform MoneyPointTransform => moneyPointTransform;
    public bool PlayerIsInBuildArea { get; set; }
    public bool CanBeBuilt => !Built && StatManager.CurrentCarry > 0;
    public int ConsumedMoney => _consumedMoney;
    public int BuildCost => buildCost;
    public bool Built => _consumedMoney == buildCost;

    public void ConsumeMoney(int amount)
    {
        throw new System.NotImplementedException();
    }

    public void FinishBuilding()
    {
        throw new System.NotImplementedException();
    }
}
