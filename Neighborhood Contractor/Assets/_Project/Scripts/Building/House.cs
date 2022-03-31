using UnityEngine;
using ZestGames.Utility;

public class House : MonoBehaviour
{
    private Building building;
    public Building Building => building == null ? building = GetComponent<Building>() : building;

    [Header("-- SETUP --")]
    [SerializeField] private Transform carTargetTransform;
    [SerializeField] private Transform carTurnTransform;

    public Vector3 TargetPosition => carTargetTransform.position;
    public Vector3 TurnPosition => carTurnTransform.position;
    //public bool MaxxedOut => Building.

    private void OnEnable()
    {
        Delayer.DoActionAfterDelay(this, 1f, Subscribe);
    }

    private void OnDisable()
    {
        Building.Buildable.OnBuildFinished -= StartTheCar;
        Building.Upgradeable.OnUpgradeHappened -= StartTheCar;
    }

    private void Subscribe()
    {
        Building.Buildable.OnBuildFinished += StartTheCar;
        Building.Upgradeable.OnUpgradeHappened += StartTheCar;
    }

    private void StartTheCar()
    {
        if (CarManager.GetTheNextFreeCar() != null)
            CarManager.GetTheNextFreeCar().StartTheCar(this);
        else
            Debug.Log("No available free car.");
    }
}
