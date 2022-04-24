using System;
using UnityEngine;

public static class EventManager { }

public static class GameEvents
{
    public static Action OnGameStart, OnGameEnd, OnPlatformEnd, OnLevelSuccess, OnLevelFail, OnChangeScene, OnCalculateReward, OnChangePhase;
}

public static class CollectableEvents
{
    public static Action OnCalculateMoveWeight;
    public static Action<int> OnIncreaseMoney, OnDecreaseMoney;
}

public static class PlayerEvents
{
    public static Action OnStartedMoving, OnStoppedMoving, OnKill, OnJump, OnLand, OnCollectedMoney, OnSpendMoney;
}

public static class NeighborhoodEvents
{
    public static Action OnCheckForPopulationSufficiency, OnNewPhaseActivated;
    public static Action<int> OnIncreaseValue, OnIncreasePopulation;
    public static Action<PhaseUnlocker, int> OnEnableThisPhase;
    public static Action<Building> OnBuildingRepaired, OnBuildingFinished, OnBuildingUpgraded, OnAccidentHappened;
}

public static class FeedbackEvents
{
    public static Action<string, FeedbackUI.Colors> OnGiveFeedback;
}

public static class BuildingUpgradeEvents
{
    public static Action<Upgradeable> OnActivateUpgradeUI, OnCloseUpgradeUI;
}

public static class ValueBarEvents
{
    public static Action OnValueLevelIncrease, OnValueIncrease;
}

public static class AnalyticEvents
{
    public static Action OnBuiltFinished, OnUpgradeFinished, OnPhaseUnlocked;
    public static Action<int> OnPhaseLoaded, OnPhaseFinished, OnPhaseStarted;
}

public static class AccidentEvents
{
    public static Action<Building> OnFireStarted, OnFloodStarted;
}
