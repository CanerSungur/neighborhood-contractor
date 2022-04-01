using UnityEngine;
using System;
using ZestGames.Utility;
using DG.Tweening;

public class Neighbor : MonoBehaviour
{
    [Header("-- REFERENCES --")]
    private NeighborMovement _movementHandler;
    private NeighborCollision _collisionHandler;
    private NeighborAnimationController _animationHandler;

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

    public Action OnStartMoving;
    public Action<Vector3> OnSpawned;

    private void Init()
    {
        Animator = GetComponent<Animator>();
        Collider = GetComponent<Collider>();
        Rigidbody = GetComponent<Rigidbody>();

        _movementHandler = GetComponent<NeighborMovement>();
        _collisionHandler = GetComponent<NeighborCollision>();
        
        _animationHandler = GetComponent<NeighborAnimationController>();
        _animationHandler.Init();

        CanMove = false;
        Delayer.DoActionAfterDelay(this, waitAfterSpawn, () => CanMove = true);
        Delayer.DoActionAfterDelay(this, waitAfterSpawn, () => OnStartMoving?.Invoke());
        OnSpawned += SetTargetPosition;
    }

    private void OnEnable()
    {
        Init();
        Bounce();
    }

    private void OnDisable()
    {
        OnSpawned -= SetTargetPosition;
    }

    private void SetTargetPosition(Vector3 targetPos) => _targetPosition = targetPos;

    private void Bounce()
    {
        transform.DORewind();

        //transform.DOShakePosition(.25f, .25f);
        transform.DOShakeRotation(.25f, .5f);
        transform.DOShakeScale(.25f, .5f);
    }
}
