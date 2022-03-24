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
    public static Action OnCheckForPopulationSufficiency;
    public static Action<int> OnIncreaseValue, OnIncreasePopulation;
}

public static class FeedbackEvents
{
    public static Action<string, FeedbackUI.Colors> OnGiveFeedback;
}

public static class BuildingUpgradeEvents
{
    public static Action<IBuilding> OnActivateBuildingUpgradeUI;
}
