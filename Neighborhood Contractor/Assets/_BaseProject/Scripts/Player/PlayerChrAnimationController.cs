using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerChrAnimationController : MonoBehaviour
{
    private Player _player;

    [Header("-- ANIMATION NAME SETUP --")]
    private readonly int runID = Animator.StringToHash("Run");

    private void Awake()
    {
        _player = GetComponent<Player>();
    }

    private void Update()
    {
        _player.animator.SetBool(runID, _player.IsMoving());
    }
}
