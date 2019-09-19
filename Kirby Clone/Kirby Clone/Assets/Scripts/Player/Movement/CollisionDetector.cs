using UnityEngine;

public class CollisionDetector : MonoBehaviour
{
    [Header("Layers")]
    public LayerMask groundLayer;

	//[HideInInspector]
    public bool onGround;

    [Space]
    [Header("Collision")]
    public float collisionRadius = 0.25f;
    public Vector2 wallOffset;

    public Transform groundDetection;

    void Update()
    {
        onGround = Physics2D.Raycast(groundDetection.position, Vector2.down, collisionRadius, groundLayer);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawLine(groundDetection.position, groundDetection.position + groundDetection.transform.up * -1 * collisionRadius);
    }
}
