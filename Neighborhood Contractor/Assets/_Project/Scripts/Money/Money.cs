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

            if (StatManager.CollectedMoney.Count > 1)
            {
                _previousMoney = StatManager.CollectedMoney[StatManager.CollectedMoney.IndexOf(this) - 1];
                _previousMoneyAnimCont = _previousMoney.GetComponent<MoneyAnimationController>();
                
                if (_previousMoneyAnimCont.GetStateInfo())
                    Animator.Play("Money_Moving", 1, _previousMoneyAnimCont.GetCurrentNormalizedTime());
            }
        });

        StackRowNumber = StatManager.CurrentCarry + 1;

        _collected = true;
        StatManager.CollectedMoney.Add(this);
        OnThisMoneyCollected?.Invoke();
    }

    public void Spend(Transform parent)
    {
        transform.parent = parent;

        transform.DOLocalJump(Vector3.zero, _spendMoneyHeight, 1, _animationTime);
        transform.DOLocalRotate(new Vector3(0f, 90f, 0f), _animationTime).OnComplete(() =>
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

    private void ResetMoney()
    {
        _collected = false;
        _spent = false;

        DisableAnimator();
        StackRowNumber = 0;
    }

    private void DisableAnimator() => _animationController.enabled = Animator.enabled = false;

    private void EnableAnimator() => _animationController.enabled = Animator.enabled = true;
}
