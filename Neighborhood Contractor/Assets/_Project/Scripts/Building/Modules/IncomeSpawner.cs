using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZestGames.Utility;
using TMPro;

[RequireComponent(typeof(Building))]
public class IncomeSpawner : MonoBehaviour
{
    private SaveIncomeSpawnerData _saveIncomeSpawnerData;
    private bool _deleteSaveData = false;

    private Building building;
    public Building Building => building == null ? building = GetComponent<Building>() : building;

    [Header("-- SETUP --")]
    [SerializeField, Tooltip("Delay in seconds for rentable buildings.")] private float incomeTimeForRentable = 11f;
    [SerializeField, Tooltip("Delay in seconds for not rentable buildings.")] private float incomeTimeForNotRentable = 3f;
    [SerializeField, Tooltip("Delay in seconds for first start of spawning income money. ")] private float startSpawnDelay = 1f;
    private List<Money> incomeMoney = new List<Money>();
    private WaitForSeconds _waitForIncomeTime, _waitForSpawnStartDelay;
    private float _incomePerSecond, _currentIncomeTime;

    [Header("-- SPAWN POSITION SETUP --")]
    [SerializeField, Tooltip("First spawn transform of income money.")] private Transform spawnStartTransform;
    private float layerOffset = 0.15f;
    private float rowOffset = 0.72f;
    private float columnOffset = 0.38f;

    [Header("-- SPAWN LIMITS SETUP --")]
    [SerializeField, Tooltip("Spawn position will move on to the next column if this number is reached.")] private int rowLength = 4;
    [SerializeField, Tooltip("Spawn position will move on to the next layer if this number is reached.")] private int columnLength = 3;
    [SerializeField, Tooltip("Spawning money will stop if this layer is reached.")] private int layerLength = 5;
    private int _currentFinishedRow = 0;
    private int _currentFinishedColumn = 0;
    private int _currentFinishedLayer = 0;
    private bool _canSpawn;

    [Header("-- REFERENCES --")]
    [SerializeField] private GameObject incomeArea;
    [SerializeField] private TextMeshProUGUI incomePerSecondText;

    [Header("-- FOR LOADING --")]
    [SerializeField] private GameObject moneyForLoading;

    #region Properties

    public bool PlayerIsInArea { get; set; }
    public int MoneyCount { get; private set; }
    public List<Money> IncomeMoney => incomeMoney;
    public float IncomePerSecond => _incomePerSecond;
    public bool CanCollectIncome => incomeMoney.Count != 0 && MoneyCount > 0 && StatManager.CurrentCarry < StatManager.CarryCapacity;

    #endregion

    public void Init()
    {
        if (Building.Rentable)
            _currentIncomeTime = incomeTimeForRentable;
        else
            _currentIncomeTime = incomeTimeForNotRentable;

        PlayerIsInArea = false;
        incomeMoney.Clear();
        Delayer.DoActionAfterDelay(this, 0.4f, CalculateIncomeTime);
        _waitForSpawnStartDelay = new WaitForSeconds(startSpawnDelay);
        _canSpawn = true;
        incomeArea.SetActive(false);

        MoneyCount = 0;

        Delayer.DoActionAfterDelay(this, 0.5f, () => ValueBarEvents.OnValueLevelIncrease += CalculateIncomeTime);

        _saveIncomeSpawnerData = LoadHandler.LoadIncomeSpawnerData(this);
        MoneyCount = _saveIncomeSpawnerData.MoneyCount;

        _deleteSaveData = GameManager.Instance.DeleteSaveGame;
    }

    public void CheckIfThisHasMoney()
    {
        for (int i = 0; i < MoneyCount; i++)
        {
            RowFinishedCheckForSpawn();
            ColumnFinishedCheckForSpawn();
            LayerFinishedCheckForSpawn();

            var spawnPoint = spawnStartTransform.position + new Vector3(_currentFinishedRow * -rowOffset, _currentFinishedLayer * layerOffset, _currentFinishedColumn * -columnOffset);
            Money mny = Instantiate(moneyForLoading, spawnPoint, Quaternion.Euler(0f, 90f, 0f)).GetComponent<Money>();
            AddIncomeMoney(mny);
            _currentFinishedRow++;
        }
    }

    public void CheckThisState()
    {
        incomeArea.SetActive(true);
        CalculateIncomeTime();
        incomePerSecondText.text = $"{_incomePerSecond:#,##0}$";

        CheckIfThisHasMoney();
    }

