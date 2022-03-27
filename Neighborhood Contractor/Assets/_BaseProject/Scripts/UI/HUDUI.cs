using UnityEngine;
using TMPro;
using System;
using DG.Tweening;
using ZestGames.Utility;

public class HUDUI : MonoBehaviour
{
    private UIManager uiManager;
    public UIManager UIManager { get { return uiManager == null ? uiManager = FindObjectOfType<UIManager>() : uiManager; } }

    private Animator animator;
    public Animator Animator { get { return animator == null ? animator = GetComponent<Animator>() : animator; } }

    [Header("-- TEXT REFERENCES --")]
    [SerializeField] private TextMeshProUGUI currentMoneyText;
    [SerializeField] private TextMeshProUGUI capacityMoneyText;
    [SerializeField] private TextMeshProUGUI populationText;
    private Transform populationTransform;
    private TextMeshProUGUI[] hudTexts;

    [Header("-- MONEY SETUP --")]
    [SerializeField] private Transform moneyHUDTransform;
    public Transform MoneyHUDTransform => moneyHUDTransform;

    public event Action<int> OnUpdateMoneyUI;
    public event Action<int> OnUpdateLevelUI;

    private void OnEnable()
    {
        Animator.enabled = false;
        populationTransform = populationText.transform.parent;

        hudTexts = GetComponentsInChildren<TextMeshProUGUI>();
        capacityMoneyText.text = Shortener.IntToStringShortener(StatManager.MoneyCapacity);
        UpdatePopulationText();

        OnUpdateMoneyUI += UpdateMoneyText;
        NeighborhoodEvents.OnCheckForPopulationSufficiency += UpdatePopulationText;
    }

    private void OnDisable()
    {
        OnUpdateMoneyUI -= UpdateMoneyText;
        NeighborhoodEvents.OnCheckForPopulationSufficiency -= UpdatePopulationText;
    }

    public void UpdateMoneyUITrigger(int ignoreThis) => OnUpdateMoneyUI?.Invoke(ignoreThis);
    public void UpdateLevelUTrigger(int level) => OnUpdateLevelUI?.Invoke(level);

    private void UpdatePopulationText()
    {
        populationText.text = NeighborhoodManager.Population.ToString();
        ShakeTransform(populationTransform);
    }
    private void UpdateMoneyText(int ignoreThis)
    {
        currentMoneyText.text = Shortener.IntToStringShortener(StatManager.TotalMoney);

        ShakeTransform(MoneyHUDTransform);
        ChangeMoneyTextColor();
    }

    private void ShakeTransform(Transform transform)
    {
        transform.DORewind();

        transform.DOShakePosition(.25f, .25f);
        transform.DOShakeRotation(.5f, .25f);
        transform.DOShakeScale(.25f, .25f);
    }

    private void ChangeMoneyTextColor()
    {
        if (StatManager.TotalMoney == StatManager.MoneyCapacity)
        {
            foreach (TextMeshProUGUI text in hudTexts)
                text.color = Color.red;
        }
        else
        {
            foreach (TextMeshProUGUI text in hudTexts)
                text.color = Color.white;
        }
    }

    // Animation event listener.
    public void AlertObservers(string message)
    {
        if (message.Equals("RewardAnimEnded")) // Level success screen should trigger here.
            GameEvents.OnLevelSuccess?.Invoke();
        else if (message.Equals("UpdateCoinAfterReward"))
            UpdateMoneyUITrigger(StatManager.TotalMoney);
    }
}
