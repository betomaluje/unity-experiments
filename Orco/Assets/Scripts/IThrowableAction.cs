using UnityEngine;
using System.Collections;

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

    public void Throw()
    {
        StartCoroutine(ToggleThrowTimer());
    }

    protected IEnumerator ToggleThrowTimer()
    {
        isBeingThrown = true;
        yield return new WaitForSeconds(timeToResetThrow);        
        isBeingThrown = false;
    }

}
