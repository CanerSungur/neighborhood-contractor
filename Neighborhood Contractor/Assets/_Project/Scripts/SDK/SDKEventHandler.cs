using UnityEngine;

public class SDKEventHandler : MonoBehaviour
{
    private void Start()
    {
        AnalyticEvents.OnBuiltFinished += BuiltFinished;
        AnalyticEvents.OnUpgradeFinished += HouseUpgraded;
        AnalyticEvents.OnPhaseUnlocked += PhaseUnlocked;

        AnalyticEvents.OnPhaseStarted += PhaseStarted;
        AnalyticEvents.OnPhaseFinished += PhaseFinished;
        AnalyticEvents.OnPhaseLoaded += PhaseLoaded;
    }

    private void OnDisable()
    {
        AnalyticEvents.OnBuiltFinished -= BuiltFinished;
        AnalyticEvents.OnUpgradeFinished -= HouseUpgraded;
        AnalyticEvents.OnPhaseUnlocked -= PhaseUnlocked;

        AnalyticEvents.OnPhaseStarted -= PhaseStarted;
        AnalyticEvents.OnPhaseFinished -= PhaseFinished;
        AnalyticEvents.OnPhaseLoaded -= PhaseLoaded;
    }

    private void PhaseLoaded(int phase)
    {

        //Debug.Log("Loaded " + phase + " phase");
        Debug.Log("Loaded CurrentPhase: " + PhaseManager.CurrentPhase);
    }

    private void PhaseFinished(int phase)
    {
        //Debug.Log("Finished " + phase + " phase");
        Debug.Log("Finished CurrentPhase: " + PhaseManager.CurrentPhase);
    }

    private void PhaseStarted(int phase)
    {
        //Debug.Log("Started " + phase + " phase");
        Debug.Log("Started CurrentPhase: " + PhaseManager.CurrentPhase);
    }



    private void BuiltFinished()
    {

    }

    private void HouseUpgraded()
    {

    }

    private void PhaseUnlocked()
    {
        //Debug.Log("Phase Unlock Event Triggered!");
    }
}
