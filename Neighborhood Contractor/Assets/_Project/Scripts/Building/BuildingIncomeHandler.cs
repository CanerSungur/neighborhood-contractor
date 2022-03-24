using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingIncomeHandler : MonoBehaviour
{
    private IContributorIncome incomeBuilding;
    public IContributorIncome IncomeBuilding => incomeBuilding == null ? incomeBuilding = GetComponent<IContributorIncome>() : incomeBuilding;

    [Header("-- INCOME SETUP --")]
    [SerializeField, Tooltip("Delay in seconds for collecting income money.")] private float incomeTime = 3f;
    [SerializeField, Tooltip("Delay in seconds for first start of spawning income money. ")] private float startSpawnDelay = 1f;
    private List<Money> incomeMoney = new List<Money>();
    private WaitForSeconds _waitForIncomeTime, _waitForSpawnStartDelay;

    [Header("-- SPAWN POSITION SETUP --")]
    [SerializeField, Tooltip("First spawn position of income money.")] private Vector3 startPoint = new Vector3(0f, 0f, 5f);
    private float layerOffset = 0.15f;

    [Header("-- SPAWN LIMITS SETUP --")]
    [SerializeField, Tooltip("Spawn position will move on to the next column if this number is reached.")] private int rowLength = 4;
    [SerializeField, Tooltip("Spawn position will move on to the next layer if this number is reached.")] private int columnLength = 3;
    [SerializeField, Tooltip("Spawning money will stop if this layer is reached.")] private int layerLength = 5;
    private int _currentFinishedRow = 0;
    private int _currentFinishedColumn = 0;
    private int _currentFinishedLayer = 0;
    private bool _canSpawn;

    #region Properties

    public int MoneyCount { get; private set; }
    public List<Money> IncomeMoney => incomeMoney;

    #endregion

    private void Init()
    {
        incomeMoney.Clear();
        _waitForIncomeTime = new WaitForSeconds(incomeTime);
        _waitForSpawnStartDelay = new WaitForSeconds(startSpawnDelay);
        _canSpawn = true;

        MoneyCount = 0;
    }

    private void Start()
    {
        Init();

        IncomeBuilding.OnStartSpawningIncome += () => StartCoroutine(SpawnIncomeMoney());
    }

    private void OnDisable()
    {
        IncomeBuilding.OnStartSpawningIncome -= () => StartCoroutine(SpawnIncomeMoney());
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
        var spawnPoint = startPoint + new Vector3(_currentFinishedRow * -0.72f, _currentFinishedLayer * layerOffset, _currentFinishedColumn * -0.38f);
        Money money = ObjectPooler.Instance.SpawnFromPool("Money_Income", spawnPoint, Quaternion.Euler(0f, 90f, 0f)).GetComponent<Money>();
        AddIncomeMoney(money);
    }

    private void NextSpawnPosition()
    {
        MoneyCount++;
        _currentFinishedRow++;
    }

    private void PreviousSpawnPosition()
    {
        MoneyCount--;
        _currentFinishedRow--;

        if (_currentFinishedRow == 0 && _currentFinishedColumn > 0)
        {
            _currentFinishedColumn--;
            _currentFinishedRow = rowLength;
        }

        if (_currentFinishedColumn == 0 && _currentFinishedLayer > 0)
        {
            _currentFinishedLayer--;
            _currentFinishedColumn = columnLength;
        }

        //if (_currentFinishedLayer == 0)
        //{
        //    _currentFinishedRow = 0;
        //    _currentFinishedColumn = 0;
        //}
    }

    #region Giving Income Checks

    //private void RowFinishedCheckForSpawn()
    //{
    //    if (_currentFinishedRow != 0 && _currentFinishedRow % rowLength == 0)
    //    {
    //        _currentFinishedColumn++;
    //        _currentFinishedRow = 0;
    //    }
    //}

    //private void ColumnFinishedCheckForSpawn()
    //{
    //    if (_currentFinishedColumn != 0 && _currentFinishedColumn % columnLength == 0)
    //    {
    //        _currentFinishedLayer++;
    //        _currentFinishedColumn = 0;
    //    }
    //}

    //private void LayerFinishedCheckForSpawn()
    //{
    //    if (_currentFinishedLayer == layerLength)
    //    {
    //        _canSpawn = false;
    //    }
    //}

    #endregion

    #region Spawning Income Checks

    private void RowFinishedCheckForSpawn()
    {
        if (_currentFinishedRow != 0 && _currentFinishedRow % rowLength == 0)
        {
            _currentFinishedColumn++;
            _currentFinishedRow = 0;
        }
    }

    private void ColumnFinishedCheckForSpawn()
    {
        if (_currentFinishedColumn != 0 && _currentFinishedColumn % columnLength == 0)
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

    public void AddIncomeMoney(Money money)
    {
        if (!incomeMoney.Contains(money))
        {
            incomeMoney.Add(money);
            NextSpawnPosition();
        }
    }

    public void RemoveIncomeMoney(Money money)
    {
        if (incomeMoney.Contains(money))
        {
            incomeMoney.Remove(money);
            PreviousSpawnPosition();
        }
    }
}
