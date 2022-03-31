using System.Collections;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;

[RequireComponent(typeof(Building))]
public class Rentable : MonoBehaviour
{
    private Building building;
    public Building Building => building == null ? building = GetComponent<Building>() : building;

    [Header("-- SETUP --")]
    [SerializeField] private int maxBuildingPopulation = 4;
    [SerializeField] private int populationIncreaseCount = 2;
    private int _currentBuildingPopulation, _rentableSpace;

    [Header("-- UI --")]
    [SerializeField] private GameObject rentUI;
    [SerializeField] private Image bubbleImg;
    [SerializeField] private TextMeshProUGUI populationText;
    [SerializeField] private Color maxxedColor;

    public int CurrentBuildingPopulation => _currentBuildingPopulation;
    public bool BuildingIsFull => _currentBuildingPopulation == maxBuildingPopulation;
    public int RentableSpace => _rentableSpace;

    public void Init()
    {
        rentUI.SetActive(false);
        _currentBuildingPopulation = 0;
        bubbleImg.color = Color.white;

        Building.Buildable.OnBuildFinished += BuildingIsFinished;
        Building.Upgradeable.OnUpgradeHappened += UpdateProperties;
    }

    private void OnDisable()
    {
        Building.Buildable.OnBuildFinished -= BuildingIsFinished;
        Building.Upgradeable.OnUpgradeHappened -= UpdateProperties;

        rentUI.transform.DOKill();
        bubbleImg.transform.DOKill();
    }

    private void UpdateProperties()
    {
        rentUI.transform.DOLocalMoveY(rentUI.transform.localPosition.y + 1, 0.5f).SetEase(Ease.InOutSine);
        DOVirtual.Color(bubbleImg.color, Color.white, 0.5f, r => {
            bubbleImg.color = r;
        }).SetEase(Ease.OutBounce);

        maxBuildingPopulation += populationIncreaseCount;
        _rentableSpace = populationIncreaseCount;
        populationText.text = $"{_currentBuildingPopulation}/{maxBuildingPopulation}";

        //StartCoroutine(StartRenting());
    }

    private void BuildingIsFinished()
    {
        rentUI.SetActive(true);
        populationText.text = $"{_currentBuildingPopulation}/{maxBuildingPopulation}";
        _rentableSpace = maxBuildingPopulation;
        //StartCoroutine(StartRenting());
    }

    private IEnumerator StartRenting()
    {
        yield return new WaitForSeconds(3f);

        while (!BuildingIsFull)
        {
            Rented();
            yield return new WaitForSeconds(2f);
        }
    }

    public void Rented()
    {
        if (!BuildingIsFull)
        {
            Bounce();

            _currentBuildingPopulation++;
            NeighborhoodEvents.OnIncreasePopulation?.Invoke(1);
            NeighborhoodEvents.OnCheckForPopulationSufficiency?.Invoke();
            populationText.text = $"{_currentBuildingPopulation}/{maxBuildingPopulation}";

            if (_currentBuildingPopulation == 1)
                Building.IncomeSpawner.StartSpawningIncome();
            
            Building.IncomeSpawner.UpdateIncomeForRent();

            if (_currentBuildingPopulation == maxBuildingPopulation)
                DOVirtual.Color(bubbleImg.color, maxxedColor, 0.5f, r => {
                    bubbleImg.color = r;
                }).SetEase(Ease.OutBounce);
        }
    }

    private void Bounce()
    {
        rentUI.transform.DORewind();

        //transform.DOShakePosition(.25f, .25f);
        rentUI.transform.DOShakeRotation(.25f, .25f);
        rentUI.transform.DOShakeScale(.25f, .25f);
    }
}
