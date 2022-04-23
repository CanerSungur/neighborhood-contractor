using UnityEngine;

[RequireComponent(typeof(Neighbor))]
public class NeighborAnimationController : MonoBehaviour
{
    private Neighbor _neighbor;

    private readonly int movingID = Animator.StringToHash("Moving");
    private readonly int complainID = Animator.StringToHash("Complain");

    public void Init(Neighbor neighbor)
    {
        _neighbor = neighbor;

        _neighbor.Animator.SetBool(movingID, false);
        _neighbor.Animator.SetBool(complainID, false);

        _neighbor.OnStartMoving += StartMoving;
        _neighbor.OnStartComplaining += StartComplaining;
    }

    private void OnDisable()
    {
        _neighbor.OnStartMoving -= StartMoving;
        _neighbor.OnStartComplaining -= StartComplaining;
    }

    private void StartMoving()
    {
        _neighbor.Animator.SetBool(movingID, true);
        _neighbor.Animator.SetBool(complainID, false);
    }

    private void StartComplaining()
    {
        _neighbor.Animator.SetBool(complainID, true);
        _neighbor.Animator.SetBool(movingID, false);
    }
}
