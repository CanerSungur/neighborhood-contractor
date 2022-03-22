using UnityEngine;
using TMPro;

/// <summary>
/// Dependant to HUD UI Script.
/// This script only triggers reward animation, shows rewarded value if reward is activated.
/// </summary>
public class LevelEndRewardUI : MonoBehaviour
{
    private HUDUI hudUI;
    public HUDUI HUDUI { get { return hudUI == null ? hudUI = GetComponentInParent<HUDUI>() : hudUI; } }

    [Header("-- SETUP --")]
    private TextMeshProUGUI rewardMoneyText;
    private readonly int startID = Animator.StringToHash("Start");
    private readonly int amountID = Animator.StringToHash("Amount");

    private void OnEnable()
    {
        if (GameManager.GameState != GameState.Finished) return;
        GameEvents.OnCalculateReward += TriggerReward;
    }

    private void OnDisable()
    {
        if (GameManager.GameState != GameState.Finished) return;
        GameEvents.OnCalculateReward -= TriggerReward;
    }

    private void TriggerReward()
    {
        rewardMoneyText = transform.GetChild(transform.childCount - 1).GetComponent<TextMeshProUGUI>();
        rewardMoneyText.text = "+" + HUDUI.UIManager.GameManager.statManager.RewardMoney;

        HUDUI.Animator.enabled = true;
        HUDUI.Animator.SetTrigger(startID);
    }
}
