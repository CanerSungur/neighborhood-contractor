using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using System;
using TMPro;

public class ValueBar : MonoBehaviour
{
    [Header("-- SETUP --")]
    [SerializeField] private TextMeshProUGUI valueLevelText;
    [SerializeField, Tooltip("Custom gradient if you're not using images for filling. Leave it white when using images!")] private Gradient gradient;
    [SerializeField, Tooltip("Seconds that will take to update the progress bar.")] private float updateSpeedSeconds = 0.5f;
    private Slider slider;
    private Image fill;

    // Events
    public event Action OnValueLevelIncrease, OnValueIncrease;

    private void Init()
    {
        if (!slider)
            slider = GetComponent<Slider>();
        if (!fill)
            fill = transform.GetChild(1).GetComponent<Image>();

        fill.color = gradient.Evaluate(1f);
        slider.maxValue = NeighborhoodManager.RequiredValueForNextLevel;
        slider.value = NeighborhoodManager.CurrentValue;

        SetValueLevelNumber(NeighborhoodManager.ValueLevel);
        UpdateValue();
    }

    private void OnEnable()
    {
        Init();

        ValueBarEvents.OnValueIncrease += UpdateValue;
        ValueBarEvents.OnValueLevelIncrease += ValueChanged;
        //OnValueIncrease += UpdateValue;
        //OnValueLevelIncrease += ValueChanged;
    }

    private void OnDisable()
    {
        ValueBarEvents.OnValueIncrease -= UpdateValue;
        ValueBarEvents.OnValueLevelIncrease -= ValueChanged;
        //OnValueIncrease -= UpdateValue;
        //OnValueLevelIncrease -= ValueChanged;
    }

    private void SetValueLevelNumber(int valueLevelNumber) => valueLevelText.text = valueLevelNumber.ToString();

    private void UpdateValue()
    {
        StartCoroutine(SmoothUpdateValueBar());
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }

    private void ValueChanged()
    {
        Init();
        UpdateValue();
    }

    private IEnumerator SmoothUpdateValueBar()
    {
        float preChange = slider.value;
        float elapsed = 0f;
        while (elapsed < updateSpeedSeconds)
        {
            elapsed += Time.deltaTime;
            Mathf.Lerp(preChange, NeighborhoodManager.CurrentValue, 0);

            slider.value = Mathf.Lerp(preChange, NeighborhoodManager.CurrentValue, elapsed / updateSpeedSeconds);
            yield return null;
        }

        slider.value = NeighborhoodManager.CurrentValue;
    }

    public void ValueLevelIncreaseTrigger() => OnValueLevelIncrease?.Invoke();
    public void ValueIncreaseTrigger() => OnValueIncrease?.Invoke();
}
