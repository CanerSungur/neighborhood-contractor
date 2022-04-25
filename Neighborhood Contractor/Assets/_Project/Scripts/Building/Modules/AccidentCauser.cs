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
    [SerializeField] private float accidentTimeBeforeDemolish = 60f;
    [SerializeField] private float accidentRandomizerDelay = 5f;
    [SerializeField] private float accidentChance = 30f;
    [SerializeField] private Transform complainArea;
    private float _defaultAccidentChance;

    private IEnumerator _accidentRandomizer;
    private WaitForSeconds _waitForAccidentRandomizer;
    private bool _accidentRandomizerIsRunning;

    private readonly float _accidentChanceDecreaseRate = 5f;
    public float AccidentChanceDecreaseRate => _accidentChanceDecreaseRate;

    public bool AccidentHappened { get; private set; }
    public AccidentHandler AccidentHandler => _accidentHandler;
    public Action<Building> OnAccidentHappened;

    // Spawning controls
    private bool _hasMan, _hasWoman, _hasChild;

    public void Init(Building building)
    {
        _building = building;
        _accidentHandler = GetComponentInChildren<AccidentHandler>();
        _accidentHandler.Init(this);

        _accidentRandomizer = AccidentRandomizer();
        _waitForAccidentRandomizer = new WaitForSeconds(accidentRandomizerDelay);
        AccidentHappened = _accidentRandomizerIsRunning = false;
        _defaultAccidentChance = accidentChance;

        Delayer.DoActionAfterDelay(this, 10f, () => CheckForActivation(_building));

        #region EVENTS

        OnAccidentHappened += Accident;
        NeighborhoodEvents.OnBuildingFinished += CheckForActivation;
        NeighborhoodEvents.OnBuildingUpgraded += CheckForActivation;
        NeighborhoodEvents.OnBuildingMaxxedOut += CheckForActivation;

        #endregion

        _indicatorTarget = GetComponentInChildren<IndicatorTarget>();
        _indicatorTarget.Init(this);

        _hasMan = _hasWoman = _hasChild = false;
    }

    private void OnDisable()
    {
        #region EVENTS

        OnAccidentHappened -= Accident;
        NeighborhoodEvents.OnBuildingFinished -= CheckForActivation;
        NeighborhoodEvents.OnBuildingUpgraded -= CheckForActivation;
        NeighborhoodEvents.OnBuildingMaxxedOut -= CheckForActivation;

        #endregion
    }

    private void UpdateAccidentChance()
    {
        if (_building.CanAccidentHappen)
        {
            accidentChance = _defaultAccidentChance - (_accidentChanceDecreaseRate * (_building.CurrentLevel - 1));
            //Debug.Log("Accident Chance: " + accidentChance);
        }
    }

    private void CheckForActivation(Building building)
    {
        if (building == _building)
        {
            if (_accidentRandomizerIsRunning)
                StopCoroutine(_accidentRandomizer);

            StartCoroutine(_accidentRandomizer);
        }
    }

    #region Accident Functions

    private IEnumerator AccidentRandomizer()
    {
        _accidentRandomizerIsRunning = true;

        while (true)
        {
            yield return _waitForAccidentRandomizer;
            UpdateAccidentChance();

            if (RNG.RollDice((int)accidentChance) && _building.CanAccidentHappen)
            {
                OnAccidentHappened?.Invoke(_building);
                NeighborhoodEvents.OnAccidentHappened?.Invoke(_building);
                StartCoroutine(SpawnComplainingNeighbors(Building.Rentable.CurrentBuildingPopulation));
                //Debug.Log("BROKEN!");
            }
            //else
                //Debug.Log("NOT BROKEN!");
        }

        _accidentRandomizerIsRunning = false;
    }

    private void Accident(Building building)
    {
        if (building != _building) return;

        AccidentHappened = true;
        StopCoroutine(_accidentRandomizer);
    }

    public void Repaired()
    {
        AccidentHappened = false;
        StartCoroutine(_accidentRandomizer);
    }

    #endregion

    #region Spawning Functions

    private IEnumerator SpawnComplainingNeighbors(int count)
    {
        for (int i = 0; i < count; i++)
        {
            Vector3 offset = new Vector3(UnityEngine.Random.Range(-1.1f, -0.8f), 0f, 0f) * i + Vector3.forward * UnityEngine.Random.Range(-1f, -2.5f);
            Vector3 pos = complainArea.transform.position + offset;
            SpawnNeighbor(pos);

            yield return new WaitForSeconds(UnityEngine.Random.Range(0.2f, 1f));
        }
    }

    private void SpawnNeighbor(Vector3 pos)
    {
        Neighbor neighbor = null;
        if (!_hasMan)
        {
            neighbor = ObjectPooler.Instance.SpawnFromPool("Complaining_Neighbor_Man", transform.position, Quaternion.identity).GetComponent<Neighbor>();
            _hasMan = true;
        }
        else if (!_hasWoman)
        {
            neighbor = ObjectPooler.Instance.SpawnFromPool("Complaining_Neighbor_Woman", transform.position, Quaternion.identity).GetComponent<Neighbor>();
            _hasWoman = true;
        }
        else if (!_hasChild)
        {
            neighbor = ObjectPooler.Instance.SpawnFromPool("Complaining_Neighbor_Child", transform.position, Quaternion.identity).GetComponent<Neighbor>();
            _hasChild = true;
        }
        else if (_hasChild)
        {
            neighbor = ObjectPooler.Instance.SpawnFromPool("Complaining_Neighbor_Child", transform.position, Quaternion.identity).GetComponent<Neighbor>();
            _hasMan = _hasWoman = _hasChild = false;
        }

        if (neighbor != null)
        {
            neighbor.OnSetTargetPos?.Invoke(pos);

            neighbor.RelatedBuilding = _building;
            neighbor.RelatedBuilding.Rentable.OnStartComplaint?.Invoke();
        }
    }

    #endregion
}
