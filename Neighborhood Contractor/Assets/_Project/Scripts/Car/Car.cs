using UnityEngine;
using System;
using ZestGames.AI;
using ZestGames.Utility;
using DG.Tweening;

public class Car : MonoBehaviour
{
    private Animator animator;
    public Animator Animator => animator == null ? animator = GetComponent<Animator>() : animator;

    [Header("-- SETUP --")]
    [SerializeField] private Transform startTransform;
    [SerializeField] private float speed = 10f;
    [SerializeField] private Transform[] spawnPoints;

    [Header("-- NEIGHBOR SPAWN SETUP --")]
    private House _activatorHouse;
    private int _neighborsToSpawnCount = 0;
    private bool _hasMan, _hasWoman, _hasChild;

    private Vector3 _targetPosition, _turnPosition;
    private bool _startMoving, _targetReached, _turnReached;
    private float _idlingTime = 2f;

    private MeshRenderer[] _meshes;

    public event Action OnStartedMoving, OnStartedIdling;

    private void Init()
    {
        if (_meshes == null)
            _meshes = GetComponentsInChildren<MeshRenderer>();

        DisableMesh();

        _hasMan = _hasWoman = _hasChild = _startMoving = _targetReached = _turnReached = false;

        CarManager.AddFreeCar(this);
    }

    private void OnEnable()
    {
        Init();
    }

    private void Update()
    {
        if (_startMoving)
        {
            if (_targetReached)
            {
                transform.Translate(transform.forward * speed * Time.deltaTime, Space.World);
                Delayer.DoActionAfterDelay(this, 5f, Init);
            }
            else
            {
                if (_turnReached)
                {
                    Movement.MoveTransform(transform, _targetPosition, speed);
                    Movement.LookAtTarget(transform, _targetPosition, 7f);
                    if (Movement.IsTargetReached(transform, _targetPosition) && !_targetReached)
                    {
                        _targetReached = true;
                        OnStartedIdling?.Invoke();
                        _startMoving = false;
                        Delayer.DoActionAfterDelay(this, _idlingTime, () => _startMoving = true);
                        Delayer.DoActionAfterDelay(this, _idlingTime, () => OnStartedMoving?.Invoke());

                        for (int i = 0; i < _neighborsToSpawnCount; i++)
                        {
                            SpawnNeighbor();
                        }
                    }
                }
                else
                {
                    Movement.MoveTransform(transform, _turnPosition, speed);
                    Movement.LookAtTarget(transform, _turnPosition, 7f);
                    if (Movement.IsTargetReached(transform, _turnPosition) && !_turnReached)
                        _turnReached = true;
                }
            }
        }
    }

    public void StartTheCar(House house)
    {
        _activatorHouse = house;
        _neighborsToSpawnCount = _activatorHouse.Building.Rentable.RentableSpace;

        CarManager.RemoveFreeCar(this);
        _turnPosition = house.TurnPosition;
        _targetPosition = house.TargetPosition;

        EnableMesh();
        Bounce();

        transform.position = startTransform.position;
        _startMoving = true;
        OnStartedMoving?.Invoke();
    }

    private void SpawnNeighbor()
    {
        Neighbor neighbor;
        if (!_hasMan)
        {
            neighbor = ObjectPooler.Instance.SpawnFromPool("Neighbor_Man", spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)].position, Quaternion.identity).GetComponent<Neighbor>();
            neighbor.OnSpawned?.Invoke(_activatorHouse.transform.position);

            _hasMan = true;
        }
        else if (!_hasWoman)
        {
            neighbor = ObjectPooler.Instance.SpawnFromPool("Neighbor_Woman", spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)].position, Quaternion.identity).GetComponent<Neighbor>();
            neighbor.OnSpawned?.Invoke(_activatorHouse.transform.position);

            _hasWoman = true;
        }
        else if (!_hasChild)
        {
            neighbor = ObjectPooler.Instance.SpawnFromPool("Neighbor_Child", spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)].position, Quaternion.identity).GetComponent<Neighbor>();
            neighbor.OnSpawned?.Invoke(_activatorHouse.transform.position);

            _hasChild = true;
        }
        else if (_hasChild)
        {
            neighbor = ObjectPooler.Instance.SpawnFromPool("Neighbor_Child", spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)].position, Quaternion.identity).GetComponent<Neighbor>();
            neighbor.OnSpawned?.Invoke(_activatorHouse.transform.position);

            _hasMan = _hasWoman = _hasChild = false;
        }
    }

    private void Bounce()
    {
        transform.DORewind();

        //transform.DOShakePosition(.25f, .25f);
        transform.DOShakeRotation(.25f, .5f);
        transform.DOShakeScale(.25f, .5f);
    }

    private void EnableMesh()
    {
        for (int i = 0; i < _meshes.Length; i++)
            _meshes[i].enabled = true;
    }

    private void DisableMesh()
    {
        transform.position = startTransform.position;

        for (int i = 0; i < _meshes.Length; i++)
            _meshes[i].enabled = false;
    }
}
