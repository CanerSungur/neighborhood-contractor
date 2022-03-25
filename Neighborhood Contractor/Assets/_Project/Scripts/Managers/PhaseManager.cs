using System;
using System.Collections.Generic;
using UnityEngine;

public class PhaseManager : MonoBehaviour
{
    [Header("-- SETUP --")]
    [SerializeField] private List<GameObject> phases;
    [SerializeField] private List<GameObject> phaseUnlockers;

    public static int CurrentPhase { get; private set; }

    private void Init()
    {
        CurrentPhase = 1;

        EnableUnlockedPhases(CurrentPhase);
    }

    private void Awake()
    {
        Init();

        NeighborhoodEvents.OnEnableThisPhase += HandlePhaseEnabling;
    }

    private void OnDisable()
    {
        NeighborhoodEvents.OnEnableThisPhase -= HandlePhaseEnabling;
    }

    private void HandlePhaseEnabling(PhaseUnlocker unlocker, int phaseNumberToUnlock)
    {
        unlocker.gameObject.SetActive(false);
        phases[phaseNumberToUnlock - 1].SetActive(true);
        phaseUnlockers[phaseNumberToUnlock - 1].SetActive(true);
        CurrentPhase++;

        NeighborhoodEvents.OnNewPhaseActivated?.Invoke();
    }

    private void EnableUnlockedPhases(int currentPhase)
    {
        phases[0].SetActive(true);
        phaseUnlockers[0].SetActive(true);

        for (int i = 1; i < currentPhase; i++)
        {
            phases[i].SetActive(true);
            phaseUnlockers[i].SetActive(true);
        }
    }
}
