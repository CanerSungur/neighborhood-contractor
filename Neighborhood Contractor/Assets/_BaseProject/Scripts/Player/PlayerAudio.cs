using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerAudio : MonoBehaviour
{
    private Player player;

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    private void Start()
    {
        PlayerEvents.OnJump += Jump;
        PlayerEvents.OnLand += Land;
    }

    private void OnDisable()
    {
        PlayerEvents.OnJump -= Jump;
        PlayerEvents.OnLand -= Land;
    }

    private void Jump() => AudioHandler.PlayAudio(AudioHandler.AudioType.Player_Jump, transform.position);
    private void Land() => AudioHandler.PlayAudio(AudioHandler.AudioType.Player_Land, transform.position);
}
