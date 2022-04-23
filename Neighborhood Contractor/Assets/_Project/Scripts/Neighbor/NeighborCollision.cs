using UnityEngine;

[RequireComponent(typeof(Neighbor))]
public class NeighborCollision : MonoBehaviour
{
    private Neighbor _neighbor;

    public void Init(Neighbor neighbor)
    {
        _neighbor = neighbor;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.parent != null && collision.transform.parent.TryGetComponent(out House house) && _neighbor.CurrentType == Neighbor.Type.Happy /* && !house.Building.Rentable.BuildingIsFull*/)
        {
            // activate rentable.
            house.Building.Rentable.Rented();
            gameObject.SetActive(false);
        }
    }
}
