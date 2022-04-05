using UnityEngine;
using TMPro;
using ZestGames.Utility;

public class BuildingUpgradeUI : MonoBehaviour
{
    public static bool IsOpen = false;

    private Animator _animator;
    private readonly int openID = Animator.StringToHash("Open");
    private readonly int closeID = Animator.StringToHash("Close");

    [Header("-- SETUP --")]
    [SerializeField] private CustomButton exitButton;
    [SerializeField] private CustomButton upgradeButton;
    [SerializeField] private CustomButton cancelButton;

    [Header("-- TEXT SETUP --")]
    [SerializeField] private TextMeshProUGUI levelFromText;
    [SerializeField] private TextMeshProUGUI levelToText;
    [SerializeField] private TextMeshProUGUI valueIncreaseText;
    [SerializeField] private TextMeshProUGUI rentSpaceText;
    [SerializeField] private TextMeshProUGUI requiredMoneyText;

    private Upgradeable _activatedUpgradeable = null;

    private void Start()
    {
        IsOpen = false;
        _animator = GetComponent<Animator>();
        
        BuildingUpgradeEvents.OnActivateUpgradeUI += Activate;
        BuildingUpgradeEvents.OnCloseUpgradeUI += Close;

        upgradeButton.onClick.AddListener(UpgradeButtonClicked);
        exitButton.onClick.AddListener(ExitButtonClicked);
        cancelButton.onClick.AddListener(CancelAreaClicked);
    }

    private void OnDisable()
    {
        BuildingUpgradeEvents.OnActivateUpgradeUI -= Activate;
        BuildingUpgradeEvents.OnCloseUpgradeUI -= Close;

        upgradeButton.onClick.RemoveListener(UpgradeButtonClicked);
        exitButton.onClick.RemoveListener(ExitButtonClicked);
        cancelButton.onClick.RemoveListener(CancelAreaClicked);
    }

    private void CancelAreaClicked()
    {
        cancelButton.ClickTrigger(TriggerClosingAnim);
    }
    private void ExitButtonClicked() => exitButton.ClickTrigger(TriggerClosingAnim);
    private void UpgradeButtonClicked() => upgradeButton.ClickTrigger(() => {
        upgradeButton.interactable = false;
        TriggerClosingAnim();
        Delayer.DoActionAfterDelay(this, .5f, () => BuildManager.Instance.UpgradeBuilding(_activatedUpgradeable));
    });

    private void Activate(Upgradeable upgradeable)
    {
        _activatedUpgradeable = upgradeable;

        TriggerOpeningAnim();

        requiredMoneyText.text = Shortener.IntToStringShortener(_activatedUpgradeable.UpgradeCost);
        levelFromText.text = "Lvl " + (_activatedUpgradeable.NextLevelNumber - 1);
        levelToText.text = "Lvl " + _activatedUpgradeable.NextLevelNumber.ToString();
        valueIncreaseText.text = "+" + _activatedUpgradeable.Building.ContributionHandler.ValueContribution;
        rentSpaceText.text = "+" + _activatedUpgradeable.Building.Rentable.RentIncreaseCount;

        if (StatManager.TotalMoney >= _activatedUpgradeable.UpgradeCost)
            upgradeButton.SetInteractable();
        else
            upgradeButton.SetNotInteractable();
    }

    private void Close(Upgradeable upgradeable)
    {
        TriggerClosingAnim();
    }
    private void TriggerOpeningAnim()
    {
        _animator.SetTrigger(openID);
    }
        
    private void TriggerClosingAnim()
    {
        _animator.SetTrigger(closeID);
    }

    public void AlertObservers(string message)
    {
        if (message.Equals("Opening"))
        {
            IsOpen = true;
        }
        else if (message.Equals("Closing"))
        {
            IsOpen = false;
        }
    }
}
