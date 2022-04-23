using System.Collections;
using UnityEngine;
using System;
using ZestGames.Utility;

[RequireComponent(typeof(Building))]
public class Breakable : MonoBehaviour
{
    private Building _building;

    [Header("-- SETUP --")]
    [SerializeField] private float brokenTimeBeforeDemolish = 60f;
    [SerializeField] private float breakRandomizerDelay = 5f;
    [SerializeField] private Transform complainArea;

    private int _buildingPopulation;
    private WaitForSeconds _waitForBreakRandomizer;

    public bool Broken { get; private set; }
    public Action OnBuildingIsBroken;

    public void Init(Building building)
    {
        _building = building;

        _buildingPopulation = building.Rentable.MaxBuildingPopulation;
        _waitForBreakRandomizer = new WaitForSeconds(breakRandomizerDelay);
        Broken = false;

        CheckForActivation();

        OnBuildingIsBroken += Break;
    }

    private void OnDisable()
    {
        OnBuildingIsBroken -= Break;
    }

    private void CheckForActivation()
    {
        if (_building.CanBeBroken)
            StartCoroutine(BreakRandomizer());
    }

    private IEnumerator BreakRandomizer()
    {
        while (_building.CanBeBroken)
        {
            yield return _waitForBreakRandomizer;

            if (RNG.RollDice(100))
            {
                OnBuildingIsBroken?.Invoke();
                StartCoroutine(SpawnComplainingNeighbors(_buildingPopulation));
                Debug.Log("BROKEN!");
            }
            else
                Debug.Log("NOT BROKEN!");
        }
    }

    private void Break()
    {
        Broken = true;
        StopCoroutine(BreakRandomizer());
    }

    public void Repaired()
    {
        Broken = false;
        if (_building.CanBeBroken)
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
            //neighbor.RelatedBuilding.Repairable.OnBuildingRepaired += neighbor.GoBackToTheHouse;
            neighbor.RelatedBuilding.Rentable.OnStartComplaint?.Invoke();

            yield return new WaitForSeconds(UnityEngine.Random.Range(0.2f, 1f));
        }
    }
}
