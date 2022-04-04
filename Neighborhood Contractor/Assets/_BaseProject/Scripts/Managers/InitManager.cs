using UnityEngine;
using DG.Tweening;

public class InitManager : MonoBehaviour
{
    private void Awake()
    {
        DOTween.Init(true, true, LogBehaviour.Verbose).SetCapacity(19530, 125);
        Application.targetFrameRate = 240;
    }

    private void Start()
    {
        GameEvents.OnChangeScene?.Invoke();
    }
}
