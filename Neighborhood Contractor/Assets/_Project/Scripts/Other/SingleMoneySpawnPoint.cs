using System.Collections;
using UnityEngine;

public class SingleMoneySpawnPoint : MonoBehaviour
{
    [Header("-- SETUP --")]
    [SerializeField] private float spawnDelay = 2f;
    private float _nextSpawnTime;
    private bool _isSpawned = false;
    private bool _stopSpawning = false;
    private WaitForSeconds _waitForSpawnDelay;
    private Money _spawnedMoney = null;

    private void Start()
    {
        _waitForSpawnDelay = new WaitForSeconds(spawnDelay);
        _nextSpawnTime = Time.time + spawnDelay;
    }

    private void Update()
    {
        if (_stopSpawning) return;

        if (!_isSpawned && Time.time > _nextSpawnTime)
        {
            _spawnedMoney = ObjectPooler.Instance.SpawnFromPool("Money_Collect", transform.position, Quaternion.identity).GetComponent<Money>();
            _isSpawned = true;
        }

        CheckIfMoneyIsCollected(_spawnedMoney);
    }

    private void CheckIfMoneyIsCollected(Money money)
    {
        if (_isSpawned && money && money.Collected)
        {
            _isSpawned = false;
            _nextSpawnTime = Time.time + spawnDelay;
        }
    }

    private void SpawnedMoneyIsCollected() => _isSpawned = false;
}
