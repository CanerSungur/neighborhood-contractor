using UnityEngine;
using TMPro;

public class BuildingTextHandler : MonoBehaviour
{
    [Header("-- BUILDING SETUP --")]
    [SerializeField] private TextMeshProUGUI consumedMoney;
    [SerializeField] private TextMeshProUGUI requiredMoney;
    [SerializeField] private TextMeshProUGUI requiredPopulation;

    [Header("-- INCOME SETUP --")]
    [SerializeField] private TextMeshProUGUI incomePerSecondText;

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
