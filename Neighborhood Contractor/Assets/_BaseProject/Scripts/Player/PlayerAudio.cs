using System;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerAudio : MonoBehaviour
{
    private Player _player;

    private float _collectTargetPitch = 10f;
    private float _spendTargetPitch = 1f;
    private float _pitchIncrement = 0.1f;
    private float _currentCollectPitch, _currentSpendPitch;

    private bool _collectingMoney, _spendingMoney;
    private float _cooldown = 2f;
    private float _collectTimer, _spendTimer;

    private void Start()
    {
        _player = GetComponent<Player>();
        AudioEvents.OnPlayCollectMoney += HandleCollectMoney;
        AudioEvents.OnPlaySpendMoney += HandleSpendMoney;

        _currentCollectPitch = 1f;
        _currentSpendPitch = 3f;
        _collectingMoney = _spendingMoney = false;
        _collectTimer = _cooldown;
        _spendTimer = _cooldown;
    }

    private void OnDisable()
    {
        AudioEvents.OnPlayCollectMoney -= HandleCollectMoney;
        AudioEvents.OnPlaySpendMoney -= HandleSpendMoney;
    }

    private void Update()
    {
        if (!_player) return;

        HandleCollectMoneyCooldown();

        HandleSpendMoneyCooldown();
    }

    private void HandleCollectMoney()
    {
        AudioHandler.PlayAudio(AudioHandler.AudioType.SpendMoney, 0.8f, _currentCollectPitch);
        _collectTimer = _cooldown;
        _collectingMoney = true;
    }

    private void HandleSpendMoney()
    {
        AudioHandler.PlayAudio(AudioHandler.AudioType.Button_Click, 0.3f, _currentSpendPitch);
        _spendTimer = _cooldown;
        _spendingMoney = true;
    }

    private void HandleCollectMoneyCooldown()
    {
        if (_collectingMoney)
        {
            _collectTimer -= Time.deltaTime;
            if (_collectTimer < 0f)
            {
                _collectTimer = _cooldown;
                _collectingMoney = false;
            }

            _currentCollectPitch = Mathf.Lerp(_currentCollectPitch, _collectTargetPitch, _pitchIncrement * Time.deltaTime);
        }
        else
            _currentCollectPitch = 1f;
    }

    private void HandleSpendMoneyCooldown()
    {
        if (_spendingMoney)
        {
            _spendTimer -= Time.deltaTime;
            if (_spendTimer < 0f)
            {
                _spendTimer = _cooldown;
                _spendingMoney = false;
            }

            _currentSpendPitch = Mathf.Lerp(_currentSpendPitch, _spendTargetPitch, _pitchIncrement * Time.deltaTime);
        }
        else
            _currentSpendPitch = 3f;
    }
}
