using UnityEngine;
using System;

/// <summary>
/// Attach this script to all the target game objects in the scene.
/// </summary>
[DefaultExecutionOrder(0)]
public class IndicatorTarget : MonoBehaviour
{
    private Building _building;
    private AccidentHandler _accidentHandler;
    public AccidentHandler AccidentHandler => _accidentHandler;

    [Space(5)]

    [Tooltip("Change this color to change the indicators color for this target")]
    [SerializeField] private Color targetColor = Color.red;

    [Tooltip("Select if box indicator is required for this target")]
    [SerializeField] private bool needBoxIndicator = true;

    [Tooltip("Select if arrow indicator is required for this target")]
    [SerializeField] private bool needArrowIndicator = true;

    [Tooltip("Select if distance text is required for this target")]
    [SerializeField] private bool needDistanceText = true;

    /// <summary>
    /// Please do not assign its value yourself without understanding its use.
    /// A reference to the target's indicator, 
    /// its value is assigned at runtime by the offscreen indicator script.
    /// </summary>
    [HideInInspector] public Indicator indicator;

    /// <summary>
    /// Gets the color for the target indicator.
    /// </summary>
    public Color TargetColor
    {
        get
        {
            return targetColor;
        }
    }

    /// <summary>
    /// Gets if box indicator is required for the target.
    /// </summary>
    public bool NeedBoxIndicator
    {
        get
        {
            return needBoxIndicator;
        }
    }

    /// <summary>
    /// Gets if arrow indicator is required for the target.
    /// </summary>
    public bool NeedArrowIndicator
    {
        get
        {
            return needArrowIndicator;
        }
    }

    /// <summary>
    /// Gets if the distance text is required for the target.
    /// </summary>
    public bool NeedDistanceText
    {
        get
        {
            return needDistanceText;
        }
    }

    /// <summary>
    /// On enable add this target object to the targets list.
    /// </summary>
    private void OnEnable()
    {
        if(OffScreenIndicator.TargetStateChanged != null)
        {
            OffScreenIndicator.TargetStateChanged.Invoke(this, true);
        }
    }

    /// <summary>
    /// On disable remove this target object from the targets list.
    /// </summary>
    private void OnDisable()
    {
        if(OffScreenIndicator.TargetStateChanged != null)
        {
            OffScreenIndicator.TargetStateChanged.Invoke(this, false);
        }

        // Addition
        AccidentEvents.OnFireStarted -= HandleFireStart;
        AccidentEvents.OnFloodStarted -= HandleFloodStart;
    }

    public void Init(AccidentCauser accidentCauser)
    {
        _building = accidentCauser.Building;
        _accidentHandler = accidentCauser.AccidentHandler;

        AccidentEvents.OnFireStarted += HandleFireStart;
        AccidentEvents.OnFloodStarted += HandleFloodStart;
    }

    private void HandleFireStart(Building building)
    {
        //if (building != _building) return;

        //if (_accidentHandler.CurrentAccident == AccidentHandler.Accident.Fire)
        //    indicator.SetAccidentFire
    }

    private void HandleFloodStart(Building building)
    {
        //if (building != _building) return;
        //indicator.OnFlood?.Invoke();
    }

    /// <summary>
    /// Gets the distance between the camera and the target.
    /// </summary>
    /// <param name="cameraPosition">Camera position</param>
    /// <returns></returns>
    public float GetDistanceFromCamera(Vector3 cameraPosition)
    {
        float distanceFromCamera = Vector3.Distance(cameraPosition, transform.position);
        return distanceFromCamera;
    }

    #region ADDITION

    private Camera _camera;

    private void Awake()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        if (indicator == null) return;

        if (GameManager.GameState != GameState.Started)
        {
            indicator.Activate();
            return;
        }

        if (IsTargetVisible(_camera, gameObject))
            indicator.Deactivate();
        else
            indicator.Activate();
    }

    private bool IsTargetVisible(Camera cam, GameObject go)
    {
        var planes = GeometryUtility.CalculateFrustumPlanes(cam);
        var point = go.transform.position;
        foreach (var plane in planes)
        {
            if (plane.GetDistanceToPoint(point) < 0)
                return false;
        }
        return true;
    }

    #endregion
}
