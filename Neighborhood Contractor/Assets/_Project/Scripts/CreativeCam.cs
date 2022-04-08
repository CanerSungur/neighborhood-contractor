using UnityEngine;
using Cinemachine;
using System;
using DG.Tweening;

public class CreativeCam : MonoBehaviour
{
    private CinemachineVirtualCamera _cam;
    private CinemachineTransposer _transposer;
    private float zoomOutRate = 2f;

    public static Action OnZoomOut, OnMaxZoom;

    private void Awake()
    {
        _cam = GetComponent<CinemachineVirtualCamera>();
        _transposer = _cam.GetCinemachineComponent<CinemachineTransposer>();
    }

    private void Start()
    {
        OnZoomOut += ZoomOut;
        OnMaxZoom += MaxZoom;
    }

    private void OnDisable()
    {
        OnZoomOut -= ZoomOut;
        OnMaxZoom -= MaxZoom;
    }

    private void MaxZoom()
    {
        DOVirtual.Float(_transposer.m_FollowOffset.y, 38, 0.5f, r => {
            _transposer.m_FollowOffset = new Vector3(_transposer.m_FollowOffset.x, r, _transposer.m_FollowOffset.z);
        });
        DOVirtual.Float(_transposer.m_FollowOffset.z, -38, 0.5f, r => {
            _transposer.m_FollowOffset = new Vector3(_transposer.m_FollowOffset.x, _transposer.m_FollowOffset.y, r);
        });
    }

    private void ZoomOut()
    {
        // decrease z, increase y
        DOVirtual.Float(_transposer.m_FollowOffset.y, _transposer.m_FollowOffset.y + zoomOutRate, 0.5f, r => {
            _transposer.m_FollowOffset = new Vector3(_transposer.m_FollowOffset.x, r, _transposer.m_FollowOffset.z);
        });
        DOVirtual.Float(_transposer.m_FollowOffset.z, _transposer.m_FollowOffset.z - zoomOutRate, 0.5f, r => {
            _transposer.m_FollowOffset = new Vector3(_transposer.m_FollowOffset.x, _transposer.m_FollowOffset.y, r);
        });
    }
}
