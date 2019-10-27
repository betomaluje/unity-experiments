using UnityEngine;

public class HumanMovement : TargetDetection
{
    [SerializeField] private float accelerationTime = 2f;
    [SerializeField] private float maxSpeed = 5f;

    [HideInInspector]
    public bool isGrabbed = false;

    private Vector2 movement;
    private float timeLeft;
    private bool isPlayerNear = false;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        isGrabbed = false;
        isPlayerNear = false;
    }

    public override void Update()
    {
        base.Update();
        isPlayerNear = onTargetDetected;

        if (!isPlayerNear || isGrabbed)
        {
            return;
        }
       
        timeLeft -= Time.deltaTime;
        if (timeLeft <= 0)
        {
            movement = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            timeLeft += accelerationTime;
        }
    }

    void FixedUpdate()
    {
        if (!isPlayerNear || isGrabbed)
        {
            return;
        }

        rb.AddForce(movement * maxSpeed);
    }
}
