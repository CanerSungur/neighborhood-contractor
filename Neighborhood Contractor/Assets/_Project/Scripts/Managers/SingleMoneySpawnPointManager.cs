using UnityEngine;

public class SingleMoneySpawnPointManager : MonoBehaviour
{
    private SingleMoneySpawnPoint[] moneyPoints;

    private void Start()
    {
        if (PhaseManager.CurrentPhase > 1)
            DisableFreeMoneySpawn();

        NeighborhoodEvents.OnNewPhaseActivated += DisableFreeMoneySpawn;
    }

    private void OnDisable()
    {
        NeighborhoodEvents.OnNewPhaseActivated -= DisableFreeMoneySpawn;
    }

    private void DisableFreeMoneySpawn()
    {
        if (PhaseManager.CurrentPhase > 1)
        {
            moneyPoints = GetComponentsInChildren<SingleMoneySpawnPoint>();
            foreach (SingleMoneySpawnPoint moneyPoint in moneyPoints)
                moneyPoint.gameObject.SetActive(false);
        }
    }
}
