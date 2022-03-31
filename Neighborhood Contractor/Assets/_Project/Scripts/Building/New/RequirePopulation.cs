using UnityEngine;
using TMPro;
using System;

public class RequirePopulation : MonoBehaviour
{
    [Header("-- SETUP --")]
    [SerializeField] private int requiredPopulation = 2;
    [SerializeField] private GameObject lockedArea;
    [SerializeField] private TextMeshProUGUI requiredPopulationText;

    public bool PopulationIsEnough => NeighborhoodManager.Population >= requiredPopulation;

    public event Action OnPopulationSufficient;

    public void Init()
    {
        if (PopulationIsEnough)
        {
            lockedArea.SetActive(false);
            OnPopulationSufficient?.Invoke();
        }
        else
        {
            lockedArea.SetActive(true);
            requiredPopulationText.text = requiredPopulation.ToString();
        }

        NeighborhoodEvents.OnCheckForPopulationSufficiency += CheckForPopulationSufficiency;
    }

    private void OnDisable()
    {
        NeighborhoodEvents.OnCheckForPopulationSufficiency -= CheckForPopulationSufficiency;
    }

    private void CheckForPopulationSufficiency()
    {
        if (PopulationIsEnough)
        {
            lockedArea.SetActive(false);
            OnPopulationSufficient?.Invoke();
        }
    }
}
