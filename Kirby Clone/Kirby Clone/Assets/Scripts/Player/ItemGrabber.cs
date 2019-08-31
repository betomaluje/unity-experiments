using System.Collections;
using UnityEngine;

public class ItemGrabber : MonoBehaviour
{
    [Header("Layers")]
    public LayerMask itemsLayer;

    [Space]
    [Header("Settings")]
    // the players item grab position
    public Transform itemGameObjectPosition;
    public float throwForce = 250f;

    [Space]
    [Header("Collision")]
    public float grabRadius = 0.25f;
    public Vector2 rightOffset, leftOffset;   

    private PlayerStats playerStats;
    private bool onGrabRight;
    private bool onGrabLeft;
    private bool canBeGrabbed = false;    
    private bool itemGrabbed = false;
    private bool itemthrown = false;

    private GameObject itemObject;

    private void Awake()
    {
        playerStats = GetComponent<PlayerStats>();
    } 

    void Update()
    {
        onGrabRight = Physics2D.OverlapCircle((Vector2)transform.position + rightOffset, grabRadius, itemsLayer);
        onGrabLeft = Physics2D.OverlapCircle((Vector2)transform.position + leftOffset, grabRadius, itemsLayer);
    }

    private void FixedUpdate()
    {
        if (itemObject != null)
        {
            if (itemGrabbed)
            {
                Grab();
                itemGrabbed = false;
                itemthrown = false;
            }

            if (itemthrown)
            {
                // we need to throw it
                Throw();
                itemObject = null;
                canBeGrabbed = false;

                itemGrabbed = false;
                itemthrown = false;
            }            
        }        
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        Gizmos.DrawWireSphere((Vector2)transform.position + rightOffset, grabRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + leftOffset, grabRadius);
    } 

    public void GrabOrThrow()
    {
        if (itemObject != null)
        {
            if (canBeGrabbed && (onGrabRight || onGrabLeft))
            {
                itemGrabbed = true;
            }
            else if (itemGameObjectPosition.childCount > 0)
            {
                itemthrown = true;
            }
        }
    }

    private void Grab()
    {
        if (itemObject != null && itemGameObjectPosition != null && itemGameObjectPosition.childCount < 1)
        {
            Debug.Log("We grabbed the item: " + itemObject.name);
            canBeGrabbed = false;

            SoundManager.instance.Play("BoxPickup");

            Rigidbody2D rb = itemObject.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                rb.simulated = false;
            }

            PutItemAsChild(itemObject);
        }       
    }

    public void Throw()
    {
        if (itemObject != null)
        {
            Debug.Log("We throw the item: " + itemObject.name);
            SoundManager.instance.Play("BoxThrow");

            itemObject.transform.localScale = new Vector3(1, 1, 1);

            Vector2 direction = transform.rotation * transform.right;
            direction.Normalize();

            itemObject.transform.parent = null;

            Rigidbody2D rb = itemObject.GetComponent<Rigidbody2D>();

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
		itemObject = null;

		foreach (Transform child in itemGameObjectPosition)
		{
			Destroy(child.gameObject);
		}
	}

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!canBeGrabbed && playerStats.isPlayerActive() && CheckLayerMask(other.gameObject))
        {
            itemObject = other.gameObject;
            canBeGrabbed = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (canBeGrabbed && playerStats.isPlayerActive() && CheckLayerMask(other.gameObject))
        {
            canBeGrabbed = false;
        }
    }

    private bool CheckLayerMask(GameObject target)
    {
        return (itemsLayer & 1 << target.layer) == 1 << target.layer;
    }
}
