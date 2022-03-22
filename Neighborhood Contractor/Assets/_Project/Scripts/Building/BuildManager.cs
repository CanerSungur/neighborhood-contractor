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

    public void StartBuilding(IBuilding building) => StartCoroutine(Build(building));
    public void StopBuilding(IBuilding building) => StopCoroutine(Build(building));

    private IEnumerator Build(IBuilding building)
    {
        while (building.PlayerIsInBuildArea && building.CanBeBuilt)
        {
            StatManager.CollectedMoney[StatManager.CollectedMoney.Count - 1].Spend(building.MoneyPointTransform);
            _player.SpendMoney(StatManager.SpendValue);
            building.ConsumeMoney(StatManager.SpendValue);


            if (building.Builded)
                building.FinishBuilding();

            yield return _waitForSpendTime;
        }
    }
}
