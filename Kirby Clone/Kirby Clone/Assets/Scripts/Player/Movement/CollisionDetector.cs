using UnityEngine;

public class CollisionDetector : MonoBehaviour
{
    [Header("Layers")]
    public LayerMask groundLayer;
	public LayerMask wallLayer;

	[HideInInspector]
    public bool onGround;

	[HideInInspector]
	public bool onWall;

    [HideInInspector]
    public int wallSide;

    [Space]
    [Header("Collision")]
    public float collisionRadius = 0.25f;
    public Vector2 bottomOffset, wallOffset;

	private PlayerMovement playerMovement;
	private int direction = 1;

	private void Awake()
	{
		playerMovement = GetComponent<PlayerMovement>();
	}

	void Update()
    {
		direction = playerMovement.getDirection();

		onGround = Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset, collisionRadius, groundLayer);
        onWall = Physics2D.OverlapCircle((Vector2)transform.position + wallOffset * direction, collisionRadius, wallLayer);

        wallSide = onWall ? -1 : 1;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere((Vector2)transform.position  + bottomOffset, collisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + wallOffset * direction, collisionRadius);
    }
}
