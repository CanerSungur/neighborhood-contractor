using UnityEngine;
using ZestGames.Utility;

[RequireComponent(typeof(Money))]
public class MoneyAnimationController : MonoBehaviour
{
    private Money _money;

    private readonly int movingID = Animator.StringToHash("Moving");
    private readonly int moveLayerID = 1;
    private float _weight = 0f;
    [SerializeField] private AnimationCurve weightCurve;

    private void OnEnable()
    {
        //if (Player.Moving)
        //    StartMoving();
        //SetFirstState();

        _money = GetComponent<Money>();
        PlayerEvents.OnStartedMoving += StartMoving;
        PlayerEvents.OnStoppedMoving += StopMoving;

        CollectableEvents.OnCalculateMoveWeight += HandleMoveWeight;
    }

    private void OnDisable()
    {
        PlayerEvents.OnStartedMoving -= StartMoving;
        PlayerEvents.OnStoppedMoving -= StopMoving;

        CollectableEvents.OnCalculateMoveWeight -= HandleMoveWeight;
    }

    public void SetFirstState()
    {
        if (Player.Moving)
            StartMoving();
    }
    private void StartMoving() => _money.Animator.SetBool(movingID, true);
    private void StopMoving() => _money.Animator.SetBool(movingID, false);
    private void HandleMoveWeight()
    {
        //_money.Animator.Rebind();
        //SetFirstState();

        _weight = weightCurve.Evaluate((1f / StatManager.CurrentCarry) * _money.StackRowNumber);
        _money.Animator.SetLayerWeight(moveLayerID, _weight);
    }
}
