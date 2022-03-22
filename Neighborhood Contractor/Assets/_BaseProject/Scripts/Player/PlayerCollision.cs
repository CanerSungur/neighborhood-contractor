using UnityEngine;
using System;

[RequireComponent(typeof(Player))]
public class PlayerCollision : MonoBehaviour
{
    private Player _player;

    private void Awake()
    {
        _player = GetComponent<Player>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!_player.IsLanded)
            _player.LandTrigger();


        if (collision.gameObject.TryGetComponent(out CollectableBase collectable))
        {
            collectable.Collect();
            _player.PickUpTrigger(collectable.CollectableEffect);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out CollectableBase collectable))
        {
            collectable.Collect();
            _player.PickUpTrigger(collectable.CollectableEffect);
        }

        if (other.TryGetComponent(out Money money) && money.CanBeCollected)
        {
            money.Collect(_player.moneyStackHandler.TargetStackPosition, _player.moneyStackHandler.StackTransform);
            _player.CollectMoneyTrigger();

            Debug.Log("Hit Money");
        }
    }
}
