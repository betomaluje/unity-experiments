using UnityEngine;

public class DeathDetector : MonoBehaviour
{
    [Header("Layers")]
    [SerializeField] private LayerMask deathLayer;

    [SerializeField] private GameEvent sacrificedEvent;

    [SerializeField] private GameEvent playerDeathEvent;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (CheckLayerMask(collision.gameObject)) {
            sacrificedEvent.sentGameObject = collision.gameObject;
            sacrificedEvent.Raise();
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            playerDeathEvent.Raise();
        } else
        {
            Destroy(collision.gameObject);
        }       
    }

    private bool CheckLayerMask(GameObject target)
    {
        return (deathLayer & 1 << target.layer) == 1 << target.layer;
    }
}
