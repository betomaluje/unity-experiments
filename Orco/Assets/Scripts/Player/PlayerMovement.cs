using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{  
    public enum Direction
    {
        UP, DOWN, LEFT, RIGHT, NONE
    }
    
    [SerializeField] private float speed = 5f;
    [SerializeField] private float turnSpeed = 100f;

    private Direction currentDirection;

    private Rigidbody2D rb;
    private Animator anim;
    private Camera cam;

    // Input Actions
    private Vector2 movement;

    #region Lifecycle methods

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        cam = Camera.main;
        currentDirection = Direction.NONE;
    }

    void Update()
    {
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
    #endregion

    public void OnMovement(InputValue value)
    {
        movement = value.Get<Vector2>();

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
    }      

    public Direction GetDirection()
    {
        return currentDirection;
    }   
}
