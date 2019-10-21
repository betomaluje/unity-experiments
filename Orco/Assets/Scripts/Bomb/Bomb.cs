using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float radius = 4;
    [SerializeField] private float damage = 10;
    [Range(0, 10)]
    [SerializeField] private float explosionForce = 10;
    [SerializeField] private CircleCollider2D circleCollider;
    [SerializeField] private GameObject particles;
    [SerializeField] private GameEvent explosionEvent;

    private Animator anim;
    private GameObject target;    

    private void Start()
    {
        anim = GetComponent<Animator>();
        circleCollider.radius = radius;
        explosionForce *= 10000;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (TriggerUtils.CheckLayerMask(playerLayer, collision.gameObject))
        {
            anim.SetTrigger("Activate");
            target = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (TriggerUtils.CheckLayerMask(playerLayer, collision.gameObject))
        {
            target = null;
        }
    }

    private void Explode()
    {
        if (target == null) return;

        Collider2D collider = Physics2D.OverlapCircle(transform.position, radius, playerLayer);
        if (collider.gameObject.tag == "Player")
        {
            float proximity = (transform.position - target.transform.position).magnitude;
            float actualDamage = (damage * proximity);
            Debug.Log("proximity " + proximity);
            Debug.Log(collider.gameObject.name + " took " + actualDamage + "  damage");

            Rigidbody2D rb = target.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                AddExplosionForce(rb, explosionForce, transform.position, radius*5);
            }

            explosionEvent.sentFloat = Mathf.Ceil(proximity);
            explosionEvent.Raise();
        }

        Instantiate(particles, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private void AddExplosionForce(Rigidbody2D body, float explosionForce, Vector3 explosionPosition, float explosionRadius)
    {
        var dir = (body.transform.position - explosionPosition);
        float wearoff = 1 - (dir.magnitude / explosionRadius);
        body.AddForce(dir.normalized * (wearoff <= 0f ? 0f : explosionForce) * wearoff, ForceMode2D.Force);
    }

    public void AddExplosionForce2(Rigidbody2D rb, float explosionForce, Vector2 explosionPosition, float explosionRadius, float upwardsModifier = 0.0F, ForceMode2D mode = ForceMode2D.Force)
    {
        var explosionDir = rb.position - explosionPosition;
        var explosionDistance = explosionDir.magnitude;

        // Normalize without computing magnitude again
        if (upwardsModifier == 0)
            explosionDir /= explosionDistance;
        else
        {
            // From Rigidbody.AddExplosionForce doc:
            // If you pass a non-zero value for the upwardsModifier parameter, the direction
            // will be modified by subtracting that value from the Y component of the centre point.
            explosionDir.y += upwardsModifier;
            explosionDir.Normalize();
        }

        rb.AddForce(Mathf.Lerp(0, explosionForce, (1 - explosionDistance)) * explosionDir, mode);
    }

    private void OnDrawGizmos()
    {
        UnityEditor.Handles.color = Color.yellow;
        UnityEditor.Handles.DrawWireDisc(circleCollider.transform.position, Vector3.back, radius);
    }
}
