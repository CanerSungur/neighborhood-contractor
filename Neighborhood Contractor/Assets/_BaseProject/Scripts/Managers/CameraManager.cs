using UnityEngine;
using DG.Tweening;
using System;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    private GameManager gameManager;
    public GameManager GameManager { get { return gameManager == null ? gameManager = GetComponent<GameManager>() : gameManager; } }

    [Header("-- CAMERA SETUP --")]
    [SerializeField] private CinemachineVirtualCamera gameStartCM;
    [SerializeField] private CinemachineVirtualCamera gameplayCM;

    [Header("-- SHAKE SETUP --")]
    private CinemachineBasicMultiChannelPerlin gameplayCMBasicPerlin;
    private bool shakeStarted = false;
    private float shakeDuration = 1f;
    private float shakeTimer;

    public static event Action OnShakeCam;

    private void Awake()
    {
        gameplayCMBasicPerlin = gameplayCM.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        gameplayCMBasicPerlin.m_AmplitudeGain = 0f;
        shakeTimer = shakeDuration;

        gameStartCM.Priority = 2;
        gameplayCM.Priority = 1;
    }

    private void Start()
    {
        GameEvents.OnGameStart += () => gameplayCM.Priority = 3;
        OnShakeCam += () => shakeStarted = true;
    }

    private void OnDisable()
    {
        GameEvents.OnGameStart -= () => gameplayCM.Priority = 3;
        OnShakeCam -= () => shakeStarted = true;

        transform.DOKill();

        OnShakeCam = null;
    }

    private void Update()
    {
        ShakeCamForAWhile();
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
