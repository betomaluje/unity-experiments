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

    private PlayerMovement playerMovement;

    // animations
    private bool isTorning = false;
    private bool buttonPressed = false;
    private bool buttonUp = false;

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
                SetDirection(Vector2.left);
                break;
            case PlayerMovement.Direction.RIGHT:
                SetDirection(Vector2.right);
                break;
            case PlayerMovement.Direction.UP:
                SetDirection(Vector2.up);
                break;
            case PlayerMovement.Direction.DOWN:
                SetDirection(Vector2.down);
                break;
        }

        if (onTargetDetected)
        {
            targetObject = onTargetDetected.gameObject;
        }

        Animate();
        CheckInput();        
    }    

    private void CheckInput()
    {
        buttonPressed = Input.GetButton("Fire1");
        buttonUp = Input.GetButtonUp("Fire1");

        if (buttonPressed)
        {
            timePressing += Time.deltaTime;

            if (targetObject != null)
            {                
                DoGrab();
            }
        }

        if (buttonUp && targetObject != null)
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
            Debug.Log("Fail torning");
            return;
        }                

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
        PutItemAsChild(targetObject);
        ChangeHumanGrabbed(true);
    }

    public void DoThrow(Vector3 dir)
    {
        if (targetObject == null) return;

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
        HumanMovement humanMovement = targetObject.GetComponent<HumanMovement>();
        if (humanMovement != null)
        {            
            humanMovement.isGrabbed = grabbed;
        }
    }
}
