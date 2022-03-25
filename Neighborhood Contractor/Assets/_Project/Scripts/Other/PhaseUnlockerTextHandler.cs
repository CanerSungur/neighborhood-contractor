using UnityEngine;
using TMPro;

public class PhaseUnlockerTextHandler : MonoBehaviour
{
    [Header("-- SETUP --")]
    [SerializeField] private TextMeshProUGUI remainingMoney;
    [SerializeField] private TextMeshProUGUI requiredPopulation;

    public void SetMoneyText(int amount) => remainingMoney.text = amount.ToString();
    public void MakeMoneyTextEmpty() => remainingMoney.text = "";
    public void DisableMoneyText() => remainingMoney.gameObject.SetActive(false);
    public void SetPopulationText(int requiredPopulation) => this.requiredPopulation.text = $"NEED {requiredPopulation} POPULATION TO UNLOCK";
    public void MakePopulationTextEmpty() => requiredPopulation.text = "";
    public void DisablePopulationText() => requiredPopulation.gameObject.SetActive(false);
}
