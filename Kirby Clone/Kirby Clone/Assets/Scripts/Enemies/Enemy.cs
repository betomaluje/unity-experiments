using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Enemy")]
public class Enemy : ScriptableObject
{
    [Header("Enemy Information")]
    public new string name;

    [Space]
    [Header("Attack Stats")]
    public LayerMask attackLayerMask;
    public int attack = 10;
    [Range(1, 10)]
    public float forceImpulse = 5;
    [Range(1, 50)]
    public float shootForce = 5;

    [Space]
    [Header("Health Stats")]
    public int health;    
    public int level;

    [Space]
    [Header("Movement Stats")]
    public LayerMask groundLayerMask;
    public float speed = 2.0f;
    public bool shouldJump = true;
    public float jumpVelocity;

    [Space]
    [Header("Misc")]
    public GameObject deathParticles;
}
