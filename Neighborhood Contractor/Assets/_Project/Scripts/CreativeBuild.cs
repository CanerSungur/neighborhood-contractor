using System.Collections;
using UnityEngine;
using TMPro;

public class CreativeBuild : MonoBehaviour
{
    private Player _player;

    [SerializeField] private GameObject level_1;
    [SerializeField] private GameObject level_2;
    [SerializeField] private GameObject level_3;
    [SerializeField] private GameObject level_4;
    [SerializeField] private GameObject level_5;
    [SerializeField] private GameObject level_6;
    [SerializeField] private GameObject level_7;
    [SerializeField] private GameObject level_8;
    [SerializeField] private GameObject level_9;

    [Space]
    [Space]

    [SerializeField] private Transform moneyPointTransform;
    [SerializeField] private int cost;
    private int consumedMoney = 0;

    [Space]
    [Space]

    private WaitForSeconds _waitForSpendTime = new WaitForSeconds(.001f);

    [SerializeField] private TextMeshProUGUI requiredMoneyText;
    [SerializeField] private TextMeshProUGUI consumedMoneyText;

    [SerializeField] private GameObject buildArea;
    [SerializeField] private GameObject incomeArea;

    [SerializeField] private ParticleSystem confetti;

    public bool PlayerIsInBuildArea { get; set; }
    public bool Built => consumedMoney == cost;
    public bool CanBeBuilt => StatManager.CurrentCarry > 0 && !Built;

    private void Awake()
    {
        _player = FindObjectOfType<Player>();
    }

    private void Start()
    {
        requiredMoneyText.text = cost.ToString("#,##0") + "$";
        consumedMoneyText.text = consumedMoney.ToString("#,##0") + "$";
    }

    public void StartBuilding() => StartCoroutine(Build());
    public void StopBuilding() => StopCoroutine(Build());
    private IEnumerator Build()
    {
        while (PlayerIsInBuildArea && CanBeBuilt)
        {
            StatManager.CollectedMoney[StatManager.CollectedMoney.Count - 1].Spend(moneyPointTransform);
            _player.SpendMoney(StatManager.SpendValue);
            ConsumeMoney(StatManager.SpendValue);

            if (Built)
                FinishBuilding();
            yield return _waitForSpendTime;
        }
    }

    private void ConsumeMoney(int amount)
    {
        consumedMoney += amount;
        consumedMoneyText.text = consumedMoney.ToString("#,##0") + "$";

        UpdateBuildPhases();
    }

    private void UpdateBuildPhases()
    {

        switch (consumedMoney)
        {
            case 4000:
                level_1.SetActive(false);
                level_2.SetActive(true);
                FeedbackEvents.OnGiveFeedback?.Invoke("KEEP GOING", FeedbackUI.Colors.ValueLevelIncrease);
                CreativeCam.OnZoomOut?.Invoke();
                break;
            case 8000:
                level_2.SetActive(false);
                level_3.SetActive(true);
                FeedbackEvents.OnGiveFeedback?.Invoke("POPULATION INCREASED", FeedbackUI.Colors.NotEnoughMoney);
                CreativeCam.OnZoomOut?.Invoke();
                break;
            case 12000:
                level_3.SetActive(false);
                level_4.SetActive(true);
                FeedbackEvents.OnGiveFeedback?.Invoke("IT'S HAPPENING", FeedbackUI.Colors.ValueLevelIncrease);
                CreativeCam.OnZoomOut?.Invoke();
                break;
            case 16000:
                level_4.SetActive(false);
                level_5.SetActive(true);
                FeedbackEvents.OnGiveFeedback?.Invoke("GOOD", FeedbackUI.Colors.NotEnoughMoney);
                CreativeCam.OnZoomOut?.Invoke();
                break;
            case 20000:
                level_5.SetActive(false);
                level_6.SetActive(true);
                FeedbackEvents.OnGiveFeedback?.Invoke("NICE", FeedbackUI.Colors.NotEnoughPopulation);
                CreativeCam.OnZoomOut?.Invoke();
                break;
            case 24000:
                level_6.SetActive(false);
                level_7.SetActive(true);
                FeedbackEvents.OnGiveFeedback?.Invoke("GREAT", FeedbackUI.Colors.ValueLevelIncrease);
                CreativeCam.OnZoomOut?.Invoke();
                break;
            case 28000:
                level_7.SetActive(false);
                level_8.SetActive(true);
                FeedbackEvents.OnGiveFeedback?.Invoke("PERFECT", FeedbackUI.Colors.NotEnoughMoney);
                CreativeCam.OnZoomOut?.Invoke();
                break;
            case 32000:
                level_8.SetActive(false);
                level_9.SetActive(true);
                FeedbackEvents.OnGiveFeedback?.Invoke("BUILDING MAXXED!", FeedbackUI.Colors.NotEnoughPopulation);
                CreativeCam.OnMaxZoom?.Invoke();
                confetti.Play();
                break;
            default:
                break;
        }
    }

    private void FinishBuilding()
    {
        buildArea.SetActive(false);
    }
}
