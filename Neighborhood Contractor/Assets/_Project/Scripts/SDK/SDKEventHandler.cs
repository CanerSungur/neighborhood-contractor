using UnityEngine;

public class SDKEventHandler : MonoBehaviour
{
    private void Start()
    {
        //AnalyticEvents.OnBuiltFinished += BuiltFinished;
        //AnalyticEvents.OnUpgradeFinished += HouseUpgraded;
        //AnalyticEvents.OnPhaseUnlocked += PhaseUnlocked;

        AnalyticEvents.OnPhaseStarted += PhaseStarted;
        AnalyticEvents.OnPhaseFinished += PhaseFinished;
        AnalyticEvents.OnPhaseLoaded += PhaseLoaded;
    }

    private void OnDisable()
    {
        //AnalyticEvents.OnBuiltFinished -= BuiltFinished;
        //AnalyticEvents.OnUpgradeFinished -= HouseUpgraded;
        //AnalyticEvents.OnPhaseUnlocked -= PhaseUnlocked;

        AnalyticEvents.OnPhaseStarted -= PhaseStarted;
        AnalyticEvents.OnPhaseFinished -= PhaseFinished;
        AnalyticEvents.OnPhaseLoaded -= PhaseLoaded;
    }

    // There is no level fail.
    private void PhaseLoaded(int phase)
    {
        // TODO: Phase or Level is loaded


#if UNITY_EDITOR
        Debug.Log("Loaded CurrentPhase: " + PhaseManager.CurrentPhase);
#endif
    }

    private void PhaseFinished(int phase)
    {
        // TODO: Phase or Level is finished successfully.


#if UNITY_EDITOR
        Debug.Log("Finished CurrentPhase: " + PhaseManager.CurrentPhase);
#endif
    }

    private void PhaseStarted(int phase)
    {
        // TODO: Phase or Level is started.


#if UNITY_EDITOR
        Debug.Log("Started CurrentPhase: " + PhaseManager.CurrentPhase);
#endif
    }

    //private void BuiltFinished() { }
    //private void HouseUpgraded() { }
    //private void PhaseUnlocked() { }
}
