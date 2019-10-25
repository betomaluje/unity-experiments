using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    private enum Direction
    {
        BOTH, HORIZONTAL, VERTICAL, NONE
    }

    [SerializeField] private Direction movementDirection;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float turnSpeed = 100f;

    private Rigidbody2D rb;
    private Animator anim;

    private Vector2 movement;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        CheckInput();       

        anim.SetBool("isWalking", movement.x != 0 || movement.y != 0);
    }

    private void FixedUpdate()
    {
        // movement
        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);

        // rotation
        Vector3 realDirection = Camera.main.transform.TransformDirection(movement);
        // this line checks whether the player is making inputs.
        if (realDirection.magnitude > 0.1f)
        {
            Quaternion newRotation = Quaternion.LookRotation(Vector3.forward, realDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.fixedDeltaTime * turnSpeed);
        }
    }

    private void CheckInput()
    {
        switch (movementDirection)
        {
            case Direction.NONE:
                break;
            case Direction.HORIZONTAL:
                movement.x = Input.GetAxisRaw("Horizontal");
                movement.y = 0;
                break;
            case Direction.VERTICAL:
                movement.x = 0;
                movement.y = Input.GetAxisRaw("Vertical");
                break;
            case Direction.BOTH:
                movement.x = Input.GetAxisRaw("Horizontal");
                movement.y = Input.GetAxisRaw("Vertical");
                break;
            default:
                break;
        }
    }

}
