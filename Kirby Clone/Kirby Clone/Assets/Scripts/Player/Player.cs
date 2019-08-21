using UnityEngine;

[CreateAssetMenu(fileName = "New Player", menuName = "Player")]
public class Player : ScriptableObject
{
    [Header("Player Information")]
    public new string name;

    [Header("Input Information")]
    public string horizontalInput;
    public string verticalInput;
    public string jumpInput;
    public string fireInput;
    public string grabInput;

    [Space]
    [Header("Health Stats")]
    public int health;
    public int lives;

    [Space]
    [Header("Movement Stats")]
    public float speed = 2.0f;
    public float jumpVelocity;
    public float dashSpeed;    
    public float slideSpeed = 1;
    public float wallJumpLerp = 5;

    public float fallMultiplier = 3f;
    public float lowJumpMultiplier = 8f;
}
