using UnityEngine;
using DG.Tweening;

public class Bounce : MonoBehaviour
{
    private void OnEnable()
    {
        BounceOnEnable();
    }

    private void OnDisable()
    {
        transform.DOKill();
    }

    private void BounceOnEnable()
    {
        transform.DORewind();

        //transform.DOShakePosition(.25f, .25f);
        transform.DOShakeRotation(.25f, .5f);
        transform.DOShakeScale(.25f, .5f);
    }
}
