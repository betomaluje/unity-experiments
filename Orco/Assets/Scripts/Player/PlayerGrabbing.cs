using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerGrabbing : TargetDetection
{
    [Space]
    [Header("Settings")]
    // the players item grab position
    [SerializeField] private Transform itemGameObjectPosition;
    [SerializeField] private float throwForce = 250f;

    [SerializeField] private float timeForLongPress = 0.5f;

    [SerializeField] private float timeForHumanReset = 2f;

    private Animator anim;
    private Rigidbody2D rb;

    private bool objectGrabbed = false;
    private GameObject targetObject;    
    private float timePressing = 0f;    

    // animations
    private bool isTorning = false;
    private bool buttonPressed = false;

    private PlayerMovement playerMovement;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    public override void Update()
    {
        base.Update();

        switch (playerMovement.GetDirection())
        {
            case PlayerMovement.Direction.LEFT:
                SetDirection(new Vector2(-0.5f, 0f));
                break;
            case PlayerMovement.Direction.RIGHT:
                SetDirection(new Vector2(0.5f, 0f));
                break;
            case PlayerMovement.Direction.UP:
                SetDirection(new Vector2(0f, 0.6f));
                break;
            case PlayerMovement.Direction.DOWN:
                SetDirection(new Vector2(0f, -0.6f));
                break;
        }

        Animate();
        CheckInput();             
    }

    private void LateUpdate()
    {
        if (objectGrabbed && targetObject != null)
        {
            targetObject.transform.parent = itemGameObjectPosition;
            targetObject.transform.localPosition = Vector3.zero;
        }
    }

    public void OnActionX(InputValue value)
    {
        buttonPressed = value.Get<float>() == 1;
    }

    private void CheckInput()
    {        
        if (buttonPressed)
        {
            timePressing += Time.deltaTime;

            if (!objectGrabbed && onTargetDetected != null && onTargetDetected.gameObject != gameObject)
            {                
                SoundManager.instance.PlayRandom("Orc Growl");
                targetObject = onTargetDetected.gameObject;

                objectGrabbed = true;
                ChangeHumanGrabbed(true);
                ChangePlayerGrabbed(true);
            }            
        } else
        {
            if (objectGrabbed)
            {
                objectGrabbed = false;

                if (timePressing >= timeForLongPress && !isTorning)
                {
                    isTorning = true;
                    DoTorn();
                }
                else if (!isTorning)
                {
                    DoThrow(itemGameObjectPosition.position - transform.position);
                }                
            }            

            timePressing = 0;
        }
    }

    private void Animate()
    {
        anim.SetBool("isAttacking", buttonPressed);
        anim.SetBool("isTorning", isTorning);
    }  

    private void DoTorn()
    {
        if (targetObject == null)
        {
            return;
        }

        SoundManager.instance.PlayRandom("Orc Torn");

        HumanDeath humanDeath = targetObject.GetComponent<HumanDeath>();
        if (humanDeath != null)
        {
            humanDeath.TornApart(gameObject);
        }

        ReleaseTarget();

        isTorning = false;
    }

    public void DoThrow(Vector3 dir)
    {
        if (targetObject == null)
        {
            return;
        }

        SoundManager.instance.PlayRandom("Orc Growl");
        
        ChangeHumanGrabbed(false);
        ChangePlayerGrabbed(false);

        ThrowableAction throwableAction = targetObject.GetComponent<ThrowableAction>();
        if (throwableAction != null)
        {
            throwableAction.Throw(dir, throwForce);
        }

        ReleaseTarget();
    }  

    private void ReleaseTarget()
    {
        objectGrabbed = false;

        foreach (Transform child in itemGameObjectPosition)
        {
            child.parent = null;
        }
        
        targetObject = null;
    }

    private void ChangeHumanGrabbed(bool grabbed)
    {
        if (targetObject == null)
        {
            return;
        }

        HumanMovement humanMovement = targetObject.GetComponent<HumanMovement>();
        if (humanMovement != null)
        {
            humanMovement.isGrabbed = grabbed;
        }

        HumanDeath humanDeath = targetObject.GetComponent<HumanDeath>();
        if (humanDeath)
        {
            humanDeath.attacker = gameObject;
            //StartCoroutine(ResetHumanGrabbed(humanDeath));
        }
    }

    private void ChangePlayerGrabbed(bool grabbed)
    {
        if (targetObject == null)
        {
            return;
        }

        PlayerMovement playerMovement = targetObject.GetComponent<PlayerMovement>();
        if (playerMovement != null)
        {
            playerMovement.canMove = !grabbed;
        }
    }

    private IEnumerator ResetHumanGrabbed(HumanDeath humanDeath)
    {
        yield return new WaitForSeconds(timeForHumanReset);
        humanDeath.attacker = null;
    }
}
