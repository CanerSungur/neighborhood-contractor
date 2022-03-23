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

    //private void InitCollectables()
    //{
    //    ObjectPooler.Instance.SpawnFromPool("Coin", );
    //}

    private void SpawnCoinRewards(Vector3 spawnPosition, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            Instantiate(coinRewardPrefab,spawnPosition + (Vector3.up * spawnPointOffset), Quaternion.identity);
        }
    }

    public void SpawnCoinRewardsTrigger(Vector3 spawnPosition, int amount) => OnSpawnCoinRewards?.Invoke(spawnPosition, amount);

    public void StartCollectingIncome(IBuilding building) => StartCoroutine(Collect(building));
    public void StopCollectingIncome(IBuilding building) => StopCoroutine(Collect(building));
    private IEnumerator Collect(IBuilding building)
    {
        while (building.PlayerIsInBuildArea)
        {
            if (building.IncomeMoneyCount > 0 && building.CanCollectIncome && building.Builded)
            {
                Money money = building.IncomeMoney[building.IncomeMoney.Count - 1];
                if (money.CanBeCollected)
                {
                    money.Collect(Player.moneyStackHandler.TargetStackPosition, Player.moneyStackHandler.StackTransform);
                    building.IncomeMoneyIsSent(money);

                    //StatManager.CollectedMoney[StatManager.CollectedMoney.Count - 1].Spend(building.MoneyPointTransform);
                    Player.CollectMoney(StatManager.MoneyValue);
                }
            }

            yield return new WaitForSeconds(0.25f);
        }
    }
}
