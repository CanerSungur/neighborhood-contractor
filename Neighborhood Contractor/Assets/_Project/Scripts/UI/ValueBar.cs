using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class ValueBar : MonoBehaviour
{
    [Header("-- SETUP --")]
    [SerializeField] private TextMeshProUGUI valueLevelText;
    [SerializeField, Tooltip("Custom gradient if you're not using images for filling. Leave it white when using images!")] private Gradient gradient;
    [SerializeField, Tooltip("Seconds that will take to update the progress bar.")] private float updateSpeedSeconds = 0.5f;
    private Slider _slider;
    private Image _fill;

    private void Init()
    {
        if (!_slider)
            _slider = GetComponent<Slider>();
        if (!_fill)
            _fill = transform.GetChild(1).GetComponent<Image>();

        _fill.color = gradient.Evaluate(1f);
        _slider.maxValue = NeighborhoodManager.ValueSystem.RequiredValueForNextLevel;
        _slider.value = NeighborhoodManager.ValueSystem.CurrentValue;

        SetValueLevelNumber(NeighborhoodManager.ValueSystem.ValueLevel);
        UpdateValue();
    }

    private void Start()
    {
        Init();

        ValueBarEvents.OnValueIncrease += UpdateValue;
        ValueBarEvents.OnValueLevelIncrease += ValueChanged;
    }

    private void OnDisable()
    {
        ValueBarEvents.OnValueIncrease -= UpdateValue;
        ValueBarEvents.OnValueLevelIncrease -= ValueChanged;
    }

    private void SetValueLevelNumber(int valueLevelNumber) => valueLevelText.text = valueLevelNumber.ToString();

    private void UpdateValue()
    {
        StartCoroutine(SmoothUpdateValueBar());
        _fill.color = gradient.Evaluate(_slider.normalizedValue);
    }

    private void ValueChanged()
    {
        //Init();
        _slider.maxValue = NeighborhoodManager.ValueSystem.RequiredValueForNextLevel;
        SetValueLevelNumber(NeighborhoodManager.ValueSystem.ValueLevel);
        UpdateValue();
    }

    private IEnumerator SmoothUpdateValueBar()
    {
        float preChange = _slider.value;
        float elapsed = 0f;
        while (elapsed < updateSpeedSeconds)
        {
            elapsed += Time.deltaTime;
            Mathf.Lerp(preChange, NeighborhoodManager.ValueSystem.CurrentValue, 0);

            _slider.value = Mathf.Lerp(preChange, NeighborhoodManager.ValueSystem.CurrentValue, elapsed / updateSpeedSeconds);

            //if (_slider.value == _slider.maxValue)
            //    ValueBarEvents.OnValueLevelIncrease?.Invoke();
            yield return null;
        }

        _slider.value = NeighborhoodManager.ValueSystem.CurrentValue;
    }
}
