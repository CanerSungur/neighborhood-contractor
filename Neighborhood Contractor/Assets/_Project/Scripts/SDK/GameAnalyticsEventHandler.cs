using UnityEngine;
using GameAnalyticsSDK;

public class GameAnalyticsEventHandler : MonoBehaviour
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
        GameAnalytics.NewResourceEvent(GAResourceFlowType.Source, "BuildingFinished", 1, "Build", "1");
    }

    private void HouseUpgraded()
    {
        GameAnalytics.NewResourceEvent(GAResourceFlowType.Source, "HouseUpgraded", 1, "Upgrade", "2");
    }

    private void PhaseUnlocked()
    {
        GameAnalytics.NewResourceEvent(GAResourceFlowType.Source, "PhaseUnlocked", 1, "PhaseBuild", "3");
    }
}
