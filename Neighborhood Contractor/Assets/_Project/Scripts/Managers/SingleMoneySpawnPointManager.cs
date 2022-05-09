using UnityEngine;

public class SingleMoneySpawnPointManager : MonoBehaviour
{
    private SingleMoneySpawnPoint[] moneyPoints;
    public static bool FreeSpawnActive;

    private void Awake()
    {
        //FreeSpawnActive = true;
        //if (PhaseManager.CurrentPhase > 1)
        //    DisableFreeMoneySpawn();
        //if (BuildManager.Instance.BuildingCount >= 2)
        //{
        //    DisableFreeMoneySpawn();
        //}

        //NeighborhoodEvents.OnNewPhaseActivated += DisableFreeMoneySpawn;
        NeighborhoodEvents.OnDisableFreeMoneySpawn += DisableFreeMoneySpawn;
    }

    private void OnDisable()
    {
        //NeighborhoodEvents.OnNewPhaseActivated -= DisableFreeMoneySpawn;
        NeighborhoodEvents.OnDisableFreeMoneySpawn -= DisableFreeMoneySpawn;
    }

    private void DisableFreeMoneySpawn()
    {
        FreeSpawnActive = false;

        moneyPoints = GetComponentsInChildren<SingleMoneySpawnPoint>();
        foreach (SingleMoneySpawnPoint moneyPoint in moneyPoints)
            moneyPoint.gameObject.SetActive(false);

        //if (PhaseManager.CurrentPhase > 1)
        //{
        //    moneyPoints = GetComponentsInChildren<SingleMoneySpawnPoint>();
        //    foreach (SingleMoneySpawnPoint moneyPoint in moneyPoints)
        //        moneyPoint.gameObject.SetActive(false);
        //}
    }
}
