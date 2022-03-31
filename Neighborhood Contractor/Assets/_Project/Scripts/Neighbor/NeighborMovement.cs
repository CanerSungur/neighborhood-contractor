using UnityEngine;
using ZestGames.AI;

[RequireComponent(typeof(Neighbor))]
public class NeighborMovement : MonoBehaviour
{
    private Neighbor neighbor;
    public Neighbor Neighbor => neighbor == null ? neighbor = GetComponent<Neighbor>() : neighbor;

    private void Update()
    {
        if (Neighbor.CanMove)
        {
            Movement.MoveTransform(transform, Neighbor.TargetPosition, Neighbor.Speed);
            Movement.LookAtTarget(transform, Neighbor.TargetPosition);
        }
    }
}
