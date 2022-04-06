using UnityEngine;
using DG.Tweening;
using System;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    private GameManager gameManager;
    public GameManager GameManager { get { return gameManager == null ? gameManager = GetComponent<GameManager>() : gameManager; } }

    private Player player;
    public Player Player => player == null ? player = FindObjectOfType<Player>() : player;

    [Header("-- CAMERA SETUP --")]
    [SerializeField] private CinemachineVirtualCamera gameStartCM;
    [SerializeField] private CinemachineVirtualCamera gameplayCM;
    private CinemachineTransposer _gameplayCMTransposer;
    private float _gameplayCMXLeftAxis = 2.5f;
    private float _gameplayCMXRightAxis = -2.5f;

    [Header("-- SHAKE SETUP --")]
    private CinemachineBasicMultiChannelPerlin gameplayCMBasicPerlin;
    private bool shakeStarted = false;
    private float shakeDuration = 1f;
    private float shakeTimer;

    public static event Action OnShakeCam;
    public static Action OnChangeXAxis, OnCamIsOnLeft, OnCamIsOnRight;

    private bool _isOnLeft = true;

    private void Awake()
    {
        _gameplayCMTransposer = gameplayCM.GetCinemachineComponent<CinemachineTransposer>();
        gameplayCMBasicPerlin = gameplayCM.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        gameplayCMBasicPerlin.m_AmplitudeGain = 0f;
        shakeTimer = shakeDuration;

        gameStartCM.Priority = 2;
        gameplayCM.Priority = 1;
    }

    private void Start()
    {
        _gameplayCMTransposer.m_FollowOffset = new Vector3(2.5f, 10f, -10f);

        GameEvents.OnGameStart += () => gameplayCM.Priority = 3;
        OnShakeCam += () => shakeStarted = true;

        OnCamIsOnLeft += CamIsOnLeft;
        OnCamIsOnRight += CamIsOnRight;
    }

    private void OnDisable()
    {
        GameEvents.OnGameStart -= () => gameplayCM.Priority = 3;
        OnShakeCam -= () => shakeStarted = true;

        transform.DOKill();

        OnShakeCam = null;

        OnCamIsOnLeft -= CamIsOnLeft;
        OnCamIsOnRight -= CamIsOnRight;
    }

    private void CamIsOnRight()
    {
        if (!_isOnLeft) return;
        transform.DOKill();
        DOVirtual.Vector3(_gameplayCMTransposer.m_FollowOffset, new Vector3(_gameplayCMXRightAxis, 10f, -10f), 2f, r => {
            _gameplayCMTransposer.m_FollowOffset = r;
        });
    }

    private void CamIsOnLeft()
    {
        if (_isOnLeft) return;
        transform.DOKill();
        DOVirtual.Vector3(_gameplayCMTransposer.m_FollowOffset, new Vector3(_gameplayCMXLeftAxis, 10f, -10f), 2f, r => {
            _gameplayCMTransposer.m_FollowOffset = r;
        });
    }

    private void Update()
    {
        UpdateCamFollowOffset();

        ShakeCamForAWhile();
    }

    private void UpdateCamFollowOffset()
    {
        if (Player.transform.position.x > 0 && _isOnLeft)
        {
            OnCamIsOnRight?.Invoke();
            _isOnLeft = false;
        }
        else if (Player.transform.position.x < 0 && !_isOnLeft)
        {
            OnCamIsOnLeft?.Invoke();
            _isOnLeft = true;
        }
    }

    private void ShakeCamForAWhile()
    {
        if (shakeStarted)
        {
            gameplayCMBasicPerlin.m_AmplitudeGain = 1f;

            shakeTimer -= Time.deltaTime;
            if (shakeTimer <= 0f)
            {
                shakeStarted = false;
                shakeTimer = shakeDuration;

                gameplayCMBasicPerlin.m_AmplitudeGain = 0f;
            }
        }
    }

    public static void ShakeCamTrigger() => OnShakeCam?.Invoke();
}
