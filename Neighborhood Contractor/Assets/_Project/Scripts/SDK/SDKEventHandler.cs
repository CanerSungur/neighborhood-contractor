using UnityEngine;

public class SDKEventHandler : MonoBehaviour
{
    private void Start()
    {
        AnalyticEvents.OnBuiltFinished += BuiltFinished;
        AnalyticEvents.OnUpgradeFinished += HouseUpgraded;
        AnalyticEvents.OnPhaseUnlocked += PhaseUnlocked;
    }

    private void OnDisable()
    {
        AnalyticEvents.OnBuiltFinished -= BuiltFinished;
        AnalyticEvents.OnUpgradeFinished -= HouseUpgraded;
        AnalyticEvents.OnPhaseUnlocked -= PhaseUnlocked;
    }

    private void BuiltFinished()
    {

    }

    private void HouseUpgraded()
    {

    }

    private void PhaseUnlocked()
    {
        Debug.Log("Phase Unlock Event Triggered!");
    }
}
