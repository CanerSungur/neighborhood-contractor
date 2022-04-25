using UnityEngine;
using DG.Tweening;

/// <summary>
/// Manages all the other managers. Holds game flow events.
/// If there will be a reward when level is finished, invoke OnCalculateReward instead of OnLevelSuccess.
/// </summary>
public class GameManager : Singleton<GameManager>
{
    public bool DeleteSaveGame = false;

    [Header("-- MANAGER REFERENCES --")]
    internal StatManager statManager;
    internal UIManager uiManager;
    internal LevelManager levelManager;
    internal CollectableManager collectableManager;
    internal NeighborhoodManager neighborhoodManager;

    public static GameState GameState { get; private set; }
    public static GameEnd GameEnd { get; private set; }

    public static bool IsSoundOn, IsVibrationOn;

    private void Awake()
    {
        this.Reload();
        DOTween.Init(true, true, LogBehaviour.Verbose).SetCapacity(48825, 312);
        Application.targetFrameRate = 240;

        IsSoundOn = IsVibrationOn = true;

        TryGetComponent(out statManager);
        TryGetComponent(out uiManager);
        TryGetComponent(out levelManager);
        TryGetComponent(out collectableManager);
        TryGetComponent(out neighborhoodManager);

        ChangeState(GameState.WaitingToStart);
    }

    private void Start()
    {
        GameEvents.OnGameStart += () => ChangeState(GameState.Started);
        GameEvents.OnGameEnd += () => ChangeState(GameState.Finished);
        GameEvents.OnLevelSuccess += LevelSuccess;
        GameEvents.OnLevelFail += LevelFail;
    }

    private void OnDisable()
    {
        GameEvents.OnGameStart -= () => ChangeState(GameState.Started);
        GameEvents.OnGameEnd -= () => ChangeState(GameState.Finished);
        GameEvents.OnLevelSuccess -= LevelSuccess;
        GameEvents.OnLevelFail -= LevelFail;

        if (DeleteSaveGame)
            PlayerPrefs.DeleteAll();
    }

    private void LevelSuccess()
    {
        GameEnd = GameEnd.Win;
        //AudioHandler.PlayAudio(AudioHandler.AudioType.Game_Win);
    }

    private void LevelFail()
    {
        GameEnd = GameEnd.Fail;
        //AudioHandler.PlayAudio(AudioHandler.AudioType.Game_Fail);
    }

    private void ChangeState(GameState newState)
    {
        if (GameState != newState) GameState = newState;
    }
}

public enum GameState
{
    WaitingToStart,
    Started,
    Paused,
    PlatformIsOver,
    Finished
}

public enum GameEnd { NotDecided, Fail, Win }
