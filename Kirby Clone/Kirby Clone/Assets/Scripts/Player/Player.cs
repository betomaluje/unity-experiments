using UnityEngine;

[CreateAssetMenu(fileName = "New Player", menuName = "Player")]
public class Player : ScriptableObject
{
    [Header("Player Information")]
    public new string name;

    [Space]
    [Header("Health Stats")]
    public int health;
    public int lives;

    [Space]
    [Header("Movement Stats")]
    public float speed = 2.0f;
    public float jumpVelocity;

    public float fallMultiplier = 3f;
    public float lowJumpMultiplier = 8f;
}
