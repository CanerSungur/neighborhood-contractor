using System;
using System.Collections;
using UnityEngine;

public class BuildManager : Singleton<BuildManager>
{
    private Player _player;
    private WaitForSeconds _waitForSpendTime = new WaitForSeconds(StatManager.SpendTime);
    private WaitForSeconds _waitForMinimumTime = new WaitForSeconds(0.001f);
    
    private WaitForSeconds _waitForRepairTime = new WaitForSeconds(0.5f);
    private readonly float _totalRepairTime = 5f;

    public int BuildingCount { get; set; }

    private void Awake()
    {
        this.Reload();
        _player = FindObjectOfType<Player>();
    }

    private void Start()
    {
        NeighborhoodEvents.OnBuildingFinished += IncreaseBuildingCount;
    }

    private void OnDisable()
    {
        NeighborhoodEvents.OnBuildingFinished -= IncreaseBuildingCount;
    }

    private void IncreaseBuildingCount(Building ignoreThis)
    {
        BuildingCount++;
        if (BuildingCount >= 2 && SingleMoneySpawnPointManager.FreeSpawnActive)
        {
            SingleMoneySpawnPointManager.FreeSpawnActive = false;
            NeighborhoodEvents.OnDisableFreeMoneySpawn?.Invoke();
        }
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

            if (buildable.BuildCost >= 30000)
                yield return _waitForMinimumTime;
            else
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

    #region Phase Unlocker

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

    #endregion

    #region Repair

    public void StartRepairing(Repairable repairable)
    {
        UpdateWaitRepairTime(repairable);
        StartCoroutine(Repair(repairable));
    }
    public void StopRepairing(Repairable repairable) => StopCoroutine(Repair(repairable));
    private void UpdateWaitRepairTime(Repairable repairable)
    {
        // Calculate wait time according to Repair Cost
        int moneyCount = (int)(repairable.RepairCost / StatManager.SpendValue);
        _waitForRepairTime = new WaitForSeconds(_totalRepairTime / moneyCount);
    }

    private IEnumerator Repair(Repairable repairable)
    {
        while (repairable.PlayerIsInRepairArea && repairable.CanBeRepaired)
        {
            StatManager.CollectedMoney[StatManager.CollectedMoney.Count - 1].SpendForRepair(repairable.MoneyPointTransform);
            _player.SpendMoney(StatManager.SpendValue);
            repairable.ConsumeMoney(StatManager.SpendValue);
            

            if (repairable.Repaired)
                repairable.RepairSuccessful();
            else
                ZestGames.Utility.Delayer.DoActionAfterDelay(this, 0.5f, () => repairable.UpdateRepairUi());

            yield return _waitForRepairTime;
        }
    }

    #endregion
}
