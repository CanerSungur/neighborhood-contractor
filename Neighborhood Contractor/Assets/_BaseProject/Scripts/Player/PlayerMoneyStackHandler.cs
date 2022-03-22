using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerMoneyStackHandler : MonoBehaviour
{
    public static int CurrentCarryCount, CarryCapacity;

    [Header("-- SETUP --")]
    [SerializeField] private Transform stackTransform;
    [SerializeField] private float moneyOffset = 0.15f;
    [SerializeField] private int carryCapacity = 5;

    private Player _player;

    public Vector3 TargetStackPosition => new Vector3(0f, (CurrentCarryCount * moneyOffset), 0f);
    public Transform StackTransform => stackTransform;

    private void Awake()
    {
        CurrentCarryCount = 0;
        CarryCapacity = carryCapacity;
        _player = GetComponent<Player>();
    }

    private void Start()
    {
        _player.OnCollectedMoney += IncreaseStackCount;
    }

    private void OnDisable()
    {
        _player.OnCollectedMoney -= IncreaseStackCount;
    }

    private void IncreaseStackCount() => CurrentCarryCount++;
}
