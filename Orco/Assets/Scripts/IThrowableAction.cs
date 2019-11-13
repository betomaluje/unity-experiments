using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class IThrowableAction: MonoBehaviour
{
    public interface IThrownCollision {
        void OnThrownCollision(GameObject collisionObject);
    }

    [SerializeField] protected LayerMask throwableDeathLayer;

    [SerializeField] private float timeToResetThrow = 1f;

    [HideInInspector]
    protected bool isBeingThrown = false;

    protected IThrownCollision onCollisionListener;

    private IThrownCollision OnCollisionListener { get => onCollisionListener; set => onCollisionListener = value; }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isBeingThrown && TriggerUtils.CheckLayerMask(throwableDeathLayer, collision.gameObject))
        {
            OnCollisionListener.OnThrownCollision(collision.gameObject);
        }
    }

    public void Throw(Vector3 dir, float throwForce)
    {
        StartCoroutine(ToggleThrowTimer(dir, throwForce));
    }

    protected IEnumerator ToggleThrowTimer(Vector3 dir, float throwForce)
    {
        isBeingThrown = true;

        Rigidbody2D targetRb = GetComponent<Rigidbody2D>();
        if (targetRb != null)
        {
            targetRb.AddForce(dir * throwForce, ForceMode2D.Impulse);
        }

        yield return new WaitForSeconds(timeToResetThrow);        
        isBeingThrown = false;
    }

}
