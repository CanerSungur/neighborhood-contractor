using UnityEngine;
using TMPro;

[RequireComponent(typeof(Building))]
public class TextHandler : MonoBehaviour
{
    private Building building;
    public Building Building => building == null ? building = GetComponent<Building>() : building;

    [Header("-- SETUP --")]
    [SerializeField] private TextMeshProUGUI consumedMoney;
    [SerializeField] private TextMeshProUGUI requiredMoney;
    [SerializeField] private TextMeshProUGUI requiredPopulation;
    [SerializeField] private TextMeshProUGUI incomePerSecondText;

    private void OnEnable()
    {
        
    }

    #region Building Functions

    public void SetRequiredMoneyText(int amount) => requiredMoney.text = amount.ToString("#,##0") + "$";
    public void SetConsumedMoneyText(int amount) => consumedMoney.text = amount.ToString("#,##0") + "$";
    public void DisableMoneyText() => consumedMoney.gameObject.SetActive(false);
    public void SetPopulationText(int requiredPopulation) => this.requiredPopulation.text = requiredPopulation.ToString();

    #endregion

    #region Income Functions

    public void SetIncomePerSecondText(float incomePerSecond) => incomePerSecondText.text = $"{incomePerSecond:#,##0}$";

    #endregion
}
