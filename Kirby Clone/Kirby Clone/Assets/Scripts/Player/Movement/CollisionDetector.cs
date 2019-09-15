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
    public Vector2 bottomOffset, wallOffset;

	void Update()
    {
		onGround = Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset, collisionRadius, groundLayer);        
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere((Vector2)transform.position  + bottomOffset, collisionRadius);
    }
}
