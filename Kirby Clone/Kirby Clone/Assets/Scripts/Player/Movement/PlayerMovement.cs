using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController2D controller;

    #region Public Variables
    [Header("Stats")]
    public int maxJumps = 2;   
    #endregion

    #region Private Variables
    private bool canMove = true;
    private bool grounded = false;
    private bool isJumping = false;
    private bool crouch = false;

    private bool groundTouch;

    private int numofJumps = 0;

    private Rigidbody2D rb;
    private Player player;
    private CollisionDetector coll;
    private Animator animator;
    private SpriteRenderer sr;
    private Camera cam;
    private RippleEffect rippleObject;

    private float moveX;

    #endregion

    #region Lifecycle methods    

    private void Start()
    {
        cam = Camera.main;
        coll = GetComponent<CollisionDetector>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        player = GetComponent<PlayerStats>().GetPlayer();
        rippleObject = FindObjectOfType<RippleEffect>();

        controller = GetComponent<CharacterController2D>();
        controller.setPlayerStats(player);
    }

    private void Update()
    {
        grounded = IsGrounded();

        if (animator != null)
        {
            animator.SetBool("isJumping", !grounded);
        }
    }

    private void FixedUpdate()
    {
        controller.Move(moveX * player.speed * Time.fixedDeltaTime, crouch, isJumping);
        isJumping = false;
    }
    #endregion

    #region Event Listener

    public void OnHorizontalMove(float horizontalMove)
    {
        moveX = horizontalMove;
        if (animator != null)
        {
            animator.SetBool("isRunning", moveX != 0);
        }
    }

    public void OnVerticalMove(float verticalMove) {

    }

    public void OnJump()
    {
        isJumping = true;
    }

    #endregion

    bool IsGrounded()
    {
        return coll.onGround;
    }

    IEnumerator DisableMovement(float time)
    {
        canMove = false;
        yield return new WaitForSeconds(time);
        canMove = true;
    }

    public bool isFacingRight()
    {
        return controller.isFacingRight();
    }

    public int getDirection()
    {
        return isFacingRight() ? 1 : -1;
    }
}
