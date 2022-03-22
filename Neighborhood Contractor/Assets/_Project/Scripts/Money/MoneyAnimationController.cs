using UnityEngine;

[RequireComponent(typeof(Money))]
public class MoneyAnimationController : MonoBehaviour
{
    private Money _money;

    private readonly int movingID = Animator.StringToHash("Moving");

    private void OnEnable()
    {
        _money = GetComponent<Money>();
        PlayerEvents.OnStartedMoving += StartMoving;
        PlayerEvents.OnStoppedMoving += StopMoving;
    }

    private void OnDisable()
    {
        PlayerEvents.OnStartedMoving -= StartMoving;
        PlayerEvents.OnStoppedMoving -= StopMoving;
    }

    private void StartMoving() => _money.Animator.SetBool(movingID, true);
    private void StopMoving() => _money.Animator.SetBool(movingID, false);
}
