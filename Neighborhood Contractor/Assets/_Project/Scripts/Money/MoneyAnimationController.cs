using UnityEngine;
using System.Linq;

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
        _money = GetComponent<Money>();
        PlayerEvents.OnStartedMoving += StartMoving;
        PlayerEvents.OnStoppedMoving += StopMoving;

        CollectableEvents.OnCalculateMoveWeight += HandleMoveWeight;
    }

    private void OnDisable()
    {
        _money.Animator.Rebind();

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
        _weight = weightCurve.Evaluate((1f / StatManager.CarryRowLength) * _money.StackRowNumber);
        _money.Animator.SetLayerWeight(moveLayerID, _weight);
    }

    public bool GetStateInfo()
    {
        return _money.Animator.GetBool(movingID);
    }

    public float GetCurrentNormalizedTime()
    {
        return _money.Animator.GetCurrentAnimatorStateInfo(1).normalizedTime;
    }
}
