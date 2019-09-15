using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public LayerMask targetLayerMask;
    public GameEvent hitEvent;

    private void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (CheckLayerMask(hitInfo.gameObject))
        {        
            float damage = Random.Range(1, 10);
            hitEvent.sentAttackEvent = new AttackEvent(hitInfo.gameObject, damage);
            hitEvent.Raise();
        }

        // we destroy this bullet
        Destroy(gameObject);
    }

    private bool CheckLayerMask(GameObject target)
    {
        return (targetLayerMask & 1 << target.layer) == 1 << target.layer;
    }
}
