using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerMoneyStackHandler : MonoBehaviour
{
    private Player _player;

    [Header("-- SETUP --")]
    [SerializeField] private Transform stackTransform;
    [SerializeField] private float moneyRowOffset = 0.15f;
    [SerializeField] private float moneyColumnOffset = -0.4f;
    [SerializeField] private int stackHeight = 50;

    // Properties
    public Vector3 TargetStackPosition => new Vector3(0f, (StatManager.CurrentCarryRow * moneyRowOffset), (StatManager.CurrentCarryColumn * moneyColumnOffset));
    public Transform StackTransform => stackTransform;
    public int StackHeight => stackHeight;

    private void Awake()
    {
        _player = GetComponent<Player>();
    }
}
