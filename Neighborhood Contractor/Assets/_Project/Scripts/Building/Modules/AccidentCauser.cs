using System.Collections;
using UnityEngine;
using System;
using ZestGames.Utility;

[RequireComponent(typeof(Building))]
public class AccidentCauser : MonoBehaviour
{
    private Building _building;
    public Building Building => _building;
    private AccidentHandler _accidentHandler;
    private IndicatorTarget _indicatorTarget;

    [Header("-- SETUP --")]
    [SerializeField] private float brokenTimeBeforeDemolish = 60f;
    [SerializeField] private float breakRandomizerDelay = 5f;
    [SerializeField] private float breakChance = 30f;
    [SerializeField] private Transform complainArea;

    private int _buildingPopulation;
    private WaitForSeconds _waitForBreakRandomizer;
    private bool _breakRandomizerIsRunning;

    private readonly float breakChanceDecreaseRate = 5f;

    public bool AccidentHappened { get; private set; }
    public AccidentHandler AccidentHandler => _accidentHandler;
    public Action<Building> OnAccidentHappened;

    public void Init(Building building)
    {
        _building = building;
        _accidentHandler = GetComponentInChildren<AccidentHandler>();
        _accidentHandler.Init(this);

        _buildingPopulation = building.Rentable.MaxBuildingPopulation;
        _waitForBreakRandomizer = new WaitForSeconds(breakRandomizerDelay);
        AccidentHappened = _breakRandomizerIsRunning = false;

        CheckForActivation(_building);

        OnAccidentHappened += Accident;
        NeighborhoodEvents.OnBuildingFinished += CheckForActivation;
        NeighborhoodEvents.OnBuildingUpgraded += CheckForActivation;

        _indicatorTarget = GetComponentInChildren<IndicatorTarget>();
        _indicatorTarget.Init(this);
    }

    private void OnDisable()
    {
        OnAccidentHappened -= Accident;
        NeighborhoodEvents.OnBuildingFinished -= CheckForActivation;
        NeighborhoodEvents.OnBuildingUpgraded -= CheckForActivation;
    }

    private void CheckForActivation(Building building)
    {
        if (_building.CanAccidentHappen && building == _building)
        {
            if (_breakRandomizerIsRunning)
                StopCoroutine(BreakRandomizer());

            StartCoroutine(BreakRandomizer());
        }
    }

    private IEnumerator BreakRandomizer()
    {
        _breakRandomizerIsRunning = true;

        while (_building.CanAccidentHappen)
        {
            yield return _waitForBreakRandomizer;

            if (RNG.RollDice((int)breakChance))
            {
                OnAccidentHappened?.Invoke(_building);
                NeighborhoodEvents.OnAccidentHappened?.Invoke(_building);
                StartCoroutine(SpawnComplainingNeighbors(_buildingPopulation));
                Debug.Log("BROKEN!");
            }
            else
                Debug.Log("NOT BROKEN!");
        }

        _breakRandomizerIsRunning = false;
    }

    private void Accident(Building building)
    {
        if (building != _building) return;

        AccidentHappened = true;
        StopCoroutine(BreakRandomizer());
    }

    public void Repaired()
    {
        AccidentHappened = false;
        if (_building.CanAccidentHappen)
            StartCoroutine(BreakRandomizer());
    }

    private IEnumerator SpawnComplainingNeighbors(int count)
    {
        for (int i = 0; i < count; i++)
        {
            Neighbor neighbor = ObjectPooler.Instance.SpawnFromPool("Complaining_Neighbor_Man", transform.position, Quaternion.identity).GetComponent<Neighbor>();

            Vector3 offset = new Vector3(-1.5f, 0f, 0f) * i + Vector3.forward * UnityEngine.Random.Range(0f, -1f);
            neighbor.OnSetTargetPos?.Invoke(complainArea.transform.position + offset);

            neighbor.RelatedBuilding = _building;
            neighbor.RelatedBuilding.Rentable.OnStartComplaint?.Invoke();

            yield return new WaitForSeconds(UnityEngine.Random.Range(0.2f, 1f));
        }
    }
}
