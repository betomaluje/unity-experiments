using UnityEngine;

public class PlayerGrabbing : MonoBehaviour
{
    [Header("Layers")]
    [SerializeField] private LayerMask grabbableLayer;

    [Space]
    [Header("Settings")]
    // the players item grab position
    [SerializeField] private Transform itemGameObjectPosition;
    [SerializeField] private float throwForce = 250f;

    [SerializeField] private Camera cam;

    private Animator anim;
    private Rigidbody2D rb;

    private bool objectGrabbed = false;
    private GameObject targetObject;

    private Vector2 mousePos;
    private Plane plane = new Plane(Vector3.forward, Vector3.zero);

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }
    
    void Update()
    {
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButton(0))
        {
            anim.SetBool("isAttacking", true);

            if (targetObject != null)
            {
                DoGrab();
            }

        } else if (Input.GetMouseButtonUp(0) && objectGrabbed)
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            float enter;
            if (plane.Raycast(ray, out enter))
            {
                var hitPoint = ray.GetPoint(enter);
                var mouseDir = hitPoint - gameObject.transform.position;
                mouseDir = mouseDir.normalized;

                DoThrow(mouseDir);                
            }
            
        } else
        {
            anim.SetBool("isAttacking", false);
        }     
    }

    private void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (CheckLayerMask(hitInfo.gameObject))
        {
            targetObject = hitInfo.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D hitInfo)
    {
        if (CheckLayerMask(hitInfo.gameObject))
        {
            targetObject = null;
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

    public void DoGrab()
    {
        objectGrabbed = true;
        PutItemAsChild(targetObject);
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

        targetObject = null;
    }

    private bool CheckLayerMask(GameObject target)
    {
        return (grabbableLayer & 1 << target.layer) == 1 << target.layer;
    }
}
