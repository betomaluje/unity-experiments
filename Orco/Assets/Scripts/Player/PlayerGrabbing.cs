using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGrabbing : TargetDetection
{
    [Space]
    [Header("Settings")]
    // the players item grab position
    [SerializeField] private Transform itemGameObjectPosition;
    [SerializeField] private float throwForce = 250f;

    [SerializeField] private float timeForLongPress = 0.5f;

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

            if (!objectGrabbed && onTargetDetected != null)
            {                
                SoundManager.instance.PlayRandom("Orc Growl");
                targetObject = onTargetDetected.gameObject;

                objectGrabbed = true;
                ChangeHumanGrabbed(true);
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
                else
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
            humanDeath.TornApart();
        }

        ReleaseTarget();

        isTorning = false;
    }

    public void DoThrow(Vector3 dir)
    {
        if (targetObject == null) return;

        SoundManager.instance.PlayRandom("Orc Growl");
        
        ChangeHumanGrabbed(false);        

        IThrowableAction throwableAction = targetObject.GetComponent<IThrowableAction>();
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
    }
}
