using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(Building))]
public class Repairable : MonoBehaviour
{
    private Building _building;
    public Building Building => _building;

    [Header("-- SETUP --")]
    [SerializeField] private float repairTime = 3f;
    [SerializeField] private GameObject repairArea;
    [SerializeField] private Image repairFillImage;
   
    private IEnumerator _repairCoroutine;
    private float _timer;

    public Action OnBuildingRepaired;

    public void Init(Building building)
    {
        _building = building;
        _repairCoroutine = Repair();
        _timer = repairTime;

        CheckForActivation();
    }

    private void CheckForActivation()
    {
        if (_building.CanBeRepaired)
            repairArea.SetActive(true);
        else
            repairArea.SetActive(false);
    }

    public void StartRepairing()
    {
        StartCoroutine(_repairCoroutine);
    }

    public void StopRepairing()
    {
        StopCoroutine(_repairCoroutine);
        ResetRepairCoroutine();
    }

    private IEnumerator Repair()
    {
        _timer = repairTime;
        while (_timer > 0f)
        {
            _timer -= Time.deltaTime;
            repairFillImage.fillAmount = 1f - (_timer / repairTime);
            yield return null;
        }

        Debug.Log("Repaired!");
        OnBuildingRepaired?.Invoke();
        NeighborhoodEvents.OnBuildingRepaired?.Invoke(_building);
        repairArea.SetActive(false);
    }

    public void Broken(Building building)
    {
        if (building != _building) return;

        repairArea.SetActive(true);
        ResetRepairCoroutine();
    }

    private void ResetRepairCoroutine()
    {
        _repairCoroutine = Repair();
        _timer = repairTime;
        repairFillImage.fillAmount = 0f;
    }
}
