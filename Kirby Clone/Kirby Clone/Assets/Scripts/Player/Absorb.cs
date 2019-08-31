using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Absorb : MonoBehaviour
{
    [Header("Layers")]
    public LayerMask absorbableLayer;

    [Space]
    [Header("Collision")]
    public float absorbRadius = 0.25f;
    public Vector2 rightOffset, leftOffset;
    
    private bool onGrabRight;
    private bool onGrabLeft;

    private bool canAbsorb = false;
    private bool objectAbsorbed = false;
    private PlayerStats playerStats;

    private GameObject absorbObject;

    private void Awake()
    {
        playerStats = GetComponent<PlayerStats>();
    }

    void Update()
    {
        onGrabRight = Physics2D.OverlapCircle((Vector2)transform.position + rightOffset, absorbRadius, absorbableLayer);
        onGrabLeft = Physics2D.OverlapCircle((Vector2)transform.position + leftOffset, absorbRadius, absorbableLayer);
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

        Gizmos.DrawWireSphere((Vector2)transform.position + rightOffset, absorbRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + leftOffset, absorbRadius);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!canAbsorb && playerStats.isPlayerActive() && CheckLayerMask(other.gameObject))
        {
            absorbObject = other.gameObject;
            canAbsorb = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (canAbsorb && playerStats.isPlayerActive() && CheckLayerMask(other.gameObject))
        {
            canAbsorb = false;
        }
    }

    private bool CheckLayerMask(GameObject target)
    {
        return (absorbableLayer & 1 << target.layer) == 1 << target.layer;
    }

    private void DoAbsorb()
    {
        Debug.Log("Absorbing");
    }
}
