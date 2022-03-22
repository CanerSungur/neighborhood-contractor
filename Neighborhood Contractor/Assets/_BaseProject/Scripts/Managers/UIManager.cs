using UnityEngine;

[RequireComponent(typeof(GameManager))]
public class UIManager : MonoBehaviour
{
    private GameManager gameManager;
    public GameManager GameManager { get { return gameManager == null ? gameManager = GetComponent<GameManager>() : gameManager; } }

    [Header("-- UI REFERENCES --")]
    [SerializeField] private TouchToStartUI touchToStart;
    [SerializeField] private HUDUI hud;
    [SerializeField] private SettingsBasicUI settings;

    public Transform MoneyHUDTransform => hud.MoneyHUDTransform;

    private void Init()
    {
        settings.gameObject.SetActive(false);
        hud.gameObject.SetActive(false);
    }

    private void Awake()
    {
        Init();
    }

    private void Start()
    {
        GameEvents.OnGameStart += GameStarted;
        GameEvents.OnGameEnd += GameEnded;

        CollectableEvents.OnIncreaseMoney += hud.UpdateMoneyUITrigger;
    }

    private void OnDisable()
    {
        GameEvents.OnGameStart -= GameStarted;
        GameEvents.OnGameEnd -= GameEnded;

        CollectableEvents.OnIncreaseMoney -= hud.UpdateMoneyUITrigger;
    }

    private void GameStarted()
    {
        touchToStart.gameObject.SetActive(false);
        settings.gameObject.SetActive(true);
        hud.gameObject.SetActive(true);
        hud.UpdateMoneyUITrigger(GameManager.statManager.TotalMoney);
        hud.UpdateLevelUTrigger(GameManager.levelManager.Level);
    }

    private void GameEnded()
    {
        settings.gameObject.SetActive(false);
        hud.gameObject.SetActive(false);
    }

    // Functions for dependant classes
    public void StartGame() => GameEvents.OnGameStart?.Invoke();
    public void ChangeScene() => GameEvents.OnChangeScene?.Invoke();
}
