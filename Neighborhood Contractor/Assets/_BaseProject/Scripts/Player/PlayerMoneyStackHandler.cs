using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerMoneyStackHandler : MonoBehaviour
{
    private Player _player;

    [Header("-- SETUP --")]
    [SerializeField] private Transform stackTransform;
    [SerializeField] private float moneyOffset = 0.15f;

    // Properties
    public Vector3 TargetStackPosition => new Vector3(0f, (StatManager.CurrentCarry * moneyOffset), 0f);
    public Transform StackTransform => stackTransform;

    private void Awake()
    {
        _player = GetComponent<Player>();
    }
}
