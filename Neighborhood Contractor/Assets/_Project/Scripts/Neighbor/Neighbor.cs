using UnityEngine;
using System;
using ZestGames.Utility;
using DG.Tweening;

public class Neighbor : MonoBehaviour
{
    public enum Type { Happy, Complaining, Idle }
    [SerializeField] private Type currentType;

    [Header("-- REFERENCES --")]
    private NeighborMovement _movementHandler;
    private NeighborCollision _collisionHandler;
    private NeighborAnimationController _animationHandler;
    private NeighborComplain _complain;

    [Header("-- SETUP --")]
    private float waitAfterSpawn = 3f;
    private float speed = 2f;
    private Vector3 _targetPosition;

    public Animator Animator { get; private set; }
    public Collider Collider { get; private set; }
    public Rigidbody Rigidbody { get; private set; }
    public NeighborMovement MovementHandler => _movementHandler;
    public NeighborCollision CollisionHandler => _collisionHandler;
    public NeighborAnimationController AnimationHandler => _animationHandler;
    public Vector3 TargetPosition => _targetPosition;
    public float Speed => speed;
    public bool CanMove { get; private set; }
    public Type CurrentType => currentType;
    public Building RelatedBuilding { get; set; }

    public Action OnStartMoving, OnStartComplaining, OnStopComplaining;
    public Action<Vector3> OnSetTargetPos;

    private void Init()
    {
        Animator = GetComponent<Animator>();
        Collider = GetComponent<Collider>();
        Rigidbody = GetComponent<Rigidbody>();

        _movementHandler = GetComponent<NeighborMovement>();
        _movementHandler.Init(this);

        _collisionHandler = GetComponent<NeighborCollision>();
        _collisionHandler.Init(this);
        
        _animationHandler = GetComponent<NeighborAnimationController>();
        _animationHandler.Init(this);

        if (TryGetComponent(out _complain))
            _complain.Init(this);

        if (currentType == Type.Happy)
        {
            CanMove = false;
            Delayer.DoActionAfterDelay(this, waitAfterSpawn, () => CanMove = true);
            Delayer.DoActionAfterDelay(this, waitAfterSpawn, () => OnStartMoving?.Invoke());
        }
        else if (currentType == Type.Complaining)
        {
            CanMove = true;
            OnStartMoving?.Invoke();
            //RelatedBuilding.Repairable.OnBuildingRepaired += GoBackToTheHouse;
            NeighborhoodEvents.OnBuildingRepaired += GoBackToTheHouse;
        }
        else if (currentType == Type.Idle)
        {
            CanMove = true;
            StartIdling();
        }
        OnSetTargetPos += SetTargetPosition;
    }

    private void OnEnable()
    {
        Init();
        Bounce();
    }

    private void OnDisable()
    {
        NeighborhoodEvents.OnBuildingRepaired += GoBackToTheHouse;
        //if (currentType == Type.Complaining && RelatedBuilding)
        //    RelatedBuilding.Repairable.OnBuildingRepaired -= GoBackToTheHouse;

        OnSetTargetPos -= SetTargetPosition;
    }

    private void SetTargetPosition(Vector3 targetPos) => _targetPosition = targetPos;

    private void Bounce()
    {
        transform.DORewind();

        //transform.DOShakePosition(.25f, .25f);
        transform.DOShakeRotation(.25f, .5f);
        transform.DOShakeScale(.25f, .5f);
    }

    private void GoBackToTheHouse(Building building)
    {
        //OnSetTargetPos?.Invoke(RelatedBuilding.transform.position);
        //Delayer.DoActionAfterDelay(this, 2f, () => OnSetTargetPos?.Invoke(building.transform.position));
        //Delayer.DoActionAfterDelay(this, 2f, () => OnStartMoving?.Invoke());
        //Delayer.DoActionAfterDelay(this, 2f, () => OnStopComplaining?.Invoke());

        if (building != RelatedBuilding) return;

        OnSetTargetPos?.Invoke(building.transform.position);
        OnStartMoving?.Invoke();
        OnStopComplaining?.Invoke();
    }

    private void StartIdling()
    {
        Debug.Log("Start Idling");
    }
}
