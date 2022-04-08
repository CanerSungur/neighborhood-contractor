using System.Collections.Generic;
using UnityEngine;

public class PhaseManager : MonoBehaviour
{
    private SavePhaseData _savePhaseData;
    private bool _deleteSaveData = false;

    [Header("-- SETUP --")]
    [SerializeField] private List<GameObject> phases;
    [SerializeField] private List<GameObject> phaseUnlockers;

    public static int CurrentPhase { get; private set; }
    public static int CurrentlyConsumedMoney { get; set; }

    private void Init()
    {
        _savePhaseData = LoadHandler.LoadPhaseData();
        CurrentPhase = _savePhaseData.CurrentPhase;
        CurrentlyConsumedMoney = _savePhaseData.CurrentlyConsumedMoney;
        if (CurrentPhase - 1 <= phaseUnlockers.Count - 1)
            ZestGames.Utility.Delayer.DoActionAfterDelay(this, 0.5f, () => phaseUnlockers[CurrentPhase - 1].GetComponent<PhaseUnlocker>().UpdateConsumedMoney());

        //CurrentPhase = 1;

        EnableUnlockedPhases(CurrentPhase);
    }

    private void OnApplicationPause(bool pause)
    {

        if ((CurrentPhase - 1) <= (phaseUnlockers.Count - 1))
            CurrentlyConsumedMoney = phaseUnlockers[CurrentPhase - 1].GetComponent<PhaseUnlocker>().ConsumedMoney;
        else
            CurrentlyConsumedMoney = 0;
        SaveHandler.SavePhaseData(this);

        if (_deleteSaveData)
            PlayerPrefs.DeleteAll();
    }

    private void OnApplicationQuit()
    {
        if ((CurrentPhase - 1) <= (phaseUnlockers.Count - 1))
            CurrentlyConsumedMoney = phaseUnlockers[CurrentPhase - 1].GetComponent<PhaseUnlocker>().ConsumedMoney;
        else
            CurrentlyConsumedMoney = 0;
        SaveHandler.SavePhaseData(this);

        if (_deleteSaveData)
            PlayerPrefs.DeleteAll();
    }

    private void Awake()
    {
        Init();

        NeighborhoodEvents.OnEnableThisPhase += HandlePhaseEnabling;
    }

    private void Start()
    {
        _deleteSaveData = GameManager.Instance.DeleteSaveGame;

        AnalyticEvents.OnPhaseLoaded?.Invoke(CurrentPhase);
        //AnalyticEvents.OnPhaseStarted?.Invoke(CurrentPhase);
    }

    private void OnDisable()
    {
        NeighborhoodEvents.OnEnableThisPhase -= HandlePhaseEnabling;
    }

    private void HandlePhaseEnabling(PhaseUnlocker unlocker, int phaseNumberToUnlock)
    {
        unlocker.gameObject.SetActive(false);
        
        if (phaseNumberToUnlock <= phases.Count)
            phases[phaseNumberToUnlock - 1].SetActive(true);

        if (phaseNumberToUnlock <= phaseUnlockers.Count)
            phaseUnlockers[phaseNumberToUnlock - 1].SetActive(true);

        AnalyticEvents.OnPhaseFinished?.Invoke(CurrentPhase);
        CurrentPhase++;
        AnalyticEvents.OnPhaseStarted?.Invoke(CurrentPhase);

        NeighborhoodEvents.OnNewPhaseActivated?.Invoke();
    }

    private void EnableUnlockedPhases(int currentPhase)
    {
        for (int i = 0; i < phases.Count; i++)
        {
            if (i <= currentPhase - 1)
                phases[i].SetActive(true);
            else
                phases[i].SetActive(false);
        }

        for (int i = 0; i < phaseUnlockers.Count; i++)
        {
            if (i == currentPhase - 1)
                phaseUnlockers[i].SetActive(true);
            else
                phaseUnlockers[i].SetActive(false);
        }

        //phases[0].SetActive(true);
        //phaseUnlockers[0].SetActive(true);

        //for (int i = 1; i < currentPhase; i++)
        //{
        //    phases[i].SetActive(true);
        //    phaseUnlockers[i].SetActive(true);
        //}
    }
}
