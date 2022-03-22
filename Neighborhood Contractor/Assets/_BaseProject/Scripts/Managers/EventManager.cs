using System;

public static class EventManager { }

public static class GameEvents
{
    public static Action OnGameStart, OnGameEnd, OnPlatformEnd, OnLevelSuccess, OnLevelFail, OnChangeScene, OnCalculateReward, OnChangePhase;
}

public static class CollectableEvents
{
    public static Action<int> OnIncreaseMoney, OnDecreaseMoney;
}

public static class PlayerEvents
{
    public static Action OnStartedMoving, OnStoppedMoving, OnKill, OnJump, OnLand, OnCollectedMoney, OnSpendMoney;
}
