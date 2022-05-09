using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(Building))]
public class Repairable : MonoBehaviour
{
    private Building _building;
    public Building Building => _building;

    [Header("-- SETUP --")]
    [SerializeField] private float repairTime = 3f;
    [SerializeField] private GameObject repairArea;
    [SerializeField] private Image repairFillImage;

    [Header("-- CONSUME --")]
    [SerializeField] private Transform moneyPointTransform;
    [SerializeField] private float repairCost = 1000;
    private float _consumedMoney;
    private readonly float _defaultRepairCost = 1000;

    public bool PlayerIsInRepairArea { get; set; }
    public bool CanBeRepaired => StatManager.CurrentCarry > 0 && !Repaired;
    public bool Repaired => repairCost == _consumedMoney;
    public float RepairCost => repairCost;
    public Transform MoneyPointTransform => moneyPointTransform;
    
    public Action OnBuildingRepaired;

    public void Init(Building building)
    {
        _building = building;
        _consumedMoney = 0;

        CheckForActivation();
    }

    private void CheckForActivation()
    {
        if (_building.CanBeRepaired)
            repairArea.SetActive(true);
        else
            repairArea.SetActive(false);
    }

    public void RepairSuccessful()
    {
        BuildManager.Instance.StopRepairing(this);
        PlayerIsInRepairArea = false;
        _consumedMoney = 0;

        OnBuildingRepaired?.Invoke();
        NeighborhoodEvents.OnBuildingRepaired?.Invoke(_building);
        ObjectPooler.Instance.SpawnFromPool("Confetti", repairArea.transform.position, Quaternion.identity);
        AudioHandler.PlayAudio(AudioHandler.AudioType.BuildingFinished);
        repairArea.SetActive(false);
        //Debug.Log("Repaired");
    }

    public void UpdateRepairUi()
    {
        //transform.DORewind();
        float currentVal = repairFillImage.fillAmount;
        float targetVal = _consumedMoney / repairCost;
        DOVirtual.Float(currentVal, targetVal, 0.3f, r => {
            repairFillImage.fillAmount = r;
            currentVal = r;
        });
    }

    public void ResetRepairUi()
    {
        transform.DOKill();

        float currentVal = repairFillImage.fillAmount;
        float targetVal = 0;
        DOVirtual.Float(currentVal, targetVal, 2f, r => {
            repairFillImage.fillAmount = r;
        });
    }

    public void Broken(Building building)
    {
        if (building != _building) return;

        UpdateRepairCost();
        repairArea.SetActive(true);
        repairFillImage.fillAmount = 0f;
    }

    private void UpdateRepairCost() => repairCost = (Building.CurrentLevel * _defaultRepairCost) + (PhaseManager.CurrentPhase * 100f);
    public void ConsumeMoney(float amount) => _consumedMoney += amount;
    public void ResetConsumedMoney() => _consumedMoney = 0;
}
