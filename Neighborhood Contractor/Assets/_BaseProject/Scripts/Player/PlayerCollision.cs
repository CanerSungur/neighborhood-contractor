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

        if (other.gameObject.layer == LayerMask.NameToLayer("Creative Building") && other.transform.parent && other.transform.parent.TryGetComponent(out CreativeBuild creativeBuilding) && !creativeBuilding.PlayerIsInBuildArea)
        {
            creativeBuilding.PlayerIsInBuildArea = true;
            if (creativeBuilding.CanBeBuilt)
            {
                creativeBuilding.StartBuilding();
            }
            else
            {
                creativeBuilding.StopBuilding();
            }
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Build Area") && other.transform.parent && other.transform.parent.TryGetComponent(out Building building) && !building.Buildable.PlayerIsInBuildArea)
        {
            building.Buildable.PlayerIsInBuildArea = true;
            if (building.Buildable.CanBeBuilt)
                BuildManager.Instance.StartBuildable(building.Buildable);
            else
            {
                BuildManager.Instance.StopBuildable(building.Buildable);
                FeedbackEvents.OnGiveFeedback?.Invoke("Not Enough MONEY", FeedbackUI.Colors.NotEnoughMoney);
            }
        }
        
        if (other.gameObject.layer == LayerMask.NameToLayer("Locked Build Area"))
            FeedbackEvents.OnGiveFeedback?.Invoke("Not Enough POPULATION", FeedbackUI.Colors.NotEnoughPopulation);
        
        if (other.gameObject.layer == LayerMask.NameToLayer("Income Area") && other.transform.parent.TryGetComponent(out IncomeSpawner incomeSpawner) && !incomeSpawner.PlayerIsInArea)
        {
            incomeSpawner.PlayerIsInArea = true;
            GameManager.Instance.collectableManager.StartCollectIncome(incomeSpawner);
        }
        
        if (other.gameObject.layer == LayerMask.NameToLayer("Upgrade Area") && other.transform.parent.TryGetComponent(out Upgradeable upgradeable) && !BuildingUpgradeUI.IsOpen)
        {
            Player.Upgrading = true;
            BuildingUpgradeEvents.OnActivateUpgradeUI?.Invoke(upgradeable);
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Repair Area") && other.transform.parent.TryGetComponent(out Repairable repairable) && !repairable.PlayerIsInRepairArea)
        {
            repairable.PlayerIsInRepairArea = true;
            //repairable.ResetConsumedMoney();
            if (repairable.Building.CanBeRepaired)
            {
                BuildManager.Instance.StartRepairing(repairable);
                //repairable.StartRepairingUi();
            }
            else
            {
                BuildManager.Instance.StopRepairing(repairable);
                //repairable.StopRepairingUi();
                FeedbackEvents.OnGiveFeedback?.Invoke("Not Enough MONEY", FeedbackUI.Colors.NotEnoughMoney);
            }
        }


        if (other.TryGetComponent(out PhaseUnlocker phaseUnlocker) && !phaseUnlocker.PlayerIsInBuildArea)
        {
            phaseUnlocker.PlayerIsInBuildArea = true;
            if (phaseUnlocker.CanBeBuilt)
                BuildManager.Instance.StartBuildingNewPhase(phaseUnlocker);
            else
            {
                BuildManager.Instance.StopBuildingNewPhase(phaseUnlocker);
                FeedbackEvents.OnGiveFeedback?.Invoke("Not Enough MONEY", FeedbackUI.Colors.NotEnoughMoney);
            }

            if (!phaseUnlocker.EnoughPopulation)
                FeedbackEvents.OnGiveFeedback?.Invoke("Not Enough POPULATION", FeedbackUI.Colors.NotEnoughPopulation);
        }

        else if (other.TryGetComponent(out Money money) && money.CanBeCollected)
        {
            money.Collect(_player.moneyStackHandler.TargetStackPosition, _player.moneyStackHandler.StackTransform);
            _player.CollectMoney(StatManager.MoneyValue);
        }

        // spend money section
        //else if (other.transform.parent.TryGetComponent(out IBuilding building) && !building.PlayerIsInBuildArea)
        //{
        //    building.PlayerIsInBuildArea = true;

        //    if (other.gameObject.layer == LayerMask.NameToLayer("Build Area"))
        //    {
        //        if (building.CanBeBuilt)
        //            BuildManager.Instance.StartBuilding(building);
        //        else
        //        {
        //            BuildManager.Instance.StopBuilding(building);
        //            FeedbackEvents.OnGiveFeedback?.Invoke("Not Enough MONEY", FeedbackUI.Colors.NotEnoughMoney);
        //        }
        //    }
        //    else if (other.gameObject.layer == LayerMask.NameToLayer("Income Area") && other.transform.parent.TryGetComponent(out IContributorIncome incomeBuilding))
        //    {
        //        GameManager.Instance.collectableManager.StartCollectingIncome(incomeBuilding, building);
        //    }
        //    else if (other.gameObject.layer == LayerMask.NameToLayer("Upgrade Area") && building.CanBeUpgraded)
        //    {
        //        BuildingUpgradeEvents.OnActivateBuildingUpgradeUI?.Invoke(building);
        //        // Open building upgrade UI here.
        //    }
        //    else if (other.gameObject.layer == LayerMask.NameToLayer("Locked Build Area"))
        //    {
        //        FeedbackEvents.OnGiveFeedback?.Invoke("Not Enough POPULATION", FeedbackUI.Colors.NotEnoughPopulation);
        //    }
        //}
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Creative Building") && other.transform.parent.TryGetComponent(out CreativeBuild creativeBuilding) && creativeBuilding.PlayerIsInBuildArea)
        {
            creativeBuilding.PlayerIsInBuildArea = false;
            creativeBuilding.StopBuilding();
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Build Area") && other.transform.parent.TryGetComponent(out Building building) && building.Buildable.PlayerIsInBuildArea)
        {
            building.Buildable.PlayerIsInBuildArea = false;
            BuildManager.Instance.StopBuildable(building.Buildable);
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Income Area") && other.transform.parent.TryGetComponent(out IncomeSpawner incomeSpawner) && incomeSpawner.PlayerIsInArea)
        {
            incomeSpawner.PlayerIsInArea = false;
            GameManager.Instance.collectableManager.StopCollectIncome(incomeSpawner);
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Upgrade Area") && other.transform.parent.TryGetComponent(out Upgradeable upgradeable) && BuildingUpgradeUI.IsOpen)
        {
            //upgradeable.PlayerIsInArea = false;
            Player.Upgrading = false;
            BuildingUpgradeEvents.OnCloseUpgradeUI?.Invoke(upgradeable);
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Repair Area") && other.transform.parent.TryGetComponent(out Repairable repairable) && repairable.PlayerIsInRepairArea/* && repairable.Building.CanBeRepaired*/)
        {
            repairable.PlayerIsInRepairArea = false;
            //repairable.ResetConsumedMoney();
            BuildManager.Instance.StopRepairing(repairable);
            //repairable.StopRepairingUi();
            //repairable.ResetRepairUi();
        }

        if (other.TryGetComponent(out PhaseUnlocker phaseUnlocker) && phaseUnlocker.PlayerIsInBuildArea)
        {
            phaseUnlocker.PlayerIsInBuildArea = false;
            BuildManager.Instance.StopBuildingNewPhase(phaseUnlocker);
        }
        //else if (other.transform.parent.TryGetComponent(out IBuilding building) && building.PlayerIsInBuildArea)
        //{
        //    building.PlayerIsInBuildArea = false;

        //    BuildManager.Instance.StopBuilding(building);

        //    if (other.transform.parent.TryGetComponent(out IContributorIncome incomeBuilding))
        //        GameManager.Instance.collectableManager.StopCollectingIncome(incomeBuilding, building);
        //}
    }
}
