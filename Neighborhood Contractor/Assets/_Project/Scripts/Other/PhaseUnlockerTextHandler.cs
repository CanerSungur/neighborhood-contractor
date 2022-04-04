using UnityEngine;
using TMPro;

public class PhaseUnlockerTextHandler : MonoBehaviour
{
    [Header("-- SETUP --")]
    [SerializeField] private TextMeshProUGUI requiredMoney;
    [SerializeField] private TextMeshProUGUI consumedMoney;
    [SerializeField] private TextMeshProUGUI requiredPopulation;

    public void SetRequiredMoneyText(int amount) => requiredMoney.text = amount.ToString("#,##0") + "$";
    public void SetConsumedMoneyText(int amount) => consumedMoney.text = amount.ToString("#,##0") + "$";
    public void MakeMoneyTextEmpty() => consumedMoney.text = "";
    public void DisableMoneyText() => consumedMoney.gameObject.SetActive(false);
    public void SetPopulationText(int requiredPopulation) => this.requiredPopulation.text = requiredPopulation.ToString();
    public void MakePopulationTextEmpty() => requiredPopulation.text = "";
    public void DisablePopulationText() => requiredPopulation.transform.parent.gameObject.SetActive(false);

    // amount.ToString("#,##0.00") 10000 returns 10.000,00
    // amount.ToString("#,##0") 10000 return 10.000
}
