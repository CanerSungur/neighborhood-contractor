using UnityEngine;
using System;
using System.Collections;
using TMPro;
using UnityEngine.UI;

[RequireComponent(typeof(Building))]
public class Repairable : MonoBehaviour
{
    private Building _building;
    public Building Building => _building;

    [Header("-- SETUP --")]
    [SerializeField] private float repairTime = 3f;
    [SerializeField] private GameObject repairArea;
    //[SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private Image repairFillImage;
   
    private WaitForSeconds _waitForRepair;
    private IEnumerator _repairCoroutine;
    private float _timer;

    public Action OnBuildingRepaired;

    public void Init(Building building)
    {
        _building = building;
        _waitForRepair = new WaitForSeconds(repairTime);
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
        //_repairCoroutine.Reset();
        _timer = repairTime;
        repairFillImage.fillAmount = 0f;
    }

    private IEnumerator Repair()
    {
        _timer = repairTime;
        while (_timer > 0f)
        {
            _timer -= Time.deltaTime;
            //timerText.text = _timer.ToString("#0.00");
            repairFillImage.fillAmount = 1f - (_timer / repairTime);
            yield return null;
        }

        Debug.Log("Repaired!");
        OnBuildingRepaired?.Invoke();
        repairArea.SetActive(false);

        //while (_building.CanBeRepaired)
        //{
        //    _timer -= Time.deltaTime;
        //    timerText.text = _timer.ToString("#0.00");
        //    if (_timer <= 0f)
        //    {
        //        _timer = repairTime;

        //        Debug.Log("Repaired!");
        //        OnBuildingRepaired?.Invoke();
        //        repairArea.SetActive(false);
        //    }
        //}

        //yield return null;
    }

    public void Broken()
    {
        repairArea.SetActive(true);
        //timerText.text = _timer.ToString("#0.00");
        _timer = repairTime;
        repairFillImage.fillAmount = 0f;
    }
}
