using UnityEngine;
using TMPro;

public class FeedbackUI : MonoBehaviour
{
    public enum Colors
    {
        NotEnoughPopulation,
        NotEnoughMoney,
        ValueLevelIncrease
    }

    private TextMeshProUGUI _feedbackText;
    private Color notEnoughPopulationColor = Color.cyan;
    private Color notEnoughMoneyColor = Color.magenta;
    private Color valueLevelIncrease = Color.green;

    private void Awake()
    {
        _feedbackText = transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
        _feedbackText.transform.parent.gameObject.SetActive(false);
    }

    private void Start()
    {
        FeedbackEvents.OnGiveFeedback += GiveFeedback;
    }

    private void OnDisable()
    {
        FeedbackEvents.OnGiveFeedback -= GiveFeedback;
    }

    private void GiveFeedback(string message, Colors color)
    {
        if (color == Colors.NotEnoughPopulation)
            _feedbackText.color = notEnoughPopulationColor;
        else if (color == Colors.NotEnoughMoney)
            _feedbackText.color = notEnoughMoneyColor;
        else if (color == Colors.ValueLevelIncrease)
            _feedbackText.color = valueLevelIncrease;

        _feedbackText.text = message;
        _feedbackText.transform.parent.gameObject.SetActive(false);
        _feedbackText.transform.parent.gameObject.SetActive(true);
        _feedbackText.transform.parent.transform.localEulerAngles = new Vector3(0, 0, Random.Range(-5f, 5f));
    }
}
