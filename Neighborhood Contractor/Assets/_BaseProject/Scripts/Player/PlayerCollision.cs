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

            if (building.CanBeBuilt)
                BuildManager.Instance.StartBuilding(building);
            else
                BuildManager.Instance.StopBuilding(building);

            //building.PlayerIsInBuildArea = true;
            //StatManager.CollectedMoney[StatManager.CollectedMoney.Count - 1].Spend(building.MoneyPointTransform);
            ////building.ConsumeMoney(StatManager.SpendValue, StatManager.SpendRate);
            //_player.SpendMoney(StatManager.SpendValue);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        //if (other.transform.parent.TryGetComponent(out IBuilding building) && !building.Builded)
        //{
        //    building.PlayerIsInBuildArea = true;
            
        //    float timePassed = Time.time + StatManager.SpendTime;
        //    if (Time.time > timePassed)
        //    {
        //        timePassed = Time.time + StatManager.SpendTime;
        //        StatManager.CollectedMoney[StatManager.CollectedMoney.Count - 1].Spend(building.MoneyPointTransform);
        //        //building.ConsumeMoney(StatManager.SpendValue, StatManager.SpendRate);
        //        _player.SpendMoney(StatManager.SpendValue);
        //    }
        //}
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.parent.TryGetComponent(out IBuilding building) && building.PlayerIsInBuildArea)
        {
            building.PlayerIsInBuildArea = false;
            BuildManager.Instance.StopBuilding(building);
        }
    }
}
