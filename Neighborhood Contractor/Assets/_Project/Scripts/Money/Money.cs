using UnityEngine;
using DG.Tweening;
using ZestGames.Utility;

public class Money : MonoBehaviour
{
    private bool _collected = false;
    private bool _spent = false;

    [Header("-- ANIMATION SETUP --")]
    private float _animationTime = 0.5f;
    private float _collectMoneyHeight = 1.5f;
    private float _spendMoneyHeight = 0.5f;

    [Header("-- SCRIPT REFERENCES --")]
    private MoneyAnimationController _animationController;

    private Animator animator;
    public Animator Animator => animator == null ? animator = GetComponent<Animator>() : animator;

    public bool CanBeCollected => !_collected && StatManager.CurrentCarry < StatManager.CarryCapacity;
    public bool CanBeSpent => !_spent && _collected;

    private void Awake()
    {
        _animationController = GetComponent<MoneyAnimationController>();
        StopWalkingAnimation();
    }

    private void OnDisable()
    {
        transform.DOKill();
    }

    public void Collect(Vector3 position, Transform parent)
    {
        transform.parent = parent;

        transform.DOLocalJump(position, _collectMoneyHeight, 1, _animationTime);
        transform.DOLocalRotate(new Vector3(0f, 90f, 0f), _animationTime);

        // money can start walking animation after DoTween anim finishes.
        Delayer.DoActionAfterDelay(this, _animationTime, StartWalkingAnimation);

        _collected = true;
        StatManager.CollectedMoney.Add(this);
        //Debug.Log(StatManager.CollectedMoney.Count);
    }

    public void Spend(Transform parent)
    {
        transform.parent = parent;

        transform.DOLocalJump(Vector3.zero, _spendMoneyHeight, 1, _animationTime);
        transform.DOLocalRotate(new Vector3(0f, 90f, 0f), _animationTime);

        // money will stop walking animation immediately.
        StopWalkingAnimation();

        _spent = true;
        StatManager.CollectedMoney.Remove(this);
        //Debug.Log(StatManager.CollectedMoney.Count);
    }

    private void StopWalkingAnimation() => _animationController.enabled = Animator.enabled = false;

    private void StartWalkingAnimation() => _animationController.enabled = Animator.enabled = true;
}
