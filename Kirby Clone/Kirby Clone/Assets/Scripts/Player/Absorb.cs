using DG.Tweening;
using UnityEngine;

public class Absorb : MonoBehaviour
{
    [Header("Layers")]
    public LayerMask absorbableLayer;

    [Space]
    [Header("Collision")]
    public float absorbRadius = 0.25f;
    public Vector2 grabOffset;

    [Space]
    [Header("Colors")]
    public Color targetColor;

    [Space]
    [Header("Settings")]
    // the players item grab position
    public Transform itemGameObjectPosition;
    public float throwForce = 250f;

    public ParticleSystem absorbParticles;

    private SkillsManager skillsManager;

    private Collider2D onGrabRight;

    private bool canAbsorb = false;
    private bool objectAbsorbed = false;
    private PlayerStats playerStats;

    private GameObject absorbObject;
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

        onGrabRight = Physics2D.OverlapCircle((Vector2)transform.position + grabOffset * direction, absorbRadius, absorbableLayer);

        if (onGrabRight)
        {
            absorbObject = onGrabRight.gameObject;
            canAbsorb = true;
            ChangeTargetColor(absorbObject);
        }
        else
        {
            ChangeTargetColorOriginal(absorbObject);
            originalTargetColor = Color.white;
            canAbsorb = false;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        Gizmos.DrawWireSphere((Vector2)transform.position + grabOffset * direction, absorbRadius);
    }

    private void ChangeTargetColor(GameObject target)
    {
        SpriteRenderer spriteRenderer = target.GetComponent<SpriteRenderer>();

        if (spriteRenderer == null)
        {
            return;
        }

        if (originalTargetColor == null)
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
    }

    public void DoAbsorbOrThrow()
    {
        if (skillsManager.hasSkill())
        {
            skillsManager.DoSkill();
        }
        else
        {
            if (objectAbsorbed)
            {
                // we need to throw it
                DoThrow();
            }
            else
            {
                // we need to absorb
                DoAbsorb();
            }
        }    
    }

    public void DoAbsorb()
    {
        if (!objectAbsorbed && canAbsorb && absorbObject != null)
        {
            Debug.Log("We absorb the item: " + absorbObject.name);            

            objectAbsorbed = true;

            Rigidbody2D rb = absorbObject.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                rb.simulated = false;
            }

            GameObject item = absorbObject;

            DOTween.Sequence()
                .Append(absorbObject.transform.DOShakePosition(0.4f, 0.5f, 5, 90, false))
                .Append(absorbObject.transform.DOMove(itemGameObjectPosition.position, 0.25f, false))
                .OnComplete(() =>
                {
                    PutItemAsChild(item);
                    GrabSkill(item);
                    absorbParticles.Play();
                });
        }
    }

    public void DoThrow()
    {
        if (absorbObject != null && objectAbsorbed)
        {
            absorbObject.transform.localScale = new Vector3(1, 1, 1);

            absorbObject.SetActive(true);

            Vector2 dir = transform.rotation * transform.right * direction;

            objectAbsorbed = false;

            absorbObject.transform.parent = null;

            Rigidbody2D rb = absorbObject.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                rb.simulated = true;
                rb.AddForce(dir * throwForce, ForceMode2D.Impulse);
            }

            RemoveItems();
        }
    }

    private void GrabSkill(GameObject item)
    {
        SkillContainer container = item.GetComponent<SkillContainer>();

        if (container != null)
        {
            skillsManager.AddSkill(container.getSkill());
        }

        item.SetActive(false);        
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
        absorbObject = null;

        foreach (Transform child in itemGameObjectPosition)
        {
            Destroy(child.gameObject);
        }
    }
}
