using UnityEngine;
using ZestGames.AI;

[RequireComponent(typeof(Neighbor))]
public class NeighborMovement : MonoBehaviour
{
    private Neighbor _neighbor;
    private bool _wait, _targetReached, _complaintFinished;

    public void Init(Neighbor neighbor)
    {
        _neighbor = neighbor;
        _wait = _targetReached = _complaintFinished = false;

        _neighbor.OnSetTargetPos += UpdateTarget;
        _neighbor.OnStopComplaining += FinishComplaint;
    }

    private void OnDisable()
    {
        _neighbor.OnSetTargetPos -= UpdateTarget;
        _neighbor.OnStopComplaining -= FinishComplaint;
    }

    private void Update()
    {
        if (_neighbor.CanMove && !_wait)
        {
            Movement.MoveTransform(transform, _neighbor.TargetPosition, _neighbor.Speed);
            Movement.LookAtTarget(transform, _neighbor.TargetPosition);

            if (Movement.IsTargetReached(transform, _neighbor.TargetPosition, 1f))
            {
                if (_complaintFinished)
                {
                    _complaintFinished = false;
                    _neighbor.RelatedBuilding.Rentable.OnStopComplaint?.Invoke();
                    gameObject.SetActive(false);
                }
                
                if (!_targetReached)
                {
                    _wait = _targetReached = true;
                    _neighbor.OnStartComplaining?.Invoke();
                }
            }
        }
    }

    private void UpdateTarget(Vector3 ignoreThis)
    {
        _wait = _targetReached = false;
    }

    private void FinishComplaint()
    {
        _complaintFinished = true;
    }
}
