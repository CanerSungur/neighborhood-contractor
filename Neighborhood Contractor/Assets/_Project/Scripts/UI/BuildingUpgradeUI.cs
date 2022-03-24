using UnityEngine;
using TMPro;
using ZestGames.Utility;

public class BuildingUpgradeUI : MonoBehaviour
{
    private Animator _animator;
    private readonly int openID = Animator.StringToHash("Open");
    private readonly int closeID = Animator.StringToHash("Close");

    [Header("-- SETUP --")]
    [SerializeField] private TextMeshProUGUI requiredMoneyText;
    [SerializeField] private CustomButton exitButton;
    [SerializeField] private CustomButton upgradeButton;
    //private GameObject ui;

    private IBuilding _activatedBuilding = null;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        //TriggerClosinAnim();
        //ui = transform.GetChild(0).gameObject;
        //ui.SetActive(false);
        
        BuildingUpgradeEvents.OnActivateBuildingUpgradeUI += ActivateUI;

        upgradeButton.onClick.AddListener(UpgradeButtonClicked);
        exitButton.onClick.AddListener(ExitButtonClicked);
    }

    private void OnDisable()
    {
        BuildingUpgradeEvents.OnActivateBuildingUpgradeUI -= ActivateUI;

        upgradeButton.onClick.RemoveListener(UpgradeButtonClicked);
        exitButton.onClick.RemoveListener(ExitButtonClicked);
    }

    private void ExitButtonClicked() => exitButton.ClickTrigger(TriggerClosinAnim);
    private void UpgradeButtonClicked() => upgradeButton.ClickTrigger(() => {
        BuildManager.Instance.Upgrade(_activatedBuilding);
        TriggerClosinAnim();
    });

    private void ActivateUI(IBuilding building)
    {
        _activatedBuilding = building;

        TriggerOpeningAnim();
        requiredMoneyText.text = Shortener.IntToStringShortener(_activatedBuilding.UpgradeCost);

        if (StatManager.TotalMoney >= _activatedBuilding.UpgradeCost)
            upgradeButton.SetInteractable();
        else
            upgradeButton.SetNotInteractable();
    }

    private void TriggerOpeningAnim() => _animator.SetTrigger(openID);
    private void TriggerClosinAnim() => _animator.SetTrigger(closeID);
}
