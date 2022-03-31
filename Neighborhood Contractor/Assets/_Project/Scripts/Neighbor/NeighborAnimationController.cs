using UnityEngine;

[RequireComponent(typeof(Neighbor))]
public class NeighborAnimationController : MonoBehaviour
{
    private Neighbor neighbor;
    public Neighbor Neighbor => neighbor == null ? neighbor = GetComponent<Neighbor>() : neighbor;

    private readonly int movingID = Animator.StringToHash("Moving");

    public void Init()
    {
        Neighbor.Animator.SetBool(movingID, false);
        Neighbor.OnStartMoving += StartMoving;
    }

    private void OnDisable()
    {
        Neighbor.OnStartMoving -= StartMoving;
    }

    private void StartMoving() => Neighbor.Animator.SetBool(movingID, true);
}
