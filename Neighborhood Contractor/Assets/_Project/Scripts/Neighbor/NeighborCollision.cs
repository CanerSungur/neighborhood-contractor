using UnityEngine;

[RequireComponent(typeof(Neighbor))]
public class NeighborCollision : MonoBehaviour
{
    private Neighbor neighbor;
    public Neighbor Neighbor => neighbor == null ? neighbor = GetComponent<Neighbor>() : neighbor;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.parent != null && collision.transform.parent.TryGetComponent(out House house)/* && !house.Building.Rentable.BuildingIsFull*/)
        {
            // activate rentable.
            house.Building.Rentable.Rented();
            gameObject.SetActive(false);
        }
    }
}
