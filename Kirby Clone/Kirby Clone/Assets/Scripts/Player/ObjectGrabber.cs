using DG.Tweening;
using UnityEngine;

public class ObjectGrabber : MonoBehaviour
{
    [Header("Layers")]
    public LayerMask grabbableLayer;

    [Space]
    [Header("Collision")]
    public float grabRadius = 0.25f;
    public Vector2 grabOffset;

    [Space]
    [Header("Colors")]
    public Color targetColor;

    [Space]
    [Header("Settings")]
    // the players item grab position
    public Transform itemGameObjectPosition;
    public float throwForce = 250f;

    [SerializeField] private GameEvent objectGrabbedEvent;
    [SerializeField] private GameEvent objectThrownEvent;

    private SkillsManager skillsManager;

    private Collider2D onGrabRight;

    private bool canGrab = false;
    private bool objectGrabbed = false;
    private PlayerStats playerStats;

    private GameObject grabbedObject;
    private GameObject targetObject;
    private Color originalTargetColor;
    private PlayerMovement playerMovement;
    private int direction = 1;

    private void Awake()
    {
        playerStats = GetComponent<PlayerStats>();
        playerMovement = GetComponent<PlayerMovement>();
        skillsManager = GetComponent<SkillsManager>();
    }

    void Update()
    {
        direction = playerMovement.getDirection();

        onGrabRight = Physics2D.OverlapCircle((Vector2)transform.position + grabOffset * direction, grabRadius, grabbableLayer);

        if (onGrabRight)
        {
            targetObject = onGrabRight.gameObject;
            canGrab = true;
            ChangeTargetColor(targetObject);
        }
        else
        {
            ChangeTargetColorOriginal(targetObject);
            targetObject = null;
            originalTargetColor = Color.white;
            canGrab = false;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        Gizmos.DrawWireSphere((Vector2)transform.position + grabOffset * direction, grabRadius);
    }

    private void ChangeTargetColor(GameObject target)
    {
        SpriteRenderer spriteRenderer = target.GetComponent<SpriteRenderer>();

        if (spriteRenderer == null)
        {
            return;
        }

        if (originalTargetColor == null || originalTargetColor == Color.white)
        {
            originalTargetColor = spriteRenderer.color;
        }

        spriteRenderer.color = targetColor;
    }

    private void ChangeTargetColorOriginal(GameObject target)
    {
        if (target == null)
        {
            return;
        }

        SpriteRenderer spriteRenderer = target.GetComponent<SpriteRenderer>();

        if (spriteRenderer == null | originalTargetColor == null)
        {
            return;
        }

        spriteRenderer.color = originalTargetColor;
        originalTargetColor = Color.white;
    }

    public void DoGrabOrThrow()
    {
        if (skillsManager.hasSkill())
        {
            skillsManager.DoSkill(direction);
        }
        else
        {
            if (objectGrabbed)
            {
                // we need to throw it
                DoThrow();
            }
            else
            {
                // we need to absorb
                DoGrab();
            }
        }
    }

    public void DoGrab()
    {
        if (!objectGrabbed && canGrab)
        {
            grabbedObject = onGrabRight.gameObject;
            objectGrabbedEvent.Raise();
            Debug.Log("We grab the item: " + grabbedObject.name);
            SoundManager.instance.Play("StartAbsorb");

            objectGrabbed = true;

            GameObject item = grabbedObject;

            DOTween.Sequence()
                .Append(grabbedObject.transform.DOMove(itemGameObjectPosition.position, 0.25f, false))
                .OnComplete(() =>
                {                    
                    PutItemAsChild(item);
                    GrabSkill(item);
                    SoundManager.instance.Play("Absorb");                    
                });
        }
    }

    public void DoThrow()
    {
        if (grabbedObject != null && objectGrabbed)
        {
            objectThrownEvent.Raise();
            objectGrabbed = false;

            skillsManager.RemoveSkill();

            grabbedObject.transform.parent = null;

            Rigidbody2D rb = grabbedObject.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                rb.AddForce(new Vector2(throwForce * direction, 0), ForceMode2D.Impulse);
            }

            SoundManager.instance.Play("Throw");

            RemoveItems();
        }
    }

    private void GrabSkill(GameObject item)
    {
        SkillContainer container = item.GetComponent<SkillContainer>();

        if (container != null)
        {
            skillsManager.AddSkill(container.getSkill(), gameObject);           
        }        
    }

    private void PutItemAsChild(GameObject item)
    {
        item.transform.parent = null;
        item.transform.parent = itemGameObjectPosition;
        item.transform.localPosition = Vector3.zero;

        Quaternion rotation = Quaternion.Euler(0, itemGameObjectPosition.rotation.y, 0);
        item.transform.rotation = rotation;
    }

    private void RemoveItems()
    {
        grabbedObject = null;

        foreach (Transform child in itemGameObjectPosition)
        {
            Destroy(child.gameObject);
        }
    }
}
