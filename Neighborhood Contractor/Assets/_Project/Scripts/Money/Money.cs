using UnityEngine;
using DG.Tweening;

public class Money : MonoBehaviour
{
    private bool _collected = false;
    private bool _spent = false;

    public bool CanBeCollected => !_collected && StatManager.CurrentCarry < StatManager.CarryCapacity;
    public bool CanBeSpent => !_spent && _collected;

    private void OnDisable()
    {
        transform.DOKill();
    }

    public void Collect(Vector3 position, Transform parent)
    {
        transform.parent = parent;

        transform.DOLocalJump(position, 2f, 1, 1f);
        transform.DOLocalRotate(new Vector3(0f, 90f, 0f), 1f);
     
        _collected = true;
    }

    public void Spend(Vector3 position, Transform parent)
    {
        transform.parent = parent;

        transform.position = position;
        _spent = true;
    }
}
