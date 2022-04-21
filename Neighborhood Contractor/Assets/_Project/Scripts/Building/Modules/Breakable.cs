using System.Collections;
using UnityEngine;
using System;
using ZestGames.Utility;

[RequireComponent(typeof(Building))]
public class Breakable : MonoBehaviour
{
    private Building _building;
    public Building Building => _building;

    [Header("-- SETUP --")]
    [SerializeField] private float brokenTimeBeforeDemolish = 60f;
    [SerializeField] private float breakRandomizerDelay = 5f;

    private WaitForSeconds _waitForBreakRandomizer;

    public bool Broken { get; private set; }
    public Action OnBuildingIsBroken;

    public void Init(Building building)
    {
        _building = building;
        _waitForBreakRandomizer = new WaitForSeconds(breakRandomizerDelay);
        Broken = false;

        CheckForActivation();

        OnBuildingIsBroken += Break;
        //_building.Repairable.OnBuildingRepaired += Repaired;
    }

    private void OnDisable()
    {
        OnBuildingIsBroken -= Break;
        //_building.Repairable.OnBuildingRepaired -= Repaired;
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
}
