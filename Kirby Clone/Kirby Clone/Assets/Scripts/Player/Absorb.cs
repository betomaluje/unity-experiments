using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

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
        } else
        {
            ChangeTargetColorOriginal(absorbObject);
            absorbObject = null;
            originalTargetColor = Color.white;
            canAbsorb = false;
        }        
    }

    private void FixedUpdate()
    {
        if (absorbObject != null)
        {
            if (objectAbsorbed)
            {
                DoAbsorb();
            }            
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
        if (objectAbsorbed)
        {
            // we need to throw it
            DoThrow();
        } else
        {
            // we need to absorb
            DoAbsorb();
        }
    }

    private void DoAbsorb()
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
            absorbObject.transform.DOMove(itemGameObjectPosition.position, 0.25f, false).OnComplete(() => PutItemAsChild(item));                        
        }
    }

    private void DoThrow()
    {
        if (absorbObject != null && objectAbsorbed)
        {
            absorbObject.transform.localScale = new Vector3(1, 1, 1);

            Vector2 direction = transform.rotation * transform.right;
            direction.Normalize();

            objectAbsorbed = false;

            absorbObject.transform.parent = null;

            Rigidbody2D rb = absorbObject.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                rb.simulated = true;
                rb.AddForce(direction * throwForce, ForceMode2D.Impulse);
            }

            RemoveItems();
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
        absorbObject = null;

        foreach (Transform child in itemGameObjectPosition)
        {
            Destroy(child.gameObject);
        }
    }
}
