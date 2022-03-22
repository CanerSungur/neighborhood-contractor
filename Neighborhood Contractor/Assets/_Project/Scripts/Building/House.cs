using UnityEngine;

public class House : MonoBehaviour, IBuilding
{
    [Header("-- SETUP --")]
    [SerializeField] private int cost = 10000;
    [SerializeField] private Transform moneyPointTransform;
    private int _consumedMoney = 0;

    public bool PlayerIsInBuildArea { get; set; }
    public int Cost => cost;
    public bool Builded => _consumedMoney == cost;
    public int ConsumedMoney => _consumedMoney;
    public Transform BuildArea { get; private set; }
    public Transform MoneyPointTransform => moneyPointTransform;

    private void OnEnable()
    {
        BuildArea = transform.GetChild(0);
        PlayerIsInBuildArea = false;
    }

    public void ConsumeMoney(int amount, float rate)
    {
        if (!Builded && PlayerIsInBuildArea)
        {
            // Consume money
           // increase consumedMoney
        }
    }
}
