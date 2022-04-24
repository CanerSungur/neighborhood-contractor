using UnityEngine;
using ZestGames.Utility;

/// <summary>
/// Attach this to a child of building called Accidents which holds all accident objects.
/// </summary>
public class AccidentHandler : MonoBehaviour
{
    private AccidentCauser _accidentCauser;

    public enum Accident { Fire, Flood}
    private Accident _currentAccident;
    public Accident CurrentAccident => _currentAccident;

    [Header("-- SETUP --")]
    [SerializeField] private ParticleSystem[] fires;
    [SerializeField] private ParticleSystem[] floods;

    public void Init(AccidentCauser accidentCauser)
    {
        _accidentCauser = accidentCauser;

        for (int i = 0; i < fires.Length; i++)
        {
            fires[i].gameObject.SetActive(false);
            floods[i].gameObject.SetActive(false);
        }

        NeighborhoodEvents.OnAccidentHappened += EnableAccident;
        NeighborhoodEvents.OnBuildingRepaired += DisableAccident;
    }

    private void OnDisable()
    {
        NeighborhoodEvents.OnAccidentHappened -= EnableAccident;
        NeighborhoodEvents.OnBuildingRepaired -= DisableAccident;
    }

    private void EnableAccident(Building building)
    {
        if (building != _accidentCauser.Building) return;

        GetRandomAccident();

        if (_currentAccident == Accident.Fire)
        {
            fires[building.CurrentLevel - 1].gameObject.SetActive(true);
            fires[building.CurrentLevel - 1].Play();

            AccidentEvents.OnFireStarted?.Invoke(_accidentCauser.Building);
        }
        else if (_currentAccident == Accident.Flood)
        {
            floods[building.CurrentLevel - 1].gameObject.SetActive(true);
            floods[building.CurrentLevel - 1].Play();

            AccidentEvents.OnFloodStarted?.Invoke(_accidentCauser.Building);
        }
    }

    private void DisableAccident(Building building)
    {
        if (building != _accidentCauser.Building) return;

        if (_currentAccident == Accident.Fire)
        {
            fires[building.CurrentLevel - 1].Stop();
            Delayer.DoActionAfterDelay(this, 10f, () => fires[building.CurrentLevel - 1].gameObject.SetActive(false));
        }
        else if (_currentAccident == Accident.Flood)
        {
            floods[building.CurrentLevel - 1].Stop();
            Delayer.DoActionAfterDelay(this, 10f, () => floods[building.CurrentLevel - 1].gameObject.SetActive(false));
        }
    }

    private void GetRandomAccident()
    {
        if (RNG.RollDice(5))
            _currentAccident = Accident.Fire;
        else
            _currentAccident = Accident.Flood;
    }
}
