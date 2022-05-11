using UnityEngine;

public class IdleNeighborAnimController : MonoBehaviour
{
    private IdleNeighbor _idleNeighbor;

    private readonly int movingID = Animator.StringToHash("Moving");
    private readonly int actionID = Animator.StringToHash("Action");
    private readonly int actionNumberID = Animator.StringToHash("ActionNumber");
    private int _actionCount = 1;

    public void Init(IdleNeighbor idleNeighbor)
    {
        _idleNeighbor = idleNeighbor;

        _idleNeighbor.Animator.SetBool(movingID, false);
        _idleNeighbor.Animator.SetBool(actionID, false);

        _idleNeighbor.OnStartIdling += StartIdling;
        _idleNeighbor.OnStartWandering += StartMoving;
        _idleNeighbor.OnStartRandomAction += StartRandomAction;
    }

    private void OnDisable()
    {
        _idleNeighbor.OnStartIdling -= StartIdling;
        _idleNeighbor.OnStartWandering -= StartMoving;
        _idleNeighbor.OnStartRandomAction -= StartRandomAction;
    }

    private void StartIdling()
    {
        _idleNeighbor.Animator.SetBool(movingID, false);
        _idleNeighbor.Animator.SetBool(actionID, false);
    }

    private void StartMoving()
    {
        _idleNeighbor.Animator.SetBool(movingID, true);
        _idleNeighbor.Animator.SetBool(actionID, false);
    }

    private void StartRandomAction()
    {
        _idleNeighbor.Animator.SetBool(movingID, false);
        _idleNeighbor.Animator.SetBool(actionID, true);
        _idleNeighbor.Animator.SetInteger(actionNumberID, Random.Range(1, _actionCount + 1));
    }
}
