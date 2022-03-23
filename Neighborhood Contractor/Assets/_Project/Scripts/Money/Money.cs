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
    public int StackRowNumber { get; private set; }

    private void Awake()
    {
        _animationController = GetComponent<MoneyAnimationController>();
        DisableAnimator();

        StackRowNumber = 0; // Meaning it's not in the stack.
    }

    private void OnDisable()
    {
        transform.DOKill();
    }

    public void Collect(Vector3 position, Transform parent)
    {
        transform.parent = parent;

        transform.DOLocalJump(position, _collectMoneyHeight, 1, _animationTime);
        transform.DOLocalRotate(new Vector3(0f, 90f, 0f), _animationTime).OnComplete(() =>
        {
            EnableAnimator();
            CollectableEvents.OnCalculateMoveWeight?.Invoke();
            _animationController.SetFirstState();
        });

        // money can start walking animation after DoTween anim finishes.
        //Delayer.DoActionAfterDelay(this, _animationTime, StartWalkingAnimation);
        StackRowNumber = StatManager.CurrentCarry + 1;
        //Delayer.DoActionAfterDelay(this, _animationTime + .01f, () => CollectableEvents.OnCalculateMoveWeight?.Invoke());

        _collected = true;
        StatManager.CollectedMoney.Add(this);
    }

    public void Spend(Transform parent)
    {
        transform.parent = parent;

        transform.DOLocalJump(Vector3.zero, _spendMoneyHeight, 1, _animationTime);
        transform.DOLocalRotate(new Vector3(0f, 90f, 0f), _animationTime);

        // money will stop walking animation immediately.
        StackRowNumber = 0;
        CollectableEvents.OnCalculateMoveWeight?.Invoke();
        DisableAnimator();

        _spent = true;
        StatManager.CollectedMoney.Remove(this);
        //Debug.Log(StatManager.CollectedMoney.Count);
    }

    private void DisableAnimator() => _animationController.enabled = Animator.enabled = false;

    private void EnableAnimator() => _animationController.enabled = Animator.enabled = true;
}
