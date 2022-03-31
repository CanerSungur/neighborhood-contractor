using UnityEngine;

public class CarAnimationController : MonoBehaviour
{
    private Car _car;

    private readonly int movingID = Animator.StringToHash("Moving");

    private void OnEnable()
    {
        _car = GetComponent<Car>();

        _car.OnStartedMoving += StartMoving;
        _car.OnStartedIdling += StopMoving;
    }

    private void OnDisable()
    {
        _car.OnStartedMoving -= StartMoving;
        _car.OnStartedIdling -= StopMoving;
    }

    private void StartMoving()
    {
        _car.Animator.SetBool(movingID, true);
    }

    private void StopMoving()
    {
        _car.Animator.SetBool(movingID, false);
    }
}
