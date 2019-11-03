using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    private enum InputDirection
    {
        BOTH, HORIZONTAL, VERTICAL, NONE
    }

    public enum Direction
    {
        UP, DOWN, LEFT, RIGHT, NONE
    }

    [SerializeField] private InputDirection movementDirection;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float turnSpeed = 100f;

    private Direction currentDirection;

    private Rigidbody2D rb;
    private Animator anim;
    private Camera cam;

    private Vector2 movement;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        cam = Camera.main;
        currentDirection = Direction.NONE;
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
        Vector3 realDirection = cam.transform.TransformDirection(movement);
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
            case InputDirection.NONE:
                currentDirection = Direction.NONE;
                break;
            case InputDirection.HORIZONTAL:
                movement.x = Input.GetAxisRaw("Horizontal");
                movement.y = 0;

                if (movement.x < 0)
                {
                    currentDirection = Direction.LEFT;
                }
                else if (movement.x > 0)
                {
                    currentDirection = Direction.RIGHT;
                }
                else
                {
                    currentDirection = Direction.NONE;
                }

                break;
            case InputDirection.VERTICAL:
                movement.x = 0;
                movement.y = Input.GetAxisRaw("Vertical");

                if (movement.y < 0)
                {
                    currentDirection = Direction.DOWN;
                }
                else if (movement.y > 0)
                {
                    currentDirection = Direction.UP;
                }
                else
                {
                    currentDirection = Direction.NONE;
                }

                break;
            case InputDirection.BOTH:
                movement.x = Input.GetAxisRaw("Horizontal");
                movement.y = Input.GetAxisRaw("Vertical");

                if (movement.y < 0)
                {
                    currentDirection = Direction.DOWN;
                }
                else if (movement.y > 0)
                {
                    currentDirection = Direction.UP;
                }
                else if (movement.x < 0)
                {
                    currentDirection = Direction.LEFT;
                }
                else if (movement.x > 0)
                {
                    currentDirection = Direction.RIGHT;
                }
                else
                {
                    currentDirection = Direction.NONE;
                }

                break;
            default:
                break;
        }
    }

    public Direction GetDirection()
    {
        return currentDirection;
    }
}
