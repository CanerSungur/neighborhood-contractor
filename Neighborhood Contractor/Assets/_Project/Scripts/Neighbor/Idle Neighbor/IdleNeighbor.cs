using UnityEngine;
using UnityEngine.AI;
using System;
using System.Collections;

public class IdleNeighbor : MonoBehaviour
{
    private IdleNeighborAnimController _animationController;
    private IdleNeighborMovement _movement;
    private enum Decision { Idle, Wander, Action }
    private Decision _decision;
    private WaitForSeconds _waitForADelay = new WaitForSeconds(10f);

    [Header("-- SETUP --")]
    [SerializeField] private Transform[] positions;

    #region Public Properties
    public Animator Animator { get; private set; }
    public NavMeshAgent Agent { get; private set; }
    public Transform[] Positions => positions;
    #endregion

    public Action OnStartIdling, OnStartWandering, OnStartRandomAction;

    private void Init()
    {
        _animationController = GetComponent<IdleNeighborAnimController>();
        _animationController.Init(this);
        _movement = GetComponent<IdleNeighborMovement>();
        _movement.Init(this);

        Animator = GetComponent<Animator>();
        Agent = GetComponent<NavMeshAgent>();

        _decision = Decision.Idle;
        StartIdling();

        OnStartIdling += StartIdling;
        OnStartWandering += StartWandering;
        OnStartRandomAction += StartRandomAction;

        StartCoroutine(MakeDecision());
    }

    private void OnEnable()
    {
        Init();
    }

    private void OnDisable()
    {
        OnStartIdling -= StartIdling;
        OnStartWandering -= StartWandering;
        OnStartRandomAction -= StartRandomAction;
    }

    private void StartIdling()
    {
        Debug.Log("Start Idling..");
    }

    private void StartWandering()
    {
        Debug.Log("Wandering...");
    }

    private void StartRandomAction()
    {
        Debug.Log("Doing Something...");
    }

    private IEnumerator MakeDecision()
    {
        yield return _waitForADelay;

        _decision = (Decision)UnityEngine.Random.Range(0, 3);
        switch (_decision)
        {
            case Decision.Idle:
                OnStartIdling?.Invoke();
                break;
            case Decision.Wander:
                OnStartWandering?.Invoke();                
                break;
            case Decision.Action:
                OnStartRandomAction?.Invoke();
                break;
        }
    }
}
