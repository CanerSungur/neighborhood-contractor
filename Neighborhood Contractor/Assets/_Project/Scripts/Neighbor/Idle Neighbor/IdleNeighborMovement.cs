using UnityEngine;

public class IdleNeighborMovement : MonoBehaviour
{
    private IdleNeighbor _idleNeighbor;

    public void Init(IdleNeighbor idleNeighbor)
    {
        _idleNeighbor = idleNeighbor;

        _idleNeighbor.OnStartIdling += Stop;
        _idleNeighbor.OnStartWandering += Move;
        _idleNeighbor.OnStartRandomAction += Stop;
    }

    private void OnDisable()
    {
        _idleNeighbor.OnStartIdling -= Stop;
        _idleNeighbor.OnStartWandering -= Move;
        _idleNeighbor.OnStartRandomAction -= Stop;
    }

    private void Move()
    {
        
    }

    private void Stop()
    {

    }
}
