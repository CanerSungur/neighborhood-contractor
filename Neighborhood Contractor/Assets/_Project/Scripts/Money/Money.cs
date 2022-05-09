using UnityEngine;
using DG.Tweening;
using System;
using ZestGames.Utility;

public class Money : MonoBehaviour
{
    private bool _collected = false;
    private bool _spent = false;
    private Money _previousMoney = null;
    private MoneyAnimationController _previousMoneyAnimCont = null;

    [Header("-- ANIMATION SETUP --")]
    private float _animationTime = 0.5f;
    private float _collectMoneyHeight = 1.5f;
    private float _spendMoneyHeight = 0.5f;

    [Header("-- SCRIPT REFERENCES --")]
    private MoneyAnimationController _animationController;

    private Animator animator;
    public Animator Animator => animator == null ? animator = GetComponent<Animator>() : animator;

    public bool Collected => _collected;
    public bool CanBeCollected => !_collected && StatManager.CurrentCarry < StatManager.CarryCapacity;
    public bool CanBeSpent => !_spent && _collected;
    public int StackRowNumber { get; private set; }

    public event Action OnThisMoneyCollected;

    private void Awake()
    {
        if (!_animationController)
            _animationController = GetComponent<MoneyAnimationController>();
        DisableAnimator();

        StackRowNumber = 0; // Meaning it's not in the stack.
    }

    private void OnDisable()
    {
        transform.DOKill();
        ResetMoney();
    }

    public void Collect(Vector3 position, Transform parent)
    {
        transform.DOKill();
        transform.parent = parent;

        transform.DOLocalJump(position, _collectMoneyHeight, 1, _animationTime);
        transform.DOLocalRotate(new Vector3(0f, 90f, 0f), _animationTime);
        Delayer.DoActionAfterDelay(this, _animationTime, () => {
            EnableAnimator();
            CollectableEvents.OnCalculateMoveWeight?.Invoke();
            _animationController.SetFirstState();

            if (_previousMoney != null && _previousMoney.TryGetComponent(out _previousMoneyAnimCont))
            {
                if (_previousMoneyAnimCont.GetStateInfo()) // Meaning if it's walking.
                    Animator.Play("Money_Moving", 1, _previousMoneyAnimCont.GetCurrentNormalizedTime());
            }
        });

        //StackRowNumber = StatManager.CurrentCarry + 1;
        StackRowNumber = StatManager.CurrentCarryRow + 1;

        _collected = true;
        StatManager.CollectedMoney.Add(this);
        OnThisMoneyCollected?.Invoke();

        _previousMoney = GetPreviousMoney();
    }

    public void Spend(Transform parent)
    {
        transform.DOKill();
        transform.parent = parent;

        transform.DOLocalJump(Vector3.zero, _spendMoneyHeight, 1, _animationTime);
        transform.DOLocalRotate(new Vector3(0f, 90f, 0f), _animationTime);
        Delayer.DoActionAfterDelay(this, _animationTime, () =>
        {
            ResetMoney();
            gameObject.SetActive(false);
        });

        StackRowNumber = 0;
        CollectableEvents.OnCalculateMoveWeight?.Invoke();
        DisableAnimator();

        _spent = true;
        StatManager.CollectedMoney.Remove(this);
    }

    public void SpendForRepair(Transform parent)
    {
        transform.DOKill();
        transform.parent = parent;
        //transform.DOLocalJump(Vector3.zero, )
        transform.DOJump(parent.position, 5f, 1, _animationTime);
        //transform.DOLocalJump(Vector3.zero, 2f, 1, _animationTime);
        transform.DOLocalRotate(new Vector3(0f, 90f, 0f), _animationTime);
        Delayer.DoActionAfterDelay(this, _animationTime, ResetMoney);

        StackRowNumber = 0;
        CollectableEvents.OnCalculateMoveWeight?.Invoke();
        DisableAnimator();

        _spent = true;
        StatManager.CollectedMoney.Remove(this);
    }

    private void ResetMoney()
    {
        transform.DOKill();
        _collected = false;
        _spent = false;

        DisableAnimator();
        StackRowNumber = 0;
        gameObject.SetActive(false);
    }

    private Money GetPreviousMoney()
    {
        return StatManager.CollectedMoney.IndexOf(this) > 0 ? StatManager.CollectedMoney[StatManager.CollectedMoney.IndexOf(this) - 1] : null;
    }

    private void DisableAnimator() => _animationController.enabled = Animator.enabled = false;
    private void EnableAnimator() => _animationController.enabled = Animator.enabled = true;
    public void SetMoneyAsCollected(int index)
    {
        StackRowNumber = index + 1;
        EnableAnimator();

        CollectableEvents.OnCalculateMoveWeight?.Invoke();
        _collected = true;
        StatManager.CollectedMoney.Add(this);
        
    }
}
