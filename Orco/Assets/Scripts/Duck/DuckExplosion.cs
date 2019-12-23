using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuckExplosion : ThrowableAction, ThrowableAction.IThrownCollision
{
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private float radius = 4;
    [SerializeField] private float damage = 10;
    [Range(0, 10)]
    [SerializeField] private float explosionForce = 10;   
    [SerializeField] private GameObject particles;
    [SerializeField] private GameEvent explosionEvent;    

    private void Awake()
    {
        explosionForce *= 10000;
        onCollisionListener = this;
    }

    public void OnThrownCollision(GameObject collisionObject)
    {
        Debug.Log("bomb collision with " + collisionObject.gameObject);
        targetLayer |= (1 << collisionObject.layer);
        Explode();
    }
    private void Explode()
    {
        Collider2D collider = Physics2D.OverlapCircle(transform.position, radius, targetLayer);
        if (collider != null)
        {
            float proximity = (transform.position - collider.gameObject.transform.position).magnitude;
            float actualDamage = (damage * proximity);
            //Debug.Log("proximity " + proximity);
            //Debug.Log(collider.gameObject.name + " took " + actualDamage + "  damage");

            Rigidbody2D rb = collider.gameObject.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                AddExplosionForce(rb, explosionForce, transform.position, radius * 5);
            }

            if (collider.gameObject.tag == "Player")
            {
                explosionEvent.sentAttackEvent = new AttackEvent(collider.gameObject, Mathf.Ceil(proximity));
                explosionEvent.Raise();
            }
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
}
