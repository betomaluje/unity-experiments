using UnityEngine;

public class Potion : MonoBehaviour
{
    [SerializeField] private LayerMask grabbableLayer;

    [SerializeField] private GameEvent healthEvent;

    private void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (TriggerUtils.CheckLayerMask(grabbableLayer, hitInfo.gameObject))
        {
            healthEvent.sentAttackEvent = new AttackEvent(hitInfo.gameObject, 1);
            healthEvent.Raise();

            Destroy(gameObject);
        }
    }
}
