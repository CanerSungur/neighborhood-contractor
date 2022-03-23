using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerCollision : MonoBehaviour
{
    private Player _player;

    private void Awake()
    {
        _player = GetComponent<Player>();
    }

    private void OnTriggerEnter(Collider other)
    {
        #region Coin

        //if (other.TryGetComponent(out CollectableBase collectable))
        //{
        //    collectable.Collect();
        //    _player.PickUpTrigger(collectable.CollectableEffect);
        //}

        #endregion

        if (other.TryGetComponent(out Money money) && money.CanBeCollected)
        {
            money.Collect(_player.moneyStackHandler.TargetStackPosition, _player.moneyStackHandler.StackTransform);
            _player.CollectMoney(StatManager.MoneyValue);
        }

        // spend money section
        if (other.transform.parent.TryGetComponent(out IBuilding building) && !building.PlayerIsInBuildArea)
        {
            building.PlayerIsInBuildArea = true;

            #region Build Section

            if (building.CanBeBuilt)
                BuildManager.Instance.StartBuilding(building);
            else
                BuildManager.Instance.StopBuilding(building);

            #endregion

            #region Collecting Income Section

            GameManager.Instance.collectableManager.StartCollectingIncome(building);

            #endregion
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.parent.TryGetComponent(out IBuilding building) && building.PlayerIsInBuildArea)
        {
            building.PlayerIsInBuildArea = false;
            BuildManager.Instance.StopBuilding(building);
            GameManager.Instance.collectableManager.StopCollectingIncome(building);
        }
    }
}