    private void OnApplicationPause(bool pause)
    {
        SaveHandler.SaveIncomeSpawnerData(this);

        if (_deleteSaveData)
            PlayerPrefs.DeleteAll();
    }

    private void OnApplicationQuit()
    {
        SaveHandler.SaveIncomeSpawnerData(this);

        if (_deleteSaveData)
            PlayerPrefs.DeleteAll();
    }

    private void OnDisable()
    {
        ValueBarEvents.OnValueLevelIncrease -= CalculateIncomeTime;
    }

    public void StartSpawningIncome()
    {
        incomeArea.SetActive(true);
        incomePerSecondText.text = $"{_incomePerSecond:#,##0}$";

        StartCoroutine(SpawnIncomeMoney());
    }
        
    public void WaitForRent()
    {
        incomeArea.SetActive(true);
        incomePerSecondText.text = $"{0:#,##0}$";
    }

    public void UpdateIncomeForRent()
    {
        _currentIncomeTime--;
        CalculateIncomeTime();
    }

    private IEnumerator SpawnIncomeMoney()
    {
        yield return _waitForSpawnStartDelay;

        // This always has to work.
        while (true)
        {
            if (_canSpawn)
            {
                RowFinishedCheckForSpawn();
                ColumnFinishedCheckForSpawn();
                LayerFinishedCheckForSpawn();

                Spawn();
            }

            yield return _waitForIncomeTime;
        }
    }

    private void Spawn()
    {
        var spawnPoint = spawnStartTransform.position + new Vector3(_currentFinishedRow * -rowOffset, _currentFinishedLayer * layerOffset, _currentFinishedColumn * -columnOffset);
        Money money = ObjectPooler.Instance.SpawnFromPool("Money_Income", spawnPoint, Quaternion.Euler(0f, 90f, 0f)).GetComponent<Money>();
        AddIncomeMoney(money);
        NextSpawnPosition();
    }

    public void AddIncomeMoney(Money money)
    {
        if (!incomeMoney.Contains(money))
            incomeMoney.Add(money);
    }

    public void RemoveIncomeMoney(Money money)
    {
        if (incomeMoney.Contains(money))
        {
            incomeMoney.Remove(money);
            PreviousSpawnPosition();
        }
    }

    private void NextSpawnPosition()
    {
        MoneyCount++;
        _currentFinishedRow++;
    }

    private void PreviousSpawnPosition()
    {
        _canSpawn = true;

        MoneyCount--;
        _currentFinishedRow--;

        if (_currentFinishedRow == 0)
        {
            if (_currentFinishedColumn > 0)
            {
                _currentFinishedColumn--;
                _currentFinishedRow = rowLength;
            }
            else if (_currentFinishedColumn == 0)
            {
                if (_currentFinishedLayer > 0)
                {
                    _currentFinishedLayer--;
                    _currentFinishedColumn = columnLength - 1;
                    _currentFinishedRow = rowLength;
                }
                else if (_currentFinishedLayer == 0)
                    _currentFinishedRow = 0;
            }
        }
    }

    #region Spawning Income Checks

    private void RowFinishedCheckForSpawn()
    {
        if (_currentFinishedRow == rowLength)
        {
            _currentFinishedColumn++;
            _currentFinishedRow = 0;
        }
    }

    private void ColumnFinishedCheckForSpawn()
    {
        if (_currentFinishedColumn == columnLength)
        {
            _currentFinishedLayer++;
            _currentFinishedColumn = 0;
        }
    }

    private void LayerFinishedCheckForSpawn()
    {
        if (_currentFinishedLayer == layerLength)
        {
            _canSpawn = false;
        }
    }

    #endregion

    #region Income Time Calculation

    private void CalculateIncomeTime()
    {
        _currentIncomeTime -= NeighborhoodManager.ValueSystem.ValueLevel * .1f;
        if (_currentIncomeTime < 0.05f)
            _currentIncomeTime = 0.05f;

        _waitForIncomeTime = new WaitForSeconds(_currentIncomeTime);

        CalculateIncomePerSecond();
        incomePerSecondText.text = $"{_incomePerSecond:#,##0}$";
    }
    private void CalculateIncomePerSecond() => _incomePerSecond = StatManager.MoneyValue / _currentIncomeTime;

    #endregion
}
