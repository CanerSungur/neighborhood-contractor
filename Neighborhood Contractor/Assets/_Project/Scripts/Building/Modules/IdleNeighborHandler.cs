using System.Collections;
using UnityEngine;

public class IdleNeighborHandler : MonoBehaviour
{
    private Building _building;

    [Header("-- SETUP --")]
    [SerializeField] private GameObject idleNeighborsParent;
    [SerializeField] private GameObject[] neigbors;
    [SerializeField] private GameObject[] childrenOnTable;
    private WaitForSeconds _waitForActivationDuration = new WaitForSeconds(100f);
    private bool _activated;
    private int _randomIndex;

    public bool CanActivateIdleNeighbors => _building.Built && NeighborhoodManager.CanActivateIdleNeighbors && !_building.AccidentCauser.AccidentHappened && _building.Rentable.BuildingIsFull;

    public void Init(Building building)
    {
        _building = building;
        idleNeighborsParent.SetActive(false);
        _activated = false;

        _building.AccidentCauser.OnAccidentHappened += Deactivate;
        NeighborhoodEvents.OnActivateIdleNeighbor += Activate;

        StartCoroutine(RequestActivation());
    }

    private void OnDisable()
    {
        NeighborhoodEvents.OnActivateIdleNeighbor -= Activate;
        _building.AccidentCauser.OnAccidentHappened -= Deactivate;
    }

    private IEnumerator RequestActivation()
    {
        while (GameManager.GameState != GameState.Finished)
        {
            if (_activated)
            {
                yield return _waitForActivationDuration;
                Deactivate(_building);
            }
            else
            {
                yield return new WaitForSeconds(Random.Range(5, 30));

                if (CanActivateIdleNeighbors)
                    Request();
            }
        }
    }

    private void Request()
    {
        if (CanActivateIdleNeighbors)
            NeighborhoodEvents.OnRequestIdleNeighborActivation?.Invoke(_building);
    }

    private void Activate(Building building)
    {
        if (building != _building) return;

        idleNeighborsParent.SetActive(true);
        _activated = true;

        RandomizeNeighbors();
        RandomizeChildrenOnTable();
    }

    private void Deactivate(Building building)
    {
        if (building != _building) return;

        if (_activated)
        {
            idleNeighborsParent.SetActive(false);
            _activated = false;

            NeighborhoodEvents.OnDeactivateIdleNeighbor?.Invoke(_building);
        }
    }

    private void RandomizeNeighbors()
    {
        _randomIndex = Random.Range(0, 2);
        for (int i = 0; i < neigbors.Length; i++)
        {
            if (i == _randomIndex)
                neigbors[i].SetActive(true);
            else
                neigbors[i].SetActive(false);
        }
    }
    private void RandomizeChildrenOnTable()
    {
        _randomIndex = Random.Range(0, 2);
        for (int i = 0; i < childrenOnTable.Length; i++)
        {
            if (i == _randomIndex)
                childrenOnTable[i].SetActive(true);
            else
                childrenOnTable[i].SetActive(false);
        }
    }
}
