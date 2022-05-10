using UnityEngine;
using UnityEngine.AI;
using System;

public class IdleNeighbor : MonoBehaviour
{
    private IdleNeighborAnimController _animationController;
    private IdleNeighborMovement _movement;

    #region Public Properties
    public Animator Animator { get; private set; }
    public NavMeshAgent Agent { get; private set; }
    #endregion

    public Action OnStartWandering, OnStartRandomAction;

    private void Init()
    {
        _animationController = GetComponent<IdleNeighborAnimController>();
        _animationController.Init(this);
        _movement = GetComponent<IdleNeighborMovement>();
        _movement.Init(this);

        Animator = GetComponent<Animator>();
        Agent = GetComponent<NavMeshAgent>();

        StartIdling();
    }

    private void OnEnable()
    {
        Init();
    }

    private void StartIdling()
    {
        Debug.Log("Start Idling..");
    }

    private void StartWandering()
    {

    }

    private void StartRandomAction()
    {

    }
}
