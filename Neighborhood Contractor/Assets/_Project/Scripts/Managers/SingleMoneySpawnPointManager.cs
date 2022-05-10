using UnityEngine;

public class SingleMoneySpawnPointManager : Singleton<SingleMoneySpawnPointManager>
{
    private SingleMoneySpawnPoint[] moneyPoints;
    public static bool FreeSpawnActive;

    private void Awake()
    {
        this.Reload();
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

    public void DisableFreeMoneySpawn()
    {
        //if (!FreeSpawnActive) return;
        //FreeSpawnActive = false;

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
