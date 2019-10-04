using DG.Tweening;
using UnityEngine;
using System.Collections;

public class ObjectGrabber : MonoBehaviour
{
    [Header("Layers")]
    [SerializeField] private LayerMask grabbableLayer;

    [Space]
    [Header("Collision")]
    [SerializeField] private float grabRadius = 0.25f;
    [SerializeField] private Vector2 grabOffset;

    [Space]
    [Header("Colors")]
    [SerializeField] private Color targetColor;
    [SerializeField] private Color debugColor;

    [Space]
    [Header("Settings")]
    // the players item grab position
    [SerializeField] private Transform itemGameObjectPosition;
    [SerializeField] private float throwForce = 250f;

    [SerializeField] private GameEvent objectGrabbedEvent;
    [SerializeField] private GameEvent objectThrownEvent;

    private SkillsManager skillsManager;

    private Collider2D onGrabRight;

    private bool objectGrabbed = false;
    private PlayerStats playerStats;

    private GameObject targetObject;
    private Color originalTargetColor;
    private PlayerMovement playerMovement;
    private int direction = 1;

    private void Awake()
    {
        playerStats = GetComponentInParent<PlayerStats>();
        playerMovement = GetComponentInParent<PlayerMovement>();
        skillsManager = GetComponentInParent<SkillsManager>();
    }

    void Update()
    {        
        direction = playerMovement.getDirection();

        if (targetObject != null) {
            UpdateTargetPosition(targetObject);
            return;
        }

        onGrabRight = Physics2D.OverlapCircle((Vector2)transform.position + grabOffset * direction, grabRadius, grabbableLayer);
    }

    private void UpdateTargetPosition(GameObject target) {
        target.transform.position = itemGameObjectPosition.transform.position;
        Quaternion rotation = Quaternion.Euler(0, itemGameObjectPosition.rotation.y, 0);
        target.transform.rotation = rotation;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = debugColor;

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
        if (targetObject == null && !objectGrabbed && onGrabRight)
        {
            targetObject = onGrabRight.gameObject;

            objectGrabbedEvent.Raise();
            
            SoundManager.instance.Play("StartAbsorb");

            StartCoroutine(ToggleGrab(true));

            GameObject item = targetObject;

            DOTween.Sequence()
                .Append(item.transform.DOMove(itemGameObjectPosition.position, 0.25f, false))
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
        if (targetObject != null && objectGrabbed)
        {
            objectThrownEvent.Raise();

            skillsManager.RemoveSkill();

            Rigidbody2D rb = targetObject.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                rb.AddForce(new Vector2(throwForce * direction, 5), ForceMode2D.Impulse);
            }

            targetObject.transform.parent = null;

            SoundManager.instance.Play("Throw");

            RemoveItems();

            StartCoroutine(ToggleGrab(false));
        }
    }

    private IEnumerator ToggleGrab(bool grabbed) {
        yield return new WaitForSeconds(0.5f);
        objectGrabbed = grabbed;
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
        targetObject = null;

        foreach (Transform child in itemGameObjectPosition)
        {
            Destroy(child.gameObject);
        }
    }
}
