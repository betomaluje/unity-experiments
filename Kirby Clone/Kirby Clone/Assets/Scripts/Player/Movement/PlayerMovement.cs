using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public class PlayerMovement : MonoBehaviour, MainInputActions.IPlayerActions
{
    private MainInputActions mainInputActions;

    private Vector2 movement;

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
    #endregion

    #region Lifecycle methods
    private void Awake()
    {
        mainInputActions = new MainInputActions();      
        mainInputActions.Player.SetCallbacks(this);        
    }

    private void OnEnable()
    {
        mainInputActions.Enable();
    }

    private void OnDisable()
    {
        mainInputActions.Disable();
    }

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
            animator.SetBool("isJumping", !grounded);        
    }

    private void FixedUpdate()
    {
        // Move our character
        controller.Move(movement.x * player.speed * Time.fixedDeltaTime, crouch, isJumping);
        isJumping = false;
    }
    #endregion

    #region Input callbacks

    public void OnMovement(InputAction.CallbackContext context)
    {
        movement = context.ReadValue<Vector2>();        
    }    

    public void OnJump(InputAction.CallbackContext context)
    {
        float jump = (float)context.ReadValueAsObject();

        isJumping = jump == 1;
    }

    public void OnAbsorb(InputAction.CallbackContext context)
    {
        float absorb = (float)context.ReadValueAsObject();

        if (absorb == 1)
        {
            Debug.Log("absorb");
        }
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
}
