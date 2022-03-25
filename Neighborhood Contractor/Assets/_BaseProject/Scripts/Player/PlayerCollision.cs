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
        else if (other.transform.parent.TryGetComponent(out IBuilding building) && !building.PlayerIsInBuildArea)
        {
            building.PlayerIsInBuildArea = true;

            if (other.gameObject.layer == LayerMask.NameToLayer("Build Area"))
            {
                if (building.CanBeBuilt)
                    BuildManager.Instance.StartBuilding(building);
                else
                {
                    BuildManager.Instance.StopBuilding(building);
                    FeedbackEvents.OnGiveFeedback?.Invoke("Not Enough MONEY", FeedbackUI.Colors.NotEnoughMoney);
                }
            }
            else if (other.gameObject.layer == LayerMask.NameToLayer("Income Area") && other.transform.parent.TryGetComponent(out IContributorIncome incomeBuilding))
            {
                GameManager.Instance.collectableManager.StartCollectingIncome(incomeBuilding, building);
            }
            else if (other.gameObject.layer == LayerMask.NameToLayer("Upgrade Area") && building.CanBeUpgraded)
            {
                Debug.Log("Upgrading...");
                BuildingUpgradeEvents.OnActivateBuildingUpgradeUI?.Invoke(building);
                // Open building upgrade UI here.
            }
            else if (other.gameObject.layer == LayerMask.NameToLayer("Locked Build Area"))
            {
                FeedbackEvents.OnGiveFeedback?.Invoke("Not Enough POPULATION", FeedbackUI.Colors.NotEnoughPopulation);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out PhaseUnlocker phaseUnlocker) && phaseUnlocker.PlayerIsInBuildArea)
        {
            phaseUnlocker.PlayerIsInBuildArea = false;
            BuildManager.Instance.StopBuildingNewPhase(phaseUnlocker);
        }
        else if (other.transform.parent.TryGetComponent(out IBuilding building) && building.PlayerIsInBuildArea)
        {
            building.PlayerIsInBuildArea = false;
            
            BuildManager.Instance.StopBuilding(building);

            if (other.transform.parent.TryGetComponent(out IContributorIncome incomeBuilding))
                GameManager.Instance.collectableManager.StopCollectingIncome(incomeBuilding, building);
        }
    }
}
