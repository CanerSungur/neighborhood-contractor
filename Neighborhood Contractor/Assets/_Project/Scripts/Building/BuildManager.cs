using System.Collections;
using UnityEngine;

public class BuildManager : Singleton<BuildManager>
{
    private Player _player;
    private WaitForSeconds _waitForSpendTime = new WaitForSeconds(StatManager.SpendTime);

    private void Awake()
    {
        this.Reload();
        _player = FindObjectOfType<Player>();
    }

    #region Buildable

    public void StartBuildable(Buildable buildable) => StartCoroutine(Buildable(buildable));
    public void StopBuildable(Buildable buildable) => StopCoroutine(Buildable(buildable));
    private IEnumerator Buildable(Buildable buildable)
    {
        while (buildable.PlayerIsInBuildArea && buildable.CanBeBuilt)
        {
            StatManager.CollectedMoney[StatManager.CollectedMoney.Count - 1].Spend(buildable.MoneyPointTransform);
            _player.SpendMoney(StatManager.SpendValue);
            buildable.ConsumeMoney(StatManager.SpendValue);

            if (buildable.Built)
                buildable.FinishBuilding();

            yield return _waitForSpendTime;
        }
    }

    #endregion

    #region Upgradeable

    public void UpgradeBuilding(Upgradeable upgradeable)
    {
        int moneyCount = upgradeable.UpgradeCost / StatManager.SpendValue;
        for (int i = 0; i < moneyCount; i++)
        {
            StatManager.CollectedMoney[StatManager.CollectedMoney.Count - 1].Spend(upgradeable.MoneyPointTransform);
            _player.SpendMoney(StatManager.SpendValue);
        }

        upgradeable.UpgradeBuilding();
    }

    #endregion

    public void StartBuilding(IBuilding building) => StartCoroutine(Build(building));
    public void StopBuilding(IBuilding building) => StopCoroutine(Build(building));

    private IEnumerator Build(IBuilding building)
    {
        while (building.PlayerIsInBuildArea && building.CanBeBuilt)
        {
            StatManager.CollectedMoney[StatManager.CollectedMoney.Count - 1].Spend(building.MoneyPointTransform);
            _player.SpendMoney(StatManager.SpendValue);
            building.ConsumeMoney(StatManager.SpendValue);

            if (building.Built)
                building.FinishBuilding();

            yield return _waitForSpendTime;
        }
    }

    public void Upgrade(IBuilding building)
    {
        int moneyCount = building.UpgradeCost / StatManager.SpendValue;
        for (int i = 0; i < moneyCount; i++)
        {
            StatManager.CollectedMoney[StatManager.CollectedMoney.Count - 1].Spend(building.MoneyPointTransform);
            _player.SpendMoney(StatManager.SpendValue);
        }

        building.UpgradeBuilding();
    }

    public void StartBuildingNewPhase(PhaseUnlocker phaseUnlocker) => StartCoroutine(OpenNewPhase(phaseUnlocker));
    public void StopBuildingNewPhase(PhaseUnlocker phaseUnlocker) => StopCoroutine(OpenNewPhase(phaseUnlocker));

    private IEnumerator OpenNewPhase(PhaseUnlocker phaseUnlocker)
    {
        while (phaseUnlocker.PlayerIsInBuildArea && phaseUnlocker.CanBeBuilt)
        {
            StatManager.CollectedMoney[StatManager.CollectedMoney.Count - 1].Spend(phaseUnlocker.MoneyPointTransform);
            _player.SpendMoney(StatManager.SpendValue);
            phaseUnlocker.ConsumeMoney(StatManager.SpendValue);

            if (phaseUnlocker.Built)
                phaseUnlocker.EnableNextPhase();

            yield return _waitForSpendTime;
        }
    }
}
