using UnityEngine;
using TMPro;

public class BuildingTextHandler : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI remainingMoney;

    public void SetMoneyText(int amount) => remainingMoney.text = amount.ToString();
    public void DisableMoneyText() => remainingMoney.gameObject.SetActive(false);
}
