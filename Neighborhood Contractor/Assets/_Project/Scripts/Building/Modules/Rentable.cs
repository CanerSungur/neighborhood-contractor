using System.Collections;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;
using ZestGames.Utility;

[RequireComponent(typeof(Building))]
public class Rentable : MonoBehaviour
{
    private Building building;
    public Building Building => building == null ? building = GetComponent<Building>() : building;

    [Header("-- SETUP --")]
    [SerializeField] private int maxBuildingPopulation = 4;
    [SerializeField] private int populationIncreaseCount = 2;
    [SerializeField] private GameObject rentSign;
    [SerializeField] private Animation rentSignFullAnimation;
    private int _currentBuildingPopulation, _rentableSpace;

    [Header("-- UI --")]
    [SerializeField] private GameObject rentUI;
    [SerializeField] private Image bubbleImg;
    [SerializeField] private TextMeshProUGUI populationText;
    [SerializeField] private Color maxxedColor;

    public int MaxBuildingPopulation => maxBuildingPopulation;
    public int CurrentBuildingPopulation => _currentBuildingPopulation;
    public bool BuildingIsFull => _currentBuildingPopulation == maxBuildingPopulation;
    public int RentableSpace => _rentableSpace;
    public int RentIncreaseCount => populationIncreaseCount;

    public void Init()
    {
        rentSign.SetActive(false);
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

        rentSign.transform.DOKill();
        rentUI.transform.DOKill();
        bubbleImg.transform.DOKill();
    }

    public void UpdateProperties()
    {
        rentUI.transform.DORewind();
        Delayer.DoActionAfterDelay(this, 1f, () => rentUI.transform.DOLocalMoveY(rentUI.transform.localPosition.y + 0.75f, 0.5f).SetEase(Ease.InOutSine));
        DOVirtual.Color(bubbleImg.color, Color.white, 0.5f, r => {
            bubbleImg.color = r;
        }).SetEase(Ease.OutBounce);

        rentSignFullAnimation.Rewind();
        rentSignFullAnimation.Play("RentSign_NotFull_LegacyAnim");
        maxBuildingPopulation += populationIncreaseCount;
        _rentableSpace = populationIncreaseCount;
        populationText.text = $"{_currentBuildingPopulation}/{maxBuildingPopulation}";

        //StartCoroutine(StartRenting());
    }

    public void BuildingIsFinished()
    {
        rentSign.SetActive(true);
        rentSignFullAnimation.Rewind();
        rentSignFullAnimation.Play("RentSign_NotFull_LegacyAnim");

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

    public void RentForLoad()
    {
        if (!BuildingIsFull)
        {
            Bounce(rentUI.transform);
            Bounce(rentSign.transform);

            _currentBuildingPopulation++;
            //NeighborhoodEvents.OnIncreasePopulation?.Invoke(1);
            NeighborhoodEvents.OnCheckForPopulationSufficiency?.Invoke();
            populationText.text = $"{_currentBuildingPopulation}/{maxBuildingPopulation}";

            if (_currentBuildingPopulation == 1)
                Building.IncomeSpawner.StartSpawningIncome();

            Building.IncomeSpawner.UpdateIncomeForRent();

            if (_currentBuildingPopulation == maxBuildingPopulation)
            {
                DOVirtual.Color(bubbleImg.color, maxxedColor, 0.5f, r => {
                    bubbleImg.color = r;
                }).SetEase(Ease.OutBounce);

                // Do something for Rent Sign.
                rentSignFullAnimation.Play("RentSign_Full_LegacyAnim");
            }
        }
    }

    public void Rented()
    {
        if (!BuildingIsFull)
        {
            Bounce(rentUI.transform);
            Bounce(rentSign.transform);

            _currentBuildingPopulation++;
            NeighborhoodEvents.OnIncreasePopulation?.Invoke(1);
            NeighborhoodEvents.OnCheckForPopulationSufficiency?.Invoke();
            populationText.text = $"{_currentBuildingPopulation}/{maxBuildingPopulation}";

            if (_currentBuildingPopulation == 1)
                Building.IncomeSpawner.StartSpawningIncome();
            
            Building.IncomeSpawner.UpdateIncomeForRent();

            if (_currentBuildingPopulation == maxBuildingPopulation)
            {
                DOVirtual.Color(bubbleImg.color, maxxedColor, 0.5f, r => {
                    bubbleImg.color = r;
                }).SetEase(Ease.OutBounce);

                // Do something for Rent Sign.
                rentSignFullAnimation.Play("RentSign_Full_LegacyAnim");
            }
        }
    }

    private void Bounce(Transform transform)
    {
        transform.DORewind();

        //transform.DOShakePosition(.25f, .25f);
        transform.DOShakeRotation(.25f, .5f);
        transform.DOShakeScale(.25f, .5f);
    }
}
