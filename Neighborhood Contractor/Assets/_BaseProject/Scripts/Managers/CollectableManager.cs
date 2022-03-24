using UnityEngine;
using System;
using System.Collections;

[RequireComponent(typeof(GameManager))]
public class CollectableManager : MonoBehaviour
{
    private GameManager gameManager;
    public GameManager GameManager { get { return gameManager == null ? gameManager = GetComponent<GameManager>() : gameManager; } }

    private Player player;
    public Player Player => player == null ? player = FindObjectOfType<Player>() : player;

    [Header("-- REWARD SETUP --")]
    [SerializeField, Tooltip("Object that will be spawned as reward when an object is destroyed.")] private GameObject coinRewardPrefab;
    [SerializeField, Tooltip("Offset relative to the destroyed object's position.")] private float spawnPointOffset = 2.75f;
    public Transform CoinHUDTransform => GameManager.uiManager.MoneyHUDTransform;

    public static event Action<Vector3, int> OnSpawnCoinRewards;

    private void Start()
    {
        OnSpawnCoinRewards += SpawnCoinRewards;
    }

    private void OnDisable()
    {
        OnSpawnCoinRewards -= SpawnCoinRewards;
    }

    private void SpawnCoinRewards(Vector3 spawnPosition, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            Instantiate(coinRewardPrefab,spawnPosition + (Vector3.up * spawnPointOffset), Quaternion.identity);
        }
    }

    public void SpawnCoinRewardsTrigger(Vector3 spawnPosition, int amount) => OnSpawnCoinRewards?.Invoke(spawnPosition, amount);

    public void StartCollectingIncome(IContributorIncome incomeBuilding, IBuilding building) => StartCoroutine(Collect(incomeBuilding, building));
    public void StopCollectingIncome(IContributorIncome incomeBuilding, IBuilding building) => StopCoroutine(Collect(incomeBuilding, building));
    private IEnumerator Collect(IContributorIncome incomeBuilding, IBuilding building)
    {
        while (building.PlayerIsInBuildArea)
        {
            if (incomeBuilding.IncomeMoneyCount > 0 && incomeBuilding.CanCollectIncome && building.Built)
            {
                Money money = incomeBuilding.IncomeMoney[incomeBuilding.IncomeMoney.Count - 1];
                if (money.CanBeCollected)
                {
                    money.Collect(Player.moneyStackHandler.TargetStackPosition, Player.moneyStackHandler.StackTransform);
                    incomeBuilding.IncomeMoneyIsSent(money);

                    //StatManager.CollectedMoney[StatManager.CollectedMoney.Count - 1].Spend(building.MoneyPointTransform);
                    Player.CollectMoney(StatManager.MoneyValue);
                }
            }

            yield return new WaitForSeconds(StatManager.TakeIncomeTime);
        }
    }
}
