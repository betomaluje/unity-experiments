using UnityEngine;

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
    private bool buttonUp = false;

    private PlayerMovement playerMovement;

    private PlayerInputActions inputActions;

    private void Awake()
    {
        inputActions = new PlayerInputActions();
        
        inputActions.Player.ActionX.performed += context => OnActionX((float)context.ReadValueAsObject());
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
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

    public void OnActionX(float action)
    {
        buttonPressed = action == 1;
        buttonUp = action == 0;
    }

    private void CheckInput()
    {        
        if (buttonPressed)
        {            
            if (onTargetDetected && targetObject == null)
            {
                timePressing += Time.deltaTime;
                SoundManager.instance.PlayRandom("Orc Growl");
                DoGrab();
            }
        } else if (buttonUp && targetObject != null)
        {
            if (timePressing >= timeForLongPress && !isTorning)
            {
                isTorning = true;
                DoTorn();
            }
            else
            {
                DoThrow(itemGameObjectPosition.position - transform.position);
            }
            timePressing = 0;
        }
    }

    private void Animate()
    {
        anim.SetBool("isAttacking", buttonPressed);
        anim.SetBool("isTorning", isTorning);
    }  

    private void PutItemAsChild(GameObject item)
    {        
        item.transform.parent = null;
        item.transform.parent = itemGameObjectPosition;
        item.transform.localPosition = Vector3.zero;

        Quaternion rotation = Quaternion.Euler(0, itemGameObjectPosition.rotation.y, 0);
        item.transform.rotation = rotation;
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

        objectGrabbed = false;

        targetObject.transform.parent = null;
        targetObject = null;
        isTorning = false;
    }

    public void DoGrab()
    {        
        objectGrabbed = true;
        targetObject = onTargetDetected.gameObject;
        PutItemAsChild(targetObject);
        ChangeHumanGrabbed(true);
    }

    public void DoThrow(Vector3 dir)
    {
        if (targetObject == null) return;

        SoundManager.instance.PlayRandom("Orc Growl");

        objectGrabbed = false;    

        Rigidbody2D targetRb = targetObject.GetComponent<Rigidbody2D>();

        if (targetRb != null)
        {
            targetRb.AddForce(dir * throwForce, ForceMode2D.Impulse);
        }

        ChangeHumanGrabbed(false);

        targetObject.transform.parent = null;
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
