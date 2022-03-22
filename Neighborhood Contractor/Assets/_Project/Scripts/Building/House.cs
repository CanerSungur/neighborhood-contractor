using UnityEngine;

public class House : MonoBehaviour, IBuilding
{
    private BuildingTextHandler _textHandler;

    [Header("-- APPEREANCE SETUP --")]
    [SerializeField] private Material constructionMat;
    [SerializeField] private Material finishedMat;
    private Renderer _renderer;

    [Header("-- BUILD SETUP --")]
    [SerializeField] private int cost = 10000;
    [SerializeField] private Transform moneyPointTransform;
    private int _consumedMoney = 0;

    public bool PlayerIsInBuildArea { get; set; }
    public int Cost => cost;
    public bool CanBeBuilt => StatManager.CurrentCarry > 0 && !Builded;
    public bool Builded => _consumedMoney == cost;
    public int ConsumedMoney => _consumedMoney;
    public Transform BuildArea { get; private set; }
    public Transform MoneyPointTransform => moneyPointTransform;

    private void OnEnable()
    {
        _textHandler = GetComponent<BuildingTextHandler>();
        _textHandler.SetMoneyText(cost);

        BuildArea = transform.GetChild(0);
        PlayerIsInBuildArea = false;

        _renderer = GetComponent<Renderer>();
        _renderer.material = constructionMat;
    }

    public void ConsumeMoney(int amount)
    {
        _consumedMoney += amount;
        _textHandler.SetMoneyText(cost - _consumedMoney);
    }

    public void FinishBuilding()
    {
        _renderer.material = finishedMat;
        _textHandler.DisableMoneyText();
    }
}
