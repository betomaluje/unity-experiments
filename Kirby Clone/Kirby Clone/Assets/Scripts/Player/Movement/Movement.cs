using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public class Movement : MonoBehaviour, MainInputActions.IPlayerActions
{     
    [Space]
    [Header("Booleans")]
    private bool canMove = true;
    private bool wallGrab;
    private bool wallJumped;
    private bool wallSlide;
    private bool isDashing;
    private bool grounded = false;

    [Space]
    private bool groundTouch;
    private bool hasDashed;
    private bool facingRight = true;

    [Space]
    private int numofJumps = 0;
    public int maxJumps = 2;

    [Header("Movement Events")]
    public GameEvent dashEvent;

    // Private variables

    private Rigidbody2D rb;
    private Player player;
    private Collision coll;
    private Animator animator;
    private SpriteRenderer sr;
    private Camera cam;
    private RippleEffect rippleObject;

    private float moveX;
    private float moveY;
    private bool attackInputPressed;
    private bool jumpInputPressed;

    private int side = 1;

    private void Start()
    {
        cam = Camera.main;
        coll = GetComponent<Collision>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        player = GetComponent<PlayerStats>().GetPlayer();
        rippleObject = FindObjectOfType<RippleEffect>();
    }

    private void FixedUpdate()
    {
        Vector2 dir = new Vector2(moveX, moveY);
        Walk(dir);

        if (wallSlide)
        {
            WallSlide();
            attackInputPressed = false;
        }

        if (wallJumped)
        {
            WallJump();
            attackInputPressed = false;
        }      
    }

    private void Update()
    {
        grounded = IsGrounded();

        if (animator != null)
            animator.SetBool("isJumping", !grounded);

        if (coll.onWall && attackInputPressed && canMove)
        {
            if (side != coll.wallSide)
                Flip(side*-1);
            
            wallGrab = true;
            wallSlide = false;
        }

        if (attackInputPressed || !coll.onWall || !canMove)
        {
            wallGrab = false;
            wallSlide = false;
        }

        if (grounded && !isDashing)
        {
            wallJumped = false;
            GetComponent<BetterJumping>().enabled = true;
            jumpInputPressed = false;
        }
       
        if (wallGrab && !isDashing)
        {
            rb.gravityScale = 0;
            if (moveX > .2f || moveX < -.2f)
            rb.velocity = new Vector2(rb.velocity.x, 0);

            float speedModifier = moveY > 0 ? .5f : 1;

            rb.velocity = new Vector2(rb.velocity.x, moveY * (player.speed * speedModifier));
        }
        else
        {
            rb.gravityScale = 3;
        }

        if (coll.onWall && !grounded)
        {
            if (moveX != 0 && !wallGrab)
            {
                wallSlide = true;                
            }

            // maybe we should use the attack input instead. Need to test how does it feel
            if (jumpInputPressed)
            {
                wallJumped = true;
            }
        }

        if (!coll.onWall || grounded)
        {
            wallSlide = false;
        }                    

        if (attackInputPressed && !hasDashed && (moveX != 0 || moveY != 0))
        {
            dashEvent.Raise();
            attackInputPressed = false;
        }

        if (grounded && !groundTouch)
        {
            GroundTouch();
            groundTouch = true;
        }

        if (!grounded && groundTouch)
        {
            groundTouch = false;
        }          

        if (moveX > 0 && !facingRight)
        {
            side = 1;
            Flip(side);
        }
        if (moveX < 0 && facingRight)
        {
            side = -1;
            Flip(side);
        }    
    }

    bool IsGrounded()
    {
        return coll.onGround;
    }

    void GroundTouch()
    {
        hasDashed = false;
        isDashing = false;

        side = !facingRight ? -1 : 1;
    }

    public void Dash()
    {
        cam.transform.DOComplete();
        cam.transform.DOShakePosition(.2f, .5f, 14, 90, false, true);
        rippleObject.Emit(cam.WorldToViewportPoint(transform.position));

        hasDashed = true;       

        rb.velocity = Vector2.zero;
        Vector2 dir = new Vector2(moveX, moveY);
        SoundManager.instance.Play("Dash");
        rb.velocity += dir.normalized * player.dashSpeed;
        StartCoroutine(DashWait());
    }

    IEnumerator DashWait()
    {
        //FindObjectOfType<GhostTrail>().ShowGhost();
        StartCoroutine(GroundDash());
        DOVirtual.Float(14, 0, .8f, RigidbodyDrag);
        
        rb.gravityScale = 0;
        GetComponent<BetterJumping>().enabled = false;
        wallJumped = true;
        isDashing = true;

        yield return new WaitForSeconds(.3f);
        
        rb.gravityScale = 3;
        GetComponent<BetterJumping>().enabled = true;
        wallJumped = false;
        isDashing = false;
    }

    IEnumerator GroundDash()
    {
        yield return new WaitForSeconds(.15f);
        if (grounded)
            hasDashed = false;
    }

    private void WallJump()
    {
        SoundManager.instance.Play("Jump");
        if ((side == 1 && coll.onRightWall) || side == -1 && !coll.onRightWall)
        {
            side *= -1;
            Flip(side);
        }

        StopCoroutine(DisableMovement(0));
        StartCoroutine(DisableMovement(.1f));

        Vector2 wallDir = coll.onRightWall ? Vector2.left : Vector2.right;    

        Jump((Vector2.up / 1.5f + wallDir / 1.5f));

        wallJumped = false;
    }

    private void WallSlide()
    {
        if (coll.wallSide != side)
        {
            Flip(side * -1);

            side = !facingRight ? -1 : 1;
        }

        if (!canMove)
            return;

        bool pushingWall = false;
        if ((rb.velocity.x > 0 && coll.onRightWall) || (rb.velocity.x < 0 && coll.onLeftWall))
        {
            pushingWall = true;
        }

        float push = pushingWall ? 0 : rb.velocity.x;

        rb.velocity = new Vector2(push, -player.slideSpeed);
    }

    private void Walk(Vector2 dir)
    {
        if (!canMove)
            return;

        if (wallGrab)
            return;

        if (!wallJumped)
        {
            rb.velocity = new Vector2(dir.x * player.speed, rb.velocity.y);
        }
        else
        {
            rb.velocity = Vector2.Lerp(rb.velocity, (new Vector2(dir.x * player.speed, rb.velocity.y)), player.wallJumpLerp * Time.deltaTime);
        }
    }

	public void OnMovement(InputAction.CallbackContext context)
	{
		float movement = (float)context.ReadValueAsObject();
		moveX = movement;

		Debug.Log("Player wants to move: " + moveX);
	}

	public void OnJump(InputAction.CallbackContext context)
	{
		float jump = (float)context.ReadValueAsObject();
		
        if (jump == 1)
		{
			attackInputPressed = true;
			Debug.Log("jump");
		}
	}

	public void Move(float move)
    {
        moveX = move;
    }

    public void AttackPressed()
    {
        attackInputPressed = true;
    }

    public void Jump(Vector2 dir)
    {
        //slideParticle.transform.parent.localScale = new Vector3(ParticleSide(), 1, 1);
        //ParticleSystem particle = wall ? wallJumpParticle : jumpParticle;
      
        if (grounded || wallJumped)
            numofJumps = 0;

        if (grounded || numofJumps < maxJumps)
        {
            SoundManager.instance.Play("Jump");

            jumpInputPressed = true;

            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.velocity += dir * player.jumpVelocity;

            numofJumps++;
            grounded = false;
        }    

        //particle.Play();
    }

    IEnumerator DisableMovement(float time)
    {
        canMove = false;
        yield return new WaitForSeconds(time);
        canMove = true;
    }

    void RigidbodyDrag(float x)
    {
        rb.drag = x;
    }

    int ParticleSide()
    {
        int particleSide = coll.onRightWall ? 1 : -1;
        return particleSide;
    }

    public void Flip(int side)
    {
        if (wallGrab || wallSlide)
        {
            if (side == -1 && sr.flipX)
                return;

            if (side == 1 && !sr.flipX)
            {
                return;
            }
        }

        facingRight = !facingRight;

        transform.Rotate(0f, 180f, 0f);
    }

    public bool IsFacingRight()
    {
        return facingRight;
    }
}
