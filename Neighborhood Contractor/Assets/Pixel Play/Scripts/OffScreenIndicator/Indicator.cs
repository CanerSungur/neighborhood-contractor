using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Assign this script to the indicator prefabs.
/// </summary>
public class Indicator : MonoBehaviour
{
    public AccidentHandler AccidentHandler;
    [SerializeField] private IndicatorType indicatorType;
    private Image indicatorImage;
    private Text distanceText;

    [Header("-- SETUP --")]
    [SerializeField] private GameObject fireImg;
    [SerializeField] private GameObject floodImg;

    /// <summary>
    /// Gets if the game object is active in hierarchy.
    /// </summary>
    public bool Active
    {
        get
        {
            return transform.gameObject.activeInHierarchy;
        }
    }

    /// <summary>
    /// Gets the indicator type
    /// </summary>
    public IndicatorType Type
    {
        get
        {
            return indicatorType;
        }
    }

    void Awake()
    {
        indicatorImage = transform.GetComponent<Image>();
        distanceText = transform.GetComponentInChildren<Text>();
    }

    /// <summary>
    /// Sets the image color for the indicator.
    /// </summary>
    /// <param name="color"></param>
    public void SetImageColor(Color color)
    {
        indicatorImage.color = color;
    }

    /// <summary>
    /// Sets the distance text for the indicator.
    /// </summary>
    /// <param name="value"></param>
    public void SetDistanceText(float value)
    {
        distanceText.text = value >= 0 ? Mathf.Floor(value) + " m" : "";
    }

    /// <summary>
    /// Sets the distance text rotation of the indicator.
    /// </summary>
    /// <param name="rotation"></param>
    public void SetTextRotation(Quaternion rotation)
    {
        distanceText.rectTransform.rotation = rotation;
    }

    /// <summary>
    /// Sets the indicator as active or inactive.
    /// </summary>
    /// <param name="value"></param>
    public void Activate()
    {
        transform.gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        transform.gameObject.SetActive(false);
    }

    public void SetAccidentFire()
    {
        fireImg.SetActive(true);
        floodImg.SetActive(false);
        Debug.Log("Fire Indicator");
    }

    public void SetAccidentFlood()
    {
        fireImg.SetActive(false);
        floodImg.SetActive(true);
        Debug.Log("Flood Indicator");
    }


    private void LateUpdate()
    {
        if (AccidentHandler && AccidentHandler.CurrentAccident == AccidentHandler.Accident.Fire)
            SetAccidentFire();
        else
            SetAccidentFlood();
    }
}

public enum IndicatorType
{
    BOX,
    ARROW
}
