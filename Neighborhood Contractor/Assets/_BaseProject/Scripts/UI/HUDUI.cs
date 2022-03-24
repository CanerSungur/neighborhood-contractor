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
    //[SerializeField] private TextMeshProUGUI levelText;
    private TextMeshProUGUI[] hudTexts;

    [Header("-- COIN SETUP --")]
    [SerializeField] private Transform moneyHUDTransform;
    public Transform MoneyHUDTransform => moneyHUDTransform;

    public event Action<int> OnUpdateMoneyUI;
    public event Action<int> OnUpdateLevelUI;

    private void OnEnable()
    {
        Animator.enabled = false;
        hudTexts = GetComponentsInChildren<TextMeshProUGUI>();
        //capacityMoneyText.text = StatManager.MoneyCapacity.ToString();
        capacityMoneyText.text = Shortener.IntToStringShortener(StatManager.MoneyCapacity);

        OnUpdateMoneyUI += UpdateMoneyText;
        //OnUpdateLevelUI += UpdateLevelText;
    }

    private void OnDisable()
    {
        OnUpdateMoneyUI -= UpdateMoneyText;
        //OnUpdateLevelUI -= UpdateLevelText;
    }

    public void UpdateMoneyUITrigger(int ignoreThis) => OnUpdateMoneyUI?.Invoke(ignoreThis);
    public void UpdateLevelUTrigger(int level) => OnUpdateLevelUI?.Invoke(level);
    //private void UpdateLevelText(int level)
    //{
    //    levelText.text = $"Level {level}";
    //}

    private void UpdateMoneyText(int ignoreThis)
    {
        //currentMoneyText.text = UIManager.GameManager.statManager.TotalMoney.ToString();
        currentMoneyText.text = Shortener.IntToStringShortener(StatManager.TotalMoney);

        ShakeMoneyHUD();
        ChangeMoneyTextColor();
    }

    private void ShakeMoneyHUD()
    {
        MoneyHUDTransform.DORewind();

        MoneyHUDTransform.DOShakePosition(.25f, .25f);
        MoneyHUDTransform.DOShakeRotation(.5f, .25f);
        MoneyHUDTransform.DOShakeScale(.25f, .25f);
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
